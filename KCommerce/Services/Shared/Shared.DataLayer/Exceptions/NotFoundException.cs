using System;

namespace Shared.DataLayer.Exceptions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string entityName, int key)
			: base($"Entity {entityName} with primary key {key} was not found.")
		{
		}
	}
}
