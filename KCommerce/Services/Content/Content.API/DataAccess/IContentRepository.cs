using Content.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Content.API.DataAccess
{
	public interface IContentRepository
	{
		Task<IEnumerable<Post>> GetAllPosts();

		Task<Post> GetPost(string id);

		Task AddPost(Post item);

		Task<bool> RemovePost(string id);
		
		Task<bool> RemoveAllNotes();
	}
}
