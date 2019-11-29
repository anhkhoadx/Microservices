using Shared.DataLayer.Models;
using System;

namespace Chat.DataLayer.DatabaseModels
{
	public class User : Entity
	{
		public Guid UserId { get; set; }
	}
}
