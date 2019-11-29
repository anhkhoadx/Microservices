using Chat.DataLayer.DataAccess.Marten;
using Chat.DataLayer.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Chat.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly IDataStore _dataSource;

		public ChatController(IDataStore dataSource)
		{
			_dataSource = dataSource;
		}

		[HttpGet("find")]
		public async Task<Room> FindRoom(Guid roomId)
		{
			return await _dataSource.ChatRepository.FindById(roomId);
		}

		[HttpPost]
		public void AddRoom([FromBody]string roomName)
		{
			var room = new Room
			{
				Name = roomName,
				IsPrivate = true
			};

			_dataSource.ChatRepository.Add(room);
			_dataSource.CommitChanges();
		}
	}
}
