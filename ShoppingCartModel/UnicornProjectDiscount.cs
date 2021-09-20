using System;

namespace ShoppingCartModel
{
    public class UnicornProjectDiscount : IDiscount
    {
        public bool DiscountApplies(ShoppingCart cart)
        {
            // the discount applies if there are one or more pairs in the cart
            return GetNumberOfPairs(cart) > 0;
        }

        public double GetDiscountAmount(ShoppingCart cart)
        {
            var numberOfPairs = GetNumberOfPairs(cart);
            return numberOfPairs * 2;
        }

        private int GetNumberOfPairs(ShoppingCart cart)
        {
            var phoenixProjectCount = 0;
            var unicornProjectCount = 0;
            
            foreach (var item in cart.GetAllItems())
            {
                if (item.Name.Equals("The Phoenix Project"))
                {
                    phoenixProjectCount++;
                }
                
                if (item.Name.Equals("The Unicorn Project"))
                {
                    unicornProjectCount++;
                }
            }

            var numberOfPairs = Math.Min(phoenixProjectCount, unicornProjectCount);
            return numberOfPairs;
        }
    }
}