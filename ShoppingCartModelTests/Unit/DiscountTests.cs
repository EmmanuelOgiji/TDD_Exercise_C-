using System;
using System.Collections.Generic;
using Moq;
using ShoppingCartModel;
using Xunit;

namespace ShoppingCartModelTests.Unit.Discounts
{
    public class WhenDiscountAppliesToShoppingCart
    {
        [Fact]
        public void ShouldAskForDiscountAmount()
        {
            var mockDiscount = new Mock<IDiscount>();
            mockDiscount.Setup(d => d.DiscountApplies(It.IsAny<ShoppingCartModel.ShoppingCart>())).Returns(true);
            
            var sut = new ShoppingCartModel.ShoppingCart(new List<IDiscount> { mockDiscount.Object });

            var result = sut.SubTotal;

            mockDiscount.Verify(d => d.GetDiscountAmount(It.IsAny<ShoppingCart>()), Times.Once());
        }
    }

    public class WhenDiscountDoesNotApplyToShoppingCart
    {
        [Fact]
        public void ShouldNotAskToApplyDiscount()
        {
            var mockDiscount = new Mock<IDiscount>();
            mockDiscount.Setup(d => d.DiscountApplies(It.IsAny<ShoppingCart>())).Returns(false);
            
            var sut = new ShoppingCart(new List<IDiscount> { mockDiscount.Object });

            var unused = sut.SubTotal;
            
            mockDiscount.Verify(d => d.GetDiscountAmount(It.IsAny<ShoppingCart>()), Times.Never);
        }
    }
    
    // If you buy 5 of 'The DevOps Handbook' you get them for £18.50 each.
    public class WhenBuyingMultipleItemsWhichAreDiscounted
    {
        [Theory]
        [InlineData("ABC1234567890123", "The DevOps Handbook", 19.99, 5, 92.50)]  // discount
        [InlineData("DEF1234567890123", "The Phoenix Project", 14.99, 5, 74.95)]  // no discount
        public void ShouldApplyDiscountToTheSubtotal(string sku, string title, double unitPrice, int quantity, double expectedSubTotal)
        {
            // Arrange
            var sut = new ShoppingCart(new List<IDiscount> { new DevOpsHandbookDiscount() });
            
            // Act
            sut.Add(new Item(sku, title, unitPrice), quantity);
            
            // Assert
            Assert.Equal(expectedSubTotal, sut.SubTotal);
        }
    }

    public class WhenBuyingFiveOrMoreCopiesOfDevOpsHandbook
    {
        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public void CallingDiscountAppliesReturnsTrue(int quantity)
        {
            // Arrange
            var sut = new DevOpsHandbookDiscount();
            var cart = new ShoppingCart(new List<IDiscount> { sut });
            cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), quantity);
            
            
            // Act
            var result = sut.DiscountApplies(cart);
            
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public void ShouldApplyDiscountToShoppingCart(int quantity)
        {
            var sut = new ShoppingCart(new List<IDiscount> { new DevOpsHandbookDiscount() });
            sut.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), quantity);

            var result = sut.SubTotal;

            Assert.Equal(quantity * 18.5, result);
        } 
    }

    public class WhenBuyingFourOrLessCopiesOfDevOpsHandbook
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(4)]
        public void CallingDiscountAppliesReturnsFalse(int quantity)
        {
            // Arrange
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), quantity);
            
            var sut = new DevOpsHandbookDiscount();
            
            // Act
            var result = sut.DiscountApplies(cart);
            
            // Assert
            Assert.False(result);
        }
    }

    public class WhenCallingDiscountAppliesWithNullCart
    {
        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            // Arrange
            var sut = new DevOpsHandbookDiscount();
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.DiscountApplies(null));
        }

        [Fact]
        public void ShouldReturnAnInformativeErrorMessage()
        {
            
        }
    }
    
    // If you buy a copy of The Phoenix Project you save £2 when you
    // also buy a copy of The Unicorn Project
}