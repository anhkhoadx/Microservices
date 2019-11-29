using System;

namespace Catalog.DataLayer.Events
{
	public class OrderStatusChangeToPaid
	{
		public Guid OrderId { get; set; }
	}
}
