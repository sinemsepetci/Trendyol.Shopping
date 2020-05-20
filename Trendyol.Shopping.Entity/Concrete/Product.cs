using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Shopping.Entity.Abstract;

namespace Trendyol.Shopping.Entity.Concrete
{
    public class Product : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
        public bool isDeleted { get; set; }
    }
}
