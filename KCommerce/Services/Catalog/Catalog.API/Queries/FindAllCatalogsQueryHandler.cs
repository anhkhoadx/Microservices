using Catalog.DataLayer.DataAccess;
using Catalog.DataLayer.Dtos;
using Catalog.DataLayer.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Queries
{
	public class FindAllCatalogsQueryHandler : IRequestHandler<FindAllCatalogsQuery, IEnumerable<ShortCatalogDto>>
	{
		private readonly ICatalogRepository _catalogRepository;

		public FindAllCatalogsQueryHandler(ICatalogRepository catalogRepository)
		{
			_catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
		}

		public async Task<IEnumerable<ShortCatalogDto>> Handle(FindAllCatalogsQuery request, CancellationToken cancellationToken)
		{
			var result = await _catalogRepository.FindAllCatalogs();

			return result.Select(k => new ShortCatalogDto
			{
				CatalogId = k.Id,
				Name = k.Name,
				Price = k.Price,
				AvailableStock = k.AvailableStock,
				PictureUrl = k.PictureUrl
			}).ToList();
		}
	}
}
