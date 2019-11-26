using System;
using MediatR;

namespace Catalog.DataLayer.Commands
{
	public class AddCatalogCommand : IRequest<bool>
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public byte[] PictureBytes { get; set; }

		public Guid CatalogTypeId { get; set; }

		public Guid CatalogBrandId { get; set; }

		public int AvailableStock { get; set; }
	}
}
