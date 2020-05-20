using System;
using System.Collections.Generic;
using System.Text;

namespace Trendyol.Shopping.Entity.Concrete
{
    public interface IDiscount
    {
        public double DiscountAmount { get; set; }
        public DiscountType Type { get; set; }
    }
}
