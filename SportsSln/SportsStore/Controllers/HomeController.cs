using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
	public class HomeController : Controller
	{
		private IStoreRepository repository;
		public int PageSize = 4;

		public HomeController(IStoreRepository repository)
			=> this.repository = repository;

		public ViewResult Index(string category, int productPage = 1)
		{
			var products = this.repository.Products
										  .Where(p => String.IsNullOrWhiteSpace(category)
												   || p.Category == category);
			var productsListVM = new ProductsListViewModel
			{
				Products = products.OrderBy(p => p.ProductID)
								   .Skip((productPage - 1) * PageSize)
								   .Take(PageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = productPage,
					ItemsPerPage = PageSize,
					TotalItems = products.Count()
				},
				CurrentCategory = category
			};
			return View(productsListVM);
		}
	}
}
