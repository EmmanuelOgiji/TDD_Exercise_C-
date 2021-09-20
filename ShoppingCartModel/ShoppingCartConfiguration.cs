using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingCartModel
{
    public static class ShoppingCartConfiguration
    {
        public static void ConfigureContainer(ServiceCollection serviceCollection)
        {
            RegisterAllTypesAgainstInterface(serviceCollection, typeof(IDiscount));
            // serviceCollection.AddTransient<IDiscount, UnicornProjectDiscount>();
            // serviceCollection.AddTransient<IDiscount, DevOpsHandbookDiscount>();
            serviceCollection.AddTransient<ShoppingCart, ShoppingCart>();
        }
        
        /// <summary>
        /// Registers all types implementing a given interface with the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method assists in making the application comply with the Open-Closed Principle.  It is used to register
        /// all classes implementing the <see cref="IItem"/> and <see cref="IOffer"/> interfaces.  If new items or new
        /// offers are added to the application they will automatically be made available without requiring any changes
        /// to other parts of the application.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection we are configuring.</param>
        /// <param name="interfaceType">The interface we want to register types against.</param>
        private static void RegisterAllTypesAgainstInterface(IServiceCollection serviceCollection, Type interfaceType)
        {
            var allTypes = Assembly.GetAssembly(typeof(ShoppingCart)).GetTypes();
            
            foreach (var type in allTypes) {

                if (type.Name.Equals("QuantityBasedDiscount") || type.Name.Equals("PairBasedDiscount"))
                {
                    // TODO: [MC] Remove this block.  It is a temporary workaround to prevent the QuantityBasedDiscount and PairBasedDiscount being registered in the container.
                    continue;
                }
                
                // Ignore abstract classes.
                if (!type.GetTypeInfo().IsAbstract) {
                    foreach (var i in type.GetInterfaces()) {
                        // If the type implements the interface we are interested in...
                        if (i.Name.Equals(interfaceType.Name)) {
                            /// ...register it in the container.
                            serviceCollection.AddTransient(interfaceType, type);
                        }
                    }
                }
            }
        }
    }
}