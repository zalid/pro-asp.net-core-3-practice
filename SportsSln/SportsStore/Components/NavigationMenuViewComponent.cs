using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SportsStore.Models;

namespace SportsStore.Components
{

	public class NavigationMenuViewComponent : ViewComponent
	{
		private IStoreRepository repository;

		public NavigationMenuViewComponent(IStoreRepository repository)
			=> this.repository = repository;

		public IViewComponentResult Invoke()
		{
			this.ViewBag.SelectedCategory = RouteData?.Values["category"];
			var products = this.repository.Products
										  .Select(x => x.Category)
										  .Distinct()
										  .OrderBy(x => x);
			return View(products);
		}
	}
}
