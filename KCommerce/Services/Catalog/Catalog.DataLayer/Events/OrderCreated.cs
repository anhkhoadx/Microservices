using System;

namespace Catalog.DataLayer.Events
{
	public class OrderCreated
	{
		public Guid UserId { get; set; }

		public Guid OrderId { get; set; }

		public DatabaseModels.Catalog[] Catalogs { get; set; }

		public float TotalMoney { get; set; }
	}
}
