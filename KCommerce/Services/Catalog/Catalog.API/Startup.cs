using Catalog.DataLayer.DataAccess;
using Catalog.DataLayer.DataAccess.Marten;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.DataLayer.Attributes;

namespace Catalog.API
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton((IConfigurationRoot)Configuration);
			services.AddControllers();

			var connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddMarten(connection);
			services.AddDbContext<CatalogContext>(options =>
				options.UseNpgsql(connection).EnableSensitiveDataLogging());
			services.AddSingleton(provider => Configuration);
			services.AddScoped<ICatalogRepository, CatalogRepository>();
			services.AddMediatR(typeof(Startup));
			services.AddMvc(options => options.Filters.Add(typeof(ValidationFilterAttribute)));
		}
		

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
