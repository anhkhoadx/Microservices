using Catalog.DataLayer.Commands;
using Catalog.DataLayer.DatabaseModels;
using Catalog.DataLayer.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.DataLayer.DataAccess
{
	public class CatalogRepository : ICatalogRepository
	{
		public IUnitOfWork UnitOfWork => _catalogContext;

		private readonly CatalogContext _catalogContext;
		private readonly AddCatalogCommandValidator _commandValidator = new AddCatalogCommandValidator();

		public CatalogRepository(CatalogContext catalogContext)
		{
			_catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
		}

		public async Task<List<DatabaseModels.Catalog>> FindAllCatalogs()
		{
			return await _catalogContext.Catalogs.Include(k => k.CatalogBrand)
				.Include(k => k.CatalogType)
				.Where(p => p.Status == CatalogStatus.Active)
				.ToListAsync();
		}

		public async Task<DatabaseModels.Catalog> FindById(Guid id)
		{
			return await _catalogContext.Catalogs.Include(c => c.CatalogBrand)
				.Include(k => k.CatalogType)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public DatabaseModels.Catalog Add(DatabaseModels.Catalog catalog)
		{
			return _catalogContext.Catalogs.Add(catalog).Entity;
		}

		public void ValidateAddCatalogCommand(AddCatalogCommand command)
		{
			_commandValidator.ValidateAndThrow(command);

			GetCatalogBrand(command.CatalogBrandId);
			GetCatalogType(command.CatalogTypeId);
		}

		private CatalogBrand GetCatalogBrand(Guid id)
		{
			var catalogBrand = _catalogContext.CatalogBrands.FirstOrDefault(k => k.Id == id);

			return catalogBrand ?? throw new ArgumentNullException(nameof(catalogBrand), "Invalid catalog brand");
		}

		private CatalogType GetCatalogType(Guid id)
		{
			var catalogType = _catalogContext.CatalogTypes.FirstOrDefault(k => k.Id == id);

			return catalogType ?? throw new ArgumentNullException(nameof(catalogType), "Invalid catalog type");
		}
	}
}
