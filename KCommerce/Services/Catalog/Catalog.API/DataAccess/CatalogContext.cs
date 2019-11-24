using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Catalog.API.DataAccess
{
	public class CatalogContext : DbContext
	{
		public DbSet<Models.Catalog> Catalogs { get; set; }
		public DbSet<CatalogBrand> CatalogBrands { get; set; }
		public DbSet<CatalogType> CatalogTypes { get; set; }

		private readonly IConfigurationRoot _configRoot;

		public CatalogContext(IConfigurationRoot configRoot)
		{
			_configRoot = configRoot;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_configRoot.GetConnectionString("DefaultConnection"));

			base.OnConfiguring(optionsBuilder);
		}
	}
}
