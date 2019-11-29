using System;
using System.Threading.Tasks;

namespace Catalog.DataLayer.DataAccess.Marten
{
	public interface IDataStore : IDisposable
	{
		IMartenRepository MartenRepository { get; }

		Task CommitChanges();
	}
}
