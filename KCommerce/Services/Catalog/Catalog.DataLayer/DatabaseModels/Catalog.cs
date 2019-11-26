using System;
using Shared.DataLayer;
using Shared.DataLayer.Models;

namespace Catalog.DataLayer.DatabaseModels
{
	public class Catalog : Entity, IAggregateRoot
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public string PictureUrl { get; set; }

		public Guid CatalogTypeId { get; set; }

		public CatalogStatus Status { get; set; }

		public virtual CatalogType CatalogType { get; set; }

		public Guid CatalogBrandId { get; set; }

		public virtual CatalogBrand CatalogBrand { get; set; }

		public int AvailableStock { get; set; }
	}
}
