using System.Collections.Generic;
using ShoppingCartModel;
using Xunit;

namespace ShoppingCartModelTests.Unit
{
    public class WhenBuyingAnItemThatGrantsADiscountOnAnotherItem
    {
        [Fact]
        public void CallingDiscountAppliesReturnsTrue()
        {
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(new Item("DEF1234567890123", "The Phoenix Project", 14.99));
            cart.Add(new Item("GHI1234567890123", "The Unicorn Project", 12.99));
            
            var sut = new UnicornProjectDiscount();
            
            // Act
            var result = sut.DiscountApplies(cart);

            Assert.True(result);
        }
        
        [Fact]
        public void ShouldCalculateDiscountCorrectly()
        {
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(new Item("DEF1234567890123", "The Phoenix Project", 14.99));
            cart.Add(new Item("GHI1234567890123", "The Unicorn Project", 12.99));
            
            var sut = new UnicornProjectDiscount();
            
            // Act
            var result = sut.GetDiscountAmount(cart);
            
            Assert.Equal(2, result);
        }
    }

    public class WhenBuyingMultipleItemsThatGrantsADiscountOtherItems
    {
        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        [InlineData(2, 1, 2)]
        [InlineData(2, 3, 4)]
        public void ShouldIndicateWhetherDiscountAppliesCorrectly(int phoenixProjectQuantity,
            int unicornProjectQuantity, int expectedDiscount)
        {
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(new Item("DEF1234567890123", "The Phoenix Project", 14.99), phoenixProjectQuantity);
            cart.Add(new Item("GHI1234567890123", "The Unicorn Project", 12.99), unicornProjectQuantity);
            
            var sut = new UnicornProjectDiscount();

            var result = sut.DiscountApplies(cart);

            Assert.True(result);
        }
        
        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        [InlineData(2, 1, 2)]
        [InlineData(2, 3, 4)]
        public void ShouldCalculateDiscountCorrectly(int phoenixProjectQuantity, int unicornProjectQuantity, int expectedDiscount)
        {
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(new Item("DEF1234567890123", "The Phoenix Project", 14.99), phoenixProjectQuantity);
            cart.Add(new Item("GHI1234567890123", "The Unicorn Project", 12.99), unicornProjectQuantity);
            
            var sut = new UnicornProjectDiscount();

            var result = sut.GetDiscountAmount(cart);
            
            Assert.Equal(expectedDiscount, result);
        }
    }

    public class WhenBuyingMultipleItemsThatDoesNotGrantADiscountOnOtherItems
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(2, 0, 0)]
        [InlineData(0, 2, 0)]
        [InlineData(0, 0, 2)]
        public void ShouldIndicateWhetherDiscountAppliesCorrectly(int phoenixProjectQuantity,
            int unicornProjectQuantity, int devOpsHandbookQuantity)
        {
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(new Item("DEF1234567890123", "The Phoenix Project", 14.99), phoenixProjectQuantity);
            cart.Add(new Item("GHI1234567890123", "The Unicorn Project", 12.99), unicornProjectQuantity);
            cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), devOpsHandbookQuantity);
            
            var sut = new UnicornProjectDiscount();

            var result = sut.DiscountApplies(cart);

            Assert.False(result);
        }
        
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0)]
        [InlineData(0, 1, 0, 0)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(2, 0, 0, 0)]
        [InlineData(0, 2, 0, 0)]
        [InlineData(0, 0, 2, 0)]
        public void ShouldCalculateDiscountCorrectly(int phoenixProjectQuantity, int unicornProjectQuantity,
            int devOpsHandbookQuantity, int expectedDiscount)
        {
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(new Item("DEF1234567890123", "The Phoenix Project", 14.99), phoenixProjectQuantity);
            cart.Add(new Item("GHI1234567890123", "The Unicorn Project", 12.99), unicornProjectQuantity);
            cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), devOpsHandbookQuantity);

            var sut = new UnicornProjectDiscount();

            var result = sut.GetDiscountAmount(cart);
            
            Assert.Equal(expectedDiscount, result);
        }
    }
}