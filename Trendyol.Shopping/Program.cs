using System;
using System.Collections.Generic;
using Trendyol.Shopping.Entity.Concrete;

namespace Trendyol.Shopping
{
    class Program
    {
        public static readonly string Id = Guid.NewGuid().ToString();
        static void Main(string[] args)
        {
            
            Basket basket = new Basket() { Id = Guid.NewGuid().ToString() };
            List<CartItem> cartItems = new List<CartItem>();
            RabbitMQClient client = new RabbitMQClient();
            Category category = new Category()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Teknoloji"
            };
            Product product = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Category = category,
                Name = "Laptop",
                Price = 5000,
                Stock = 50,
            };
            CartItem cartItem = new CartItem()
            {
                Id = Id,
                Product = product,
            };
            cartItems.Add(cartItem);
            basket = (Basket)basket.AddItem(product, 3);

            Campaign cmp = new Campaign() { Category = category, Id = Guid.NewGuid().ToString(), DiscountAmount = 20.0, MinProductQuantity = 3, Type = DiscountType.Rate };

            var campaignDiscount = basket.ApplyDiscounts(cmp);
            basket.Campaign = campaignDiscount;
            basket = (Basket)basket.ApplyCampaign();
            campaignDiscount.ToString();
            basket.Print();
        }
    }
}
