using Catalog.DataLayer.Commands;
using Shared.DataLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.DataLayer.DataAccess
{
	public interface ICatalogRepository : IRepository<DatabaseModels.Catalog>
	{
		Task<List<DatabaseModels.Catalog>> FindAllCatalogs();

		Task<DatabaseModels.Catalog> FindById(Guid id);

		DatabaseModels.Catalog Add(DatabaseModels.Catalog catalog);

		void ValidateAddCatalogCommand(AddCatalogCommand command);
	}
}
