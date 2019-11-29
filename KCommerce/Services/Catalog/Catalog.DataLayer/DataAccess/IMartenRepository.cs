using System;
using System.Collections.Generic;

namespace Catalog.DataLayer.DataAccess
{
	public interface IMartenRepository
	{
		/// <summary>
		/// User is only allowed to have one processing order at the same time (can contain many catalogs)
		/// StreamId is considered as orderId
		/// After this order is completed, user can create another order
		/// </summary>
		void SetUserStream(Guid userId, Guid streamId);

		void AddCatalog(Guid userId, DatabaseModels.Catalog catalog, int quantity);

		void RemoveCatalog(Guid userId, DatabaseModels.Catalog catalog, int quantity);

		void CreateOrder(Guid userId, Guid orderId, List<DatabaseModels.Catalog> catalogs);

		void ChangeOrderStatus(Guid orderId, OrderEventStatus status);
	}
}
