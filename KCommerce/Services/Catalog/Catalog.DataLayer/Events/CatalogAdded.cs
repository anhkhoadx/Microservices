using System;

namespace Catalog.DataLayer.Events
{
	public class CatalogAdded
	{
		public Guid UserId { get; set; }

		public Guid CatalogId { get; set; }

		public int Quantity { get; set; }
	}
}
