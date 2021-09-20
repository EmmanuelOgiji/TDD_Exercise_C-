using System;

namespace ShoppingCartModel
{
    /// <summary>
    /// Represents an item that can be added to the shopping cart.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The name of the item, e.g. "The DevOps Handbook"
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The unit price of the item, e.g. 19.99
        /// </summary>
        /// <remarks>
        /// Note that the application does not yet consider currency.
        /// </remarks>
        public double Price { get; }

        /// <summary>
        /// The SKU/barcode of the item
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string SKU { get; }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="name">The name of the new item.</param>
        /// <param name="price">The price of the new item.</param>
        /// <param name="sku">The SKU for the item.</param>
        public Item(string sku, string name, double price)
        {
            if(sku == null)
            {
                throw new ArgumentNullException(nameof(sku), "The SKU cannot be null.");
            }

            if (string.IsNullOrEmpty(sku.Trim()))
            {
                throw new ArgumentException("The SKU cannot be blank or empty.");
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "The name cannot be null.");
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                throw new ArgumentException("The name cannot be blank or empty.");
            }
            Name = name;
            Price = price;
            SKU = sku;
        }
    }
}