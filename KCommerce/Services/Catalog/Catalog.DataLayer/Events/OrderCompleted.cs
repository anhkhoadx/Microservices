using System;

namespace Catalog.DataLayer.Events
{
	public class OrderCompleted
	{
		public Guid OrderId { get; set; }
	}
}
