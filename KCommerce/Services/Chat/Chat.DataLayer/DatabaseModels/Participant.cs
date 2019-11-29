using Shared.DataLayer.Models;
using System;

namespace Chat.DataLayer.DatabaseModels
{
	public class Participant : Entity
	{
		public Guid UserId { get; set; }
		public Guid RoomId { get; set; }
	}
}
