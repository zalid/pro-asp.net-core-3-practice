using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;

namespace SportsStore.Pages
{
	public class CartModel : PageModel
	{
		private IStoreRepository repository;

		public CartModel(IStoreRepository repository, Cart cart)
		{
			this.repository = repository;
			this.Cart = cart;
		}

		public Cart Cart { get; set; }

		public string ReturnUrl { get; set; }

		public void OnGet(string returnUrl)
			=> this.ReturnUrl = String.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;

		public IActionResult OnPost(long productId, string returnUrl)
		{
			var product = this.repository.Products.FirstOrDefault(p => p.ProductID == productId);
			this.Cart.AddItem(product, 1);
			return RedirectToPage(new { returnUrl = returnUrl });
		}

		public IActionResult OnPostRemove(long productId, string returnUrl)
		{
			var line = this.Cart.Lines.FirstOrDefault(cl => cl.Product.ProductID == productId);
			if(line != null
			&& line.Product != null)
			{
				this.Cart.RemoveLine(line.Product);
			}
			return RedirectToPage(new { returnUrl = returnUrl });
		}
	}
}
