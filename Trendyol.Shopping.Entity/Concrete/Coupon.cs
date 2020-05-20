using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Shopping.Entity.Abstract;

namespace Trendyol.Shopping.Entity.Concrete
{
    public class Coupon : IEntity, IDiscount
    {
        public string Id { get; set; }
        public bool IsAvailable { get; set; }
        public double MinCartTotal { get; set; }
        public double DiscountAmount { get; set; }
        public DiscountType Type { get; set; }
    }
}
