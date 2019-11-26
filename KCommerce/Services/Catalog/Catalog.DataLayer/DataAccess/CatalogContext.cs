using Catalog.DataLayer.DatabaseModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Shared.DataLayer;
using Shared.DataLayer.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.DataLayer.DataAccess
{
	public class CatalogContext : DbContext, IUnitOfWork
	{
		public DbSet<DatabaseModels.Catalog> Catalogs { get; set; }
		public DbSet<CatalogBrand> CatalogBrands { get; set; }
		public DbSet<CatalogType> CatalogTypes { get; set; }

		public bool HasActiveTransaction => _currentTransaction != null;

		private IDbContextTransaction _currentTransaction;
		private readonly IMediator _mediator;
		private readonly IConfigurationRoot _configRoot;

		public CatalogContext(IConfigurationRoot configRoot, IMediator mediator)
		{
			_configRoot = configRoot;
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.UseHiLo();
			
			base.OnModelCreating(modelBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_configRoot.GetConnectionString("DefaultConnection"));
			
			base.OnConfiguring(optionsBuilder);
		}

		public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

		public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			// Dispatch Domain Events collection. 
			// Choices:
			// A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
			// side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
			// B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
			// You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
			await DispatchDomainEventsAsync(_mediator, this);

			// After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
			// performed through the DbContext will be committed
			var result = await base.SaveChangesAsync(cancellationToken);

			return true;
		}

		public async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			if (_currentTransaction != null) return null;

			_currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

			return _currentTransaction;
		}

		public async Task CommitTransactionAsync(IDbContextTransaction transaction)
		{
			if (transaction == null) throw new ArgumentNullException(nameof(transaction));
			if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

			try
			{
				await SaveChangesAsync();
				transaction.Commit();
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
			finally
			{
				if (_currentTransaction != null)
				{
					_currentTransaction.Dispose();
					_currentTransaction = null;
				}
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				_currentTransaction?.Rollback();
			}
			finally
			{
				if (_currentTransaction != null)
				{
					_currentTransaction.Dispose();
					_currentTransaction = null;
				}
			}
		}

		public async Task DispatchDomainEventsAsync(IMediator mediator, CatalogContext ctx)
		{
			var domainEntities = ctx.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

			var domainEvents = domainEntities
				.SelectMany(x => x.Entity.DomainEvents)
				.ToList();

			domainEntities.ToList()
				.ForEach(entity => entity.Entity.ClearDomainEvents());

			var tasks = domainEvents
				.Select(async (domainEvent) => {
					await mediator.Publish(domainEvent);
				});

			await Task.WhenAll(tasks);
		}
	}
}
