using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trendyol.Shopping.Entity.Concrete;

namespace Trendyol.Shopping
{
    public class DeliveryCostCalculator
    {
        public double costPerDelivery { get; set; }
        public double costPerProduct { get; set; }
        public double fixedCost { get; set; }

        public DeliveryCostCalculator(double costPerDelivery, double costPerProduct)
        {
            this.costPerDelivery = costPerDelivery;
            this.costPerProduct = costPerProduct;
            this.fixedCost = 2.99;
        }
        public double CalculateFor(ShoppingCart cart)
        {
            //var numberOfDeliveries = cart.ProductList.Select(x => x.Category).Distinct().Count();
            var numberOfDeliveries = cart.CartItems.GroupBy(x => x.Product.Category).Count();
            //var numberOfProducts = cart.ProductList.Distinct().Count();
            var numberOfProducts = cart.TotalProductCount;
            return (costPerDelivery * numberOfDeliveries) + (costPerProduct * numberOfProducts) + fixedCost;
        }
    }
}
