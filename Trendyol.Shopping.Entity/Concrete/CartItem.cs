using System;
using Trendyol.Shopping.Entity.Abstract;

namespace Trendyol.Shopping.Entity.Concrete
{
    public class CartItem : IEntity
    {
        public string Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double ProductPrice { get; set; }
    }
}