using System;
using System.Collections.Generic;
using System.Text;
using Trendyol.Shopping.Entity.Abstract;

namespace Trendyol.Shopping.Entity.Concrete
{
    public abstract class ShoppingCart : IEntity
    {
        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<Campaign> Campaign { get; set; }
        public Coupon Coupon { get; set; }
        public double TotalCouponAmount { get; set; }
        public double TotalCampaignAmount { get; set; }
        public int TotalProductCount { get; set; }
        public double CargoPrice { get; set; }
        public double TotalPrice { get; set; }

        public abstract ShoppingCart AddItem(Product product, int quantity);
        public abstract ShoppingCart RemoveItem(Product product, int quantity);
        public abstract ShoppingCart ApplyCampaign();
        public abstract double ApplyCoupon(Coupon coupon);
        public abstract List<Campaign> ApplyDiscounts(params Campaign[] campaigns);
        public abstract double GetCampaignDiscount();
        public abstract double GetCouponDiscount();
        public abstract double GetDeliveryCost(double costPerDelivery, double costPerProduct);
        public abstract double GetTotalAmountAfterDiscounts();
        public abstract double GetTotalAmount();
        public abstract ShoppingCart Print();
        public abstract double GetTotalDiscount();
    }
}
