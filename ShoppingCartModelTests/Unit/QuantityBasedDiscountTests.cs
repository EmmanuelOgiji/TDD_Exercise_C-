using System;
using System.Collections.Generic;
using ShoppingCartModel;
using Xunit;

namespace ShoppingCartModelTests.Unit
{
    public class WhenCreatingQuantityBasedDiscountWithNullSku
    {
        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new QuantityBasedDiscount(null, 1, 0.1));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new QuantityBasedDiscount(null, 1, 0.1));

            Assert.Contains("The SKU cannot be null", message);
        }
    }

    public class WhenCreatingQuantityBasedDiscountWithEmptySku
    {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new QuantityBasedDiscount(string.Empty, 1, 0.1));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new QuantityBasedDiscount(string.Empty, 1, 0.1));

            Assert.Contains("The SKU cannot be empty or blank.", message);
        }
    }

    public class WhenCreatingQuantityBasedDiscountWithBlankSku
    {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new QuantityBasedDiscount("   ", 1, 0.1));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new QuantityBasedDiscount("", 1, 0.1));

            Assert.Contains("The SKU cannot be empty or blank.", message);
        }
    }
    
    public class WhenCreatingQuantityBasedDiscountWithQuantityOfZero
    {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new QuantityBasedDiscount("AB1234", 0, 0.1));
        }
    }

    public class WhenCallingDiscountAppliesWithNullShoppingCart
    {
        private readonly QuantityBasedDiscount _sut;

        public WhenCallingDiscountAppliesWithNullShoppingCart()
        {
            _sut = new QuantityBasedDiscount("AB1234", 1, 0.1);
        }

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.DiscountApplies(null));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => _sut.DiscountApplies(null));

            Assert.Contains("The cart cannot be null", message);
        }
    }

    public class WhenCallingGetDiscountAmountWithNullShoppingCart
    {
        private readonly QuantityBasedDiscount _sut;

        public WhenCallingGetDiscountAmountWithNullShoppingCart()
        {
            _sut = new QuantityBasedDiscount("AB1234", 1, 0.1);
        }

        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.GetDiscountAmount(null));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => _sut.GetDiscountAmount(null));

            Assert.Contains("The cart cannot be null", message);
        }
    }
    
    public class WhenPassingEmptyShoppingCart
    {
        private readonly ShoppingCart _cart;
        private readonly QuantityBasedDiscount _sut;
        
        public WhenPassingEmptyShoppingCart()
        {
            _cart = new ShoppingCart(new List<IDiscount>());
            _sut = new QuantityBasedDiscount("AB1234", 1, 0.1);
        }

        [Fact]
        public void ShouldReturnFalseFromDiscountApplies()
        {
            Assert.False(_sut.DiscountApplies(_cart));
        }
        
        [Fact]
        public void ShouldReturnZeroFromGetDiscountAmount()
        {
            Assert.Equal(0, _sut.GetDiscountAmount(_cart));
        }
    }

    public class WhenPassingShoppingCartToWhichDiscountDoesNotApply
    {
        private readonly ShoppingCart _cart;
        private readonly QuantityBasedDiscount _sut;

        public WhenPassingShoppingCartToWhichDiscountDoesNotApply()
        {
            _cart = new ShoppingCart(new List<IDiscount>());
            _cart.Add(Globals.Items.PhoenixProject);
            _sut = new QuantityBasedDiscount(Globals.Items.DevOpsHandbookSku, 1, 0.1);
        }

        [Fact]
        public void ShouldReturnFalseFromDiscountApplies()
        {
            Assert.False(_sut.DiscountApplies(_cart));
        }
        
        [Fact]
        public void ShouldReturnZeroFromGetDiscountAmount()
        {
            Assert.Equal(0, _sut.GetDiscountAmount(_cart));
        }
    }
    
    public class WhenPassingShoppingCartToWhichDiscountApplies
    {
        private const string itemSku = "AB1234";
        private readonly QuantityBasedDiscount _sut;
        
        public WhenPassingShoppingCartToWhichDiscountApplies()
        {
            _sut = new QuantityBasedDiscount(itemSku, 1, 0.1);
        }

        [Theory]
        [ClassData(typeof(ShoppingCartData))]
        // [InlineData(1, true)]
        // [InlineData(2, true)]
        // [InlineData(5, true)]
        public void ShouldReturnTrueFromDiscountApplies(ShoppingCart cart, double dummy)
        {
            // var cart = new ShoppingCart(new List<IDiscount>());
            // cart.Add(new Item(itemSku, "The DevOps Handbook", 19.99), quantity);
            
            // var result = _sut.DiscountApplies(cart);
            
            // Assert.Equal(expectedResult, result);
            Assert.True(_sut.DiscountApplies(cart));
        }

        [Theory]
        [ClassData(typeof(ShoppingCartData))]
        // [InlineData(1, 1.99)]
        // [InlineData(2, 3.99)]
        // [InlineData(5, 9.99)]
        public void ShouldReturnCorrectDiscountAmountFromGetDiscountAmount(ShoppingCart cart, double expectedAmount)
        {
            var result = _sut.GetDiscountAmount(cart);
            
            Assert.Equal(expectedAmount, result);
        }
    }

    public class ShoppingCartData : TheoryData<ShoppingCart, double>
    {
        public ShoppingCartData()
        {
            var firstCart = new ShoppingCart(new List<IDiscount>());
            firstCart.Add(Globals.Items.DevOpsHandbook, 1);
            Add(firstCart, 1.99);
            
            var secondCart = new ShoppingCart(new List<IDiscount>());
            secondCart.Add(Globals.Items.DevOpsHandbook, 2);
            Add(secondCart, 3.99);
            
            var thirdCart = new ShoppingCart(new List<IDiscount>());
            thirdCart.Add(Globals.Items.DevOpsHandbook, 5);
            Add(thirdCart, 9.99);
        }
    }
    // quantity that the discount applies to
    // quantity in the basket
    // sku
    // discount amount
    
    // Requires 1 copy of DOH for 10% discount
    
    // 1 copy in cart then discount = 1.99
    // 2 copies in cart then discount = 3.98
    // 5 copies in car then discount = 9.94
    
    // Requires 2 copy of DOH for 10% discount
    
    // 1 copy in cart then discount = 1.99
    // 2 copies in cart then discount = 3.99
    // 5 copies in car then discount = 9.99
    // 6 copies in cart then discount = 11.99
    
    public class WhenPassingShoppingCartWhichDoesNotContainEnoughForDiscountToApply
    {
        private readonly ShoppingCart _cart;
        private readonly QuantityBasedDiscount _sut;

        public WhenPassingShoppingCartWhichDoesNotContainEnoughForDiscountToApply()
        {
            _cart = new ShoppingCart(new List<IDiscount>());
            _cart.Add(Globals.Items.DevOpsHandbook, 3);
            _sut = new QuantityBasedDiscount(Globals.Items.DevOpsHandbookSku, 5, 0.1);
        }

        [Fact]
        public void ShouldReturnFalseFromDiscountApplies()
        {
            Assert.False(_sut.DiscountApplies(_cart));
        }

        [Fact]
        public void ShouldReturnZeroFromGetDiscountAmount()
        {
            Assert.Equal(0, _sut.GetDiscountAmount(_cart));
        }
    }

    public class WhenPassingShoppingCartWhichContainsEnoughForDiscountToApply
    {
        private readonly ShoppingCart _cart;
        private readonly QuantityBasedDiscount _sut;

        public WhenPassingShoppingCartWhichContainsEnoughForDiscountToApply()
        {
            _cart = new ShoppingCart(new List<IDiscount>());
            _cart.Add(Globals.Items.DevOpsHandbook, 5);
            _sut = new QuantityBasedDiscount(Globals.Items.DevOpsHandbookSku, 5, 0.1);
        }

        [Fact]
        public void ShouldReturnTrueFromDiscountApplies()
        {
            Assert.True(_sut.DiscountApplies(_cart));
        }

        [Fact]
        public void ShouldReturnCorrectValueFromGetDiscountAmount()
        {
            Assert.Equal(9.99, _sut.GetDiscountAmount(_cart));
        }
    }
    
    public class WhenPassingShoppingCartWhichContainsMoreThanOneItemAndDiscountApplies
    {
        private readonly ShoppingCart _cart;
        private readonly QuantityBasedDiscount _sut;

        public WhenPassingShoppingCartWhichContainsMoreThanOneItemAndDiscountApplies()
        {
            _cart = new ShoppingCart(new List<IDiscount>());
            _cart.Add(Globals.Items.PhoenixProject, 5);
            _cart.Add(Globals.Items.DevOpsHandbook, 5);
            _sut = new QuantityBasedDiscount(Globals.Items.DevOpsHandbookSku, 5, 0.1);
        }

        [Fact]
        public void ShouldReturnTrueFromDiscountApplies()
        {
            Assert.True(_sut.DiscountApplies(_cart));
        }

        [Fact]
        public void ShouldReturnCorrectValueFromGetDiscountAmount()
        {
            Assert.Equal(9.99, _sut.GetDiscountAmount(_cart));
        }
    }
    public static class Globals
    {
        public static class Items
        {
            public const string DevOpsHandbookSku = "AB1234";
            public static Item DevOpsHandbook = new Item(DevOpsHandbookSku, "The DevOps Handbook", 19.99);
            public static Item PhoenixProject = new Item("CD7890", "The Phoenix Project", 14.99);
            public static Item UnicornProject = new Item("GHI1234567890123", "The Unicorn Project", 12.99);
        }
    }
}