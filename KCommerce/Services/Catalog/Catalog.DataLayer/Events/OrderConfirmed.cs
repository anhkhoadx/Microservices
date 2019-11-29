using System;

namespace Catalog.DataLayer.Events
{
	public class OrderConfirmed
	{
		public Guid OrderId { get; set; }
	}
}
