using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
	public class Startup
	{

		public Startup(IConfiguration config)
			=> this.Configuration = config;

		private IConfiguration Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddDistributedMemoryCache();
			services.AddSession();
			services.AddDbContext<StoreDbContext>(opts =>
			{
				opts.UseSqlServer(
					Configuration["ConnectionStrings:SportsStoreConnection"]);
			});
			services.AddScoped<IStoreRepository, EFStoreRepository>();
			services.AddScoped<IOrderRepository, EFOrderRepository>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped(sp => SessionCart.GetCart(sp));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseStatusCodePages();
			app.UseStaticFiles();
			app.UseSession();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					"catpage",
					"{category}/Page{productPage:int}",
					new { Controller = "Home", action = "Index" });

				endpoints.MapControllerRoute(
					"page",
					"Page{productPage:int}",
					new { Controller = "Home", action = "Index", productPage = 1 });

				endpoints.MapControllerRoute(
					"category",
					"{category}",
					new { Controller = "Home", action = "Index", productPage = 1 });

				endpoints.MapControllerRoute(
					"pagination",
					"Products/Page{productPage}",
					new { Controller = "Home", action = "Index", productPage = 1 });
				endpoints.MapDefaultControllerRoute();
				endpoints.MapRazorPages();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
			});

			SeedData.EnsurePopulated(app);
		}
	}
}
