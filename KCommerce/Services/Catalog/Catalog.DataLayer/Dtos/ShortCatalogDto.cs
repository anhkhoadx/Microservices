using System;

namespace Catalog.DataLayer.Dtos
{
	public class ShortCatalogDto
	{
		public Guid CatalogId { get; set; }

		public string Name { get; set; }
		
		public decimal Price { get; set; }

		public string PictureUrl { get; set; }

		public int AvailableStock { get; set; }
	}
}
