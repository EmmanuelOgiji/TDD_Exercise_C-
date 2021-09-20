using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartModel;
using ShoppingCartModelTests.Demo;
using Xunit;
using Xunit.Abstractions;

namespace ShoppingCartModelTests.Integration
{
    public class WhenAskingTheShoppingCartToConfigureItself
    {
        private readonly ShoppingCart _result;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITestOutputHelper _testOutputHelper;
        
        public WhenAskingTheShoppingCartToConfigureItself(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            
            var serviceCollection = new ServiceCollection();
            
            // Act
            ShoppingCartConfiguration.ConfigureContainer(serviceCollection);
            
            /*
             * The following code demonstrates how to configure the IoC container
             * to enable the use of the Decorator pattern.
             */
            // serviceCollection.AddTransient<ShoppingCart, ShoppingCart>();
            // serviceCollection.AddTransient<DevOpsHandbookDiscount, DevOpsHandbookDiscount>();
            // serviceCollection.AddTransient<IDiscount>(
                // c => ActivatorUtilities.CreateInstance<LoggingDiscount>(c,
                    // ActivatorUtilities.CreateInstance<DevOpsHandbookDiscount>(c),
                    // testOutputHelper));
            
            
            _serviceProvider = serviceCollection.BuildServiceProvider();

            _result = _serviceProvider.GetService<ShoppingCart>();
            _result.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), 5);
        }

        [Fact]
        public void ShouldLog()
        {
            var subTotal = _result.SubTotal;
            _testOutputHelper.WriteLine("You should see log messages from the LoggingDiscount class above.");
        }
        
        [Fact]
        public void ShouldBeAbleToResolveShoppingCart()
        {
            Assert.IsType<ShoppingCart>(_result);
        }

        [Fact]
        public void ShouldBeAbleToResolveDiscounts()
        {
            var discounts = _serviceProvider.GetServices<IDiscount>();
            //TODO: split into separate test classes

            Assert.Equal(2, discounts.Count());
            Assert.Contains(discounts, d => d.GetType() == typeof(DevOpsHandbookDiscount));
            Assert.Contains(discounts, d => d.GetType() == typeof(UnicornProjectDiscount));
        }
    }
    
    public class WhenBuyingFiveOrMoreCopiesOfDevOpsHandbook
    {
        private readonly ShoppingCart _sut;
        
        public WhenBuyingFiveOrMoreCopiesOfDevOpsHandbook()
        {
            var serviceCollection = new ServiceCollection();
            ShoppingCartConfiguration.ConfigureContainer(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _sut = serviceProvider.GetService<ShoppingCart>();
            _sut.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), 5);
            _sut.Add(new Item("DEF1234567890123", "The Phoenix Project", 14.99));
            _sut.Add(new Item("GHI1234567890123", "The Unicorn Project", 12.99));
        }

        [Fact]
        public void ShouldApplyBothDiscounts()
        {
            Assert.Equal(118.48, _sut.SubTotal);
        }
    }
}