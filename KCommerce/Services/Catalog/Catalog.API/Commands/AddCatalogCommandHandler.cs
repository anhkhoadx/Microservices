using Catalog.DataLayer;
using Catalog.DataLayer.Commands;
using Catalog.DataLayer.DataAccess;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Commands
{
	public class AddCatalogCommandHandler : IRequestHandler<AddCatalogCommand, bool>
	{
		private readonly ICatalogRepository _catalogRepository;

		public AddCatalogCommandHandler(ICatalogRepository catalogRepository)
		{
			_catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
		}

		public async Task<bool> Handle(AddCatalogCommand request, CancellationToken cancellationToken)
		{
			_catalogRepository.ValidateAddCatalogCommand(request);

			// TODO: save picture and return url
			var catalog = new DataLayer.DatabaseModels.Catalog
			{
				CatalogTypeId = request.CatalogTypeId,
				CatalogBrandId = request.CatalogBrandId,
				Name = request.Name,
				AvailableStock = request.AvailableStock,
				Description = request.Description,
				Price = request.Price,
				Status = CatalogStatus.Active
			};

			_catalogRepository.Add(catalog);

			return await _catalogRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
		}
	}
}
