using Shared.DataLayer.Models;
using System;

namespace Chat.DataLayer.DatabaseModels
{
	public class Message : Entity
	{
		public Guid UserId { get; set; }
		public Guid RoomId { get; set; }
		public string Content { get; set; }
		public DateTime SentDate { get; set; }
	}
}
