using Chat.DataLayer.DatabaseModels;
using Marten;
using Marten.Schema.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.DataLayer.DataAccess.Marten
{
	public static class MartenInstaller
	{
		public static void AddMarten(this IServiceCollection services, string cnnString)
		{
			services.AddSingleton(CreateDocumentStore(cnnString));

			services.AddScoped<IDataStore, MartenDataStore>();
		}

		private static IDocumentStore CreateDocumentStore(string connectionString)
		{
			var store = DocumentStore.For(_ =>
			{
				_.Connection(connectionString);
				_.DefaultIdStrategy = (mapping, storeOptions) => new CombGuidIdGeneration();
			});

			return store;
		}
	}
}
