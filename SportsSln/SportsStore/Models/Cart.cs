using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
	public class Cart
	{
		public List<CartLine> Lines { get; set; } = new List<CartLine>();

		public virtual void AddItem(Product product, int quantity)
		{
			var line = Lines.FirstOrDefault(p => p.Product.ProductID == product.ProductID);

			if(line == null)
			{
				Lines.Add(new CartLine
				{
					Product = product,
					Quantity = quantity
				});
			}
			else
			{
				line.Quantity += quantity;
			}
		}

		public virtual void RemoveLine(Product product)
			=> this.Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);

		public decimal ComputeTotalValue()
			=> this.Lines.Sum(e => e.Product.Price * e.Quantity);

		public virtual void Clear()
			=> this.Lines.Clear();
	}
}
