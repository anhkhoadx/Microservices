using Content.API.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Content.API.DataAccess
{
	public class ContentRepository : IContentRepository
	{
		private readonly ContentContext _context;

		public ContentRepository(IConfigurationRoot configRoot)
		{
			_context = new ContentContext(configRoot);
		}

		public async Task<IEnumerable<Post>> GetAllPosts()
		{
			return await _context.Posts.Find(_ => true).ToListAsync();
		}

		public async Task<Post> GetPost(string id)
		{
			var internalId = GetInternalId(id);
			
			return await _context.Posts.Find(post => post.Id == id || post.InternalId == internalId).FirstOrDefaultAsync();
		}

		public async Task AddPost(Post item)
		{
			await _context.Posts.InsertOneAsync(item);
		}

		public async Task<bool> RemovePost(string id)
		{
			DeleteResult actionResult = await _context.Posts.DeleteOneAsync(Builders<Post>.Filter.Eq("Id", id));

			return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
		}

		public async Task<bool> RemoveAllNotes()
		{
			DeleteResult actionResult = await _context.Posts.DeleteManyAsync(new BsonDocument());

			return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
		}

		private ObjectId GetInternalId(string id)
		{
			if (!ObjectId.TryParse(id, out var internalId))
			{
				internalId = ObjectId.Empty;
			}

			return internalId;
		}
	}
}
