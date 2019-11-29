using Chat.DataLayer.DatabaseModels;
using System;
using System.Threading.Tasks;

namespace Chat.DataLayer.DataAccess
{
	public interface IChatRepository
	{
		void Add(Room room);

		void Update(Room room);

		Task<Room> FindById(Guid roomId);
	}
}
