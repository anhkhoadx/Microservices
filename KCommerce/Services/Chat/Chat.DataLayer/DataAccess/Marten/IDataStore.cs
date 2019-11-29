using System;
using System.Threading.Tasks;

namespace Chat.DataLayer.DataAccess.Marten
{
	public interface IDataStore : IDisposable
	{
		IChatRepository ChatRepository { get; }

		Task CommitChanges();
	}
}
