using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
	public class OrderController : Controller
	{
		private IOrderRepository repository;
		private Cart cart;

		public OrderController(IOrderRepository repository, Cart cart)
		{
			this.repository = repository;
			this.cart = cart;
		}

		public ViewResult Checkout() => View(new Order());

		[HttpPost]
		public IActionResult Checkout(Order order)
		{
			if(!cart.Lines.Any())
			{
				this.ModelState.AddModelError("", "Sorry, your cart is empty!");
			}

			if(this.ModelState.IsValid)
			{
				order.Lines = cart.Lines.ToArray();
				this.repository.SaveOrder(order);
				this.cart.Clear();
				return RedirectToPage("/Completed", new { orderId = order.OrderID });
			}
			else
			{
				return View();
			}
		}
	}
}
