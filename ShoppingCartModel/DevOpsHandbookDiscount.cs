using System;
using System.Linq;

namespace ShoppingCartModel
{
    public class DevOpsHandbookDiscount : IDiscount
    {
        public bool DiscountApplies(ShoppingCart cart)
        {
            if (cart == null) throw new ArgumentNullException(nameof(cart));

            if (cart.GetAllItems().Count(item => item.Name == "The DevOps Handbook") >= 5)
            {
                return true;
            }

            return false;
        }

        public double GetDiscountAmount(ShoppingCart cart)
        {
            var count = cart.GetAllItems().Count(item => item.Name == "The DevOps Handbook");
            var numberOfDiscountsToApply = count / 5;

            return numberOfDiscountsToApply * (1.49 * 5);
        }
    }
}