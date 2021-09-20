using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartModel
{
    /// <summary>
    /// Represents a shopping cart containing a list of goods
    /// to be purchased.
    /// </summary>
    public class ShoppingCart
    {
        private readonly IEnumerable<IDiscount> _discounts;
        private readonly IList<Item> _items;

        // TODO: [MC/EPO] Improve the API of the ShoppingCart by exposing the quantity of a given item. 
        
        public ShoppingCart(IEnumerable<IDiscount> discounts)
        {
            // TODO: [MC] Test for the constructor arguments being null.
            _items = new List<Item>();
            
            _discounts = discounts;
        }
        
        /* What not to do 1:
        public ShoppingCartModel()
        {
            _discounts = new List<IDiscount>();
            _discounts.Add(new DevOpsHandbookDiscount());
            _discounts.Add(new UnicornProjectDiscount());
        }

        // What not to do 2:  Service Locator Anti-pattern
        public ShoppingCartModel(IServiceProvider provider)
        {
            _discounts = new List<IDiscount>();
            _discounts.Add(provider.GetService(typeof(IDiscount)));
        }
        */
        
        /// <summary>
        /// The subtotal of all the goods currently in the cart,
        /// minus any discounts.
        /// </summary>
        public double SubTotal
        {
            get
            {
                var totalPrice = Math.Round(_items.Sum(item => item.Price), 2);
                double totalDiscountAmount = 0.0;
                
                foreach (var discount in _discounts)
                {
                    if (discount.DiscountApplies(this))
                    {
                        totalDiscountAmount += discount.GetDiscountAmount(this);
                    }
                }

                return totalPrice - totalDiscountAmount;
            }
        }

        public IList<Item> GetAllItems()
        {
            return new List<Item> (_items);
        }
        
        /// <summary>
        /// Add one of the given item to the shopping cart.
        /// </summary>
        /// <param name="newItem">The item to add to the cart.</param>
        /// <exception cref="ArgumentNullException">Thrown if the item is null.</exception>
        public void Add(Item newItem)
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem), Errors.NoNullItemAllowed);
            }
            
            _items.Add(newItem);
        }

        /// <summary>
        /// Add a quantity of the given item to the shopping cart.
        /// </summary>
        /// <param name="newItem">The item to add to the cart.</param>
        /// <param name="qty">The quantity of the item to add to the cart.</param>
        /// <exception cref="ArgumentNullException">Thrown if the item is null</exception>
        public void Add(Item newItem, int qty)
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem), Errors.NoNullItemAllowed);
            }
            
            for (var i = 0; i < qty; i++)
            {
                _items.Add(newItem);
            }
        }
    }
}