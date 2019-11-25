using Content.API.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Content.API.DataAccess
{
	public class ContentContext
	{
		private readonly IMongoDatabase _database;

		public ContentContext(IConfigurationRoot configRoot)
		{
			var client = new MongoClient(configRoot.GetConnectionString("MongoConnection:ConnectionString"));
			_database = client.GetDatabase(configRoot.GetConnectionString("MongoConnection:Database"));
		}

		public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Post");
	}
}
