using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Shopping.Entity.Abstract;

namespace Trendyol.Shopping.Entity.Concrete
{
    public class Campaign : IEntity, IDiscount
    {
        public string Id { get; set; }
        public double DiscountAmount { get; set; }
        public DiscountType Type { get; set; }
        public int MinProductQuantity { get; set; }
        public Category Category { get; set; }
    }
}
