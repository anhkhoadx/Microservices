using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Content.API.Models
{
	public class Post
	{
		[BsonId]
		public ObjectId InternalId { get; set; }

		// external Id, easier to reference
		public string Id { get; set; }

		public int CatalogId { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Content { get; set; }

		public string Status { get; set; }

		public PostImage HeaderImage { get; set; }

		[BsonDateTimeOptions]
		public DateTime PostedDate { get; set; }
	}
}
