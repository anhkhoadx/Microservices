using Shared.DataLayer.Models;

namespace Chat.DataLayer.DatabaseModels
{
	public class Room : Entity
	{
		public string Name { get; set; }
		public bool IsPrivate { get; set; }
	}
}
