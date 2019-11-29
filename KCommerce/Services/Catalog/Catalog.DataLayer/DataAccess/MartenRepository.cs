using Catalog.DataLayer.Events;
using Marten;
using System;
using System.Collections.Generic;

namespace Catalog.DataLayer.DataAccess
{
	public class MartenRepository : IMartenRepository
	{
		private readonly IDocumentSession _documentSession;
		private readonly Dictionary<Guid, Guid> _userOrder; // <userId, streamId>
		public MartenRepository(IDocumentSession documentSession)
		{
			_documentSession = documentSession;
			_userOrder = new Dictionary<Guid, Guid>();
		}

		public void SetUserStream(Guid userId, Guid streamId)
		{
			if (_userOrder.ContainsKey(userId))
			{
				throw new Exception("User is only allow to have one processing order");
			}

			_userOrder[userId] = streamId;
		}

		public void AddCatalog(Guid userId, DatabaseModels.Catalog catalog, int quantity)
		{
			CheckOrderCondition(userId);
			// TODO: all logic must be checked

			var started = new CatalogAdded
			{
				CatalogId = catalog.Id,
				Quantity = quantity,
				UserId = userId
			};

			_documentSession.Events.StartStream(_userOrder[userId], started);
		}

		public void RemoveCatalog(Guid userId, DatabaseModels.Catalog catalog, int quantity)
		{
			CheckOrderCondition(userId);

			// TODO: all logic must be checked
			var removed = new CatalogRemoved()
			{
				CatalogId = catalog.Id,
				Quantity = quantity,
				UserId = userId
			};

			_documentSession.Events.StartStream(_userOrder[userId], removed);
		}

		public void CreateOrder(Guid userId, Guid orderId, List<DatabaseModels.Catalog> catalogs)
		{
			throw new NotImplementedException();
		}

		public void ChangeOrderStatus(Guid orderId, OrderEventStatus status)
		{
			// TODO: all logic must be checked
		}

		private void CheckOrderCondition(Guid userId)
		{
			if (!_userOrder.ContainsKey(userId))
			{
				throw new Exception("Need to set stream Id");
			}
		}
	}
}
