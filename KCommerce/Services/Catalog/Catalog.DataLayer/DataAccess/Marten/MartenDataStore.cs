using Marten;
using System;
using System.Threading.Tasks;

namespace Catalog.DataLayer.DataAccess.Marten
{
	public class MartenDataStore : IDataStore
	{
		private readonly IDocumentSession _session;
		public IMartenRepository MartenRepository { get; }

		public MartenDataStore(IDocumentStore documentStore)
		{
			_session = documentStore.LightweightSession();
			MartenRepository = new MartenRepository(_session);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		public async Task CommitChanges()
		{
			await _session.SaveChangesAsync();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_session.Dispose();
			}
		}
	}
}
