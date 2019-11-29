using System;

namespace Catalog.DataLayer.Events
{
	public class OrderShipped
	{
		public Guid OrderId { get; set; }
	}
}
