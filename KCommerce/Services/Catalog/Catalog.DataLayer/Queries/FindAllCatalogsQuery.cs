using MediatR;
using System.Collections.Generic;
using Catalog.DataLayer.Dtos;

namespace Catalog.DataLayer.Queries
{
	public class FindAllCatalogsQuery : IRequest<IEnumerable<ShortCatalogDto>>
	{
	}
}
