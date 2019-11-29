using Chat.DataLayer.DatabaseModels;
using Marten;
using System;
using System.Threading.Tasks;

namespace Chat.DataLayer.DataAccess
{
	public class ChatRepository : IChatRepository
	{
		private readonly IDocumentSession _documentSession;

		public ChatRepository(IDocumentSession documentSession)
		{
			_documentSession = documentSession;
		}

		public void Add(Room room)
		{
			_documentSession.Insert(room);
		}

		public void Update(Room room)
		{
			_documentSession.Update(room);
		}

		public async Task<Room> FindById(Guid roomId)
		{
			return await _documentSession
				.Query<Room>()
				.FirstOrDefaultAsync(p => p.Id == roomId);
		}
	}
}
