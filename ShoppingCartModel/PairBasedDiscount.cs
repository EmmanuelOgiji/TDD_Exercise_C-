using System;

namespace ShoppingCartModel
{
    public class PairBasedDiscount : IDiscount, IEntity
    {
        private readonly string _sku1;
        private readonly string _sku2;
        private readonly double _discountAmount;
        private readonly string _date;
        
        public long DbId { get; protected set; }

        public PairBasedDiscount(String sku1, String sku2, double discountAmount, string date=null) : this(sku1,sku2,discountAmount)
        {
            _date = date;
        }

        public PairBasedDiscount(String sku1, String sku2, double discountAmount)
        {
            if (sku1 == null || sku2 == null)
            {
                throw new ArgumentNullException(nameof(sku1), "The SKU cannot be null");
            }

            if (string.IsNullOrEmpty(sku1.Trim()) || string.IsNullOrEmpty(sku2.Trim()))
            {
                throw new ArgumentException("The SKU cannot be empty or blank.");
            }
            
            if (discountAmount <= 0)
            {
                throw new ArgumentException("The discount amount must be greater than zero.");
            }

            _sku1 = sku1;
            _sku2 = sku2;
            _discountAmount = discountAmount;
        }

        public bool DiscountApplies(ShoppingCart cart)
        {
            if (this._date != null)
            {
                return false;
            }
            
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart), "The cart cannot be null");
            }
            
            var sku1Count = 0;
            var sku2Count = 0;

            foreach (var t in cart.GetAllItems())
            {
                if (t.SKU == _sku1)
                {
                    sku1Count++;
                }
                if (t.SKU == _sku2)
                {
                    sku2Count++;
                }
            }


            if (sku1Count > 0 && sku2Count > 0)
            {
                return true;
            }
            return false;
        }

        //TODO: Next sessions: Work towards database integration: Discount Id + Discount start/end dates
        public double GetDiscountAmount(ShoppingCart cart)
        {
            if (DiscountApplies(cart))
            {
                return _discountAmount;
            }
            return 0;
        }
    }
}