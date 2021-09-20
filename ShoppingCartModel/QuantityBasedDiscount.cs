using System;
using System.Linq;

namespace ShoppingCartModel
{
    public class QuantityBasedDiscount : IDiscount, IEntity
    {
        private readonly string _sku;
        private readonly int _quantity;
        private readonly double _discountPercent;
        
        public long DbId { get; protected set; }

        public QuantityBasedDiscount(string sku, int quantity, double discountPercent)
        {
            if (sku == null)
            {
                throw new ArgumentNullException(nameof(sku), "The SKU cannot be null");
            }

            if (string.IsNullOrEmpty(sku.Trim()))
            {
                throw new ArgumentException("The SKU cannot be empty or blank.");
            }
            
            if (quantity == 0)
            {
                throw new ArgumentException("The quantity cannot be 0.");
            }

            _sku = sku;
            _quantity = quantity;
            _discountPercent = discountPercent;
        }

        public bool DiscountApplies(ShoppingCart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart), "The cart cannot be null");
            }

            var count = 0;
            
            // If the cart contains an item with the SKU return true
            foreach (var item in cart.GetAllItems())
            {
                if (item.SKU == _sku)
                {
                    count++;
                }

                if (count >= _quantity)
                {
                    return true;
                }
            }
            
            return false;
        }

        // TODO: [MC] Introduce a MonetaryAmount class and use that as the return type.
        public double GetDiscountAmount(ShoppingCart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart), "The cart cannot be null");
            }

            // TODO: [MC] Implement a cart.IsEmpty method
            if (cart.GetAllItems().Count == 0)
            {
                return 0;
            }

            if (!DiscountApplies(cart))
            {
                return 0;
            }
            
            // ASSUMPTION: The cart has at least one item with the relevant SKU.
            var itemsThatApply = cart.GetAllItems().Where(i => i.SKU.Equals(_sku));
            
            // Class invariant
            // When DiscountApplies is true, there is a least one item in the cart which has the SKU of the item to which the discount applies.
            if (DiscountApplies(cart) && !itemsThatApply.Any())
            {
                throw new ApplicationException("Class invariant broken.");
            }
            
            var item = itemsThatApply.First();
            return new Money(_discountPercent * item.Price * itemsThatApply.Count()).Value;
        }
    }
}