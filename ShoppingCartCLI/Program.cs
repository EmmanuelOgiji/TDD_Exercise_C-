using System;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart;

namespace ShoppingCartCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IDiscount, DevOpsHandbookDiscount>();
            serviceCollection.AddTransient<IDiscount, UnicornProjectDiscount>();
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var application = serviceProvider.GetService<ConsoleApplication>();
            application.Run(args);
        }
    }
}