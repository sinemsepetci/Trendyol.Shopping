using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trendyol.Shopping.Entity.Concrete;

namespace Trendyol.Shopping
{
    public class Basket : ShoppingCart
    {
        private RabbitMQClient client = new RabbitMQClient();
        private readonly string cartId = Program.Id;
        public override ShoppingCart AddItem(Product product, int quantity)
        {
            var cartItems = client.Pop(cartId);
            if (cartItems == null || cartItems.Count <= 0)
            {
                cartItems.Add(new CartItem() { Id = cartId, Product = product, ProductPrice = quantity * product.Price, Quantity = quantity });
            }
            else
            {
                if (cartItems.Where(x => x.Product.Id == product.Id).Any())
                {
                    CartItem ci = cartItems.Where(x => x.Product.Id == product.Id).FirstOrDefault();
                    ci.Quantity += quantity;
                    ci.ProductPrice += product.Price * quantity;
                }
                else
                {
                    cartItems.Add(new CartItem() { Id = cartId, Product = product, ProductPrice = quantity * product.Price, Quantity = quantity });
                }
            }
            client.Push(cartItems);

            return new Basket() { CartItems = cartItems };
        }

        public override ShoppingCart ApplyCampaign()
        {
            double discount = 0;
            int totalQuantity = 0;
            double maxAmount = 0;
            var cartItems = client.Pop(cartId);
            foreach (var cmp in this.Campaign)
            {
                var products = cartItems.Where(x => x.Product.Category.Id == cmp.Id || x.Product.Category.Id == cmp.Id).ToList();
                foreach (var p in products)
                {
                    totalQuantity += cartItems.Where(x => x.Id == p.Id).Count();
                }
                if (totalQuantity >= cmp.MinProductQuantity)
                {
                    switch (cmp.Type)
                    {
                        case DiscountType.Rate:
                            discount = ((this.TotalPrice) * cmp.DiscountAmount / 100);
                            if (discount > maxAmount)
                                maxAmount = discount;
                            break;
                        case DiscountType.Amount:
                            discount = cmp.DiscountAmount;
                            if (discount > maxAmount)
                                maxAmount = discount;
                            break;
                    }
                }
            }
            this.TotalPrice -= maxAmount;
            this.TotalCampaignAmount += maxAmount;
            this.CartItems = cartItems;
            return this;
        }

        public override double ApplyCoupon(Coupon coupon)
        {
            double discount = 0;
            var basket = client.Pop(cartId);
            if (this.TotalPrice >= coupon.MinCartTotal)
            {
                switch (coupon.Type)
                {
                    case DiscountType.Rate:
                        discount = ((this.TotalPrice) * coupon.DiscountAmount / 100);
                        break;
                    case DiscountType.Amount:
                        discount = coupon.DiscountAmount;
                        break;
                }
                this.TotalCouponAmount += discount;
                this.TotalPrice -= discount;
                this.Coupon = coupon;
                client.Push(basket);
            }

            return discount;
        }

        public override List<Campaign> ApplyDiscounts(params Campaign[] campaigns)
        {
            Basket basket = new Basket();
            var cartItems = client.Pop(cartId);
            //double discount = 0;
            //foreach (var cmp in campaigns)
            //{
            //    if (basket.CartItems.Where(x=>x.Product.Category.Id==cmp.Category.Id).FirstOrDefault().Quantity >= cmp.MinProductQuantity)
            //    {
            //        switch (cmp.Type)
            //        {
            //            case DiscountType.Rate:
            //                discount = ((basket.TotalPrice) * cmp.DiscountAmount / 100);
            //                break;
            //            case DiscountType.Amount:
            //                discount = cmp.DiscountAmount;
            //                break;
            //        }
            //        basket.TotalCouponAmount += discount;
            //        basket.TotalPrice -= discount;
            //        basket.Campaign.Add(cmp);
            //        client.Push(basket);
            //    }
            //}

            basket.Campaign = campaigns.ToList();
            return basket.Campaign;
        }

        public override double GetCampaignDiscount()
        {
            //var basket = client.Pop(cartId);

            return this.TotalCampaignAmount;
        }

        public override double GetCouponDiscount()
        {
            //var basket = client.Pop(cartId);
            return this.TotalCouponAmount;
        }

        public override double GetDeliveryCost(double costPerDelivery, double costPerProduct)
        {
            //var basket = client.Pop();
            DeliveryCostCalculator deliveryCostCalculator = new DeliveryCostCalculator(costPerDelivery, costPerProduct);
            return deliveryCostCalculator.CalculateFor(this);
        }

        public override double GetTotalAmountAfterDiscounts()
        {
            //var basket = client.Pop();
            return this.TotalPrice - this.TotalCouponAmount - this.TotalCampaignAmount;
        }

        public override double GetTotalAmount()
        {
            //var basket = client.Pop();
            return this.TotalPrice;
        }

        public override double GetTotalDiscount()
        {
            throw new NotImplementedException();
        }

        public override ShoppingCart Print()
        {
            this.CartItems = client.Pop(cartId);
            this.CargoPrice = GetDeliveryCost(10, 2);
            this.TotalPrice = GetTotalAmountAfterDiscounts();
            this.TotalCampaignAmount = GetCampaignDiscount();
            return this;
        }

        public override ShoppingCart RemoveItem(Product product, int quantity)
        {
            var cartItems = client.Pop(cartId);
            if (cartItems.Count > 0)
            {
                var removedItem = cartItems.Where(x => x.Id == product.Id).FirstOrDefault();

                for (int i = 0; i < quantity; i++)
                {
                    cartItems.Remove(cartItems.Where(x => x.Id == removedItem.Id).FirstOrDefault());
                }

                this.TotalProductCount = this.TotalProductCount - quantity;
                this.TotalPrice -= (quantity * product.Price);
                client.Push(this.CartItems);
                return this;
            }
            return this;
        }
    }
}
