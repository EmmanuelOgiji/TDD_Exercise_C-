using System;
using ShoppingCartModel;
using Xunit;

namespace ShoppingCartModelTests.Unit
{
    public class WhenCreatingAnItem
    {
        [Fact]
        public void TheSkuShouldMatchTheSkuProvided()
        {
            const string expectedSKU = "ABC1234567890123";

            var sut = new Item(expectedSKU, "The DevOps Handbook", 19.99);

            Assert.Equal(expectedSKU, sut.SKU);
        }

        [Fact]
        public void ThePriceShouldMatchThePriceSupplied()
        {
            var sut = new Item("ABC1234567890123", "The DevOps Handbook", 9.99);
            var result = sut.Price;
            
            Assert.Equal(9.99, result);
        }

        [Theory]
        [InlineData("ABC1234567890123", "The DevOps Handbook", 19.99)]
        [InlineData("DEF1234567890123", "The Phoenix Project", 14.99)]
        public void TheNameShouldMatchTheNameSupplied(string sku, string name, double price)
        {
            var sut = new Item(sku, name, price);
            
            var result = sut.Name;
            
            Assert.Equal(name, result);
        }
    }

    public class WhenCreatingAnItemWithNullSKU
    {
        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Item(null, "The DevOps Handbook", 19.99));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentNullException>(() => new Item(null, "The DevOps Handbook", 19.99));

            Assert.Contains("The SKU cannot be null.", message);
        }
    }
    
    public class WhenCreatingAnItemWithBlankSKU {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Item("   ", "The DevOps Handbook", 19.99));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new Item("   ", "The DevOps Handbook", 19.99));

            Assert.Contains("The SKU cannot be blank or empty.", message);
        }
    }
    
    public class WhenCreatingAnItemWithEmptySKU {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Item(string.Empty, "The DevOps Handbook", 19.99));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new Item(string.Empty, "The DevOps Handbook", 19.99));

            Assert.Contains("The SKU cannot be blank or empty.", message);
        }
    }
    
    public class WhenCreatingAnItemWithNullName {
        [Fact]
        public void ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Item("AB1234567890", null, 19.99));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentNullException>(() => new Item("AB1234567890", null, 19.99));

            Assert.Contains("The name cannot be null.", message);
        }
    }
    
    public class WhenCreatingAnItemWithBlankName {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Item("AB1234567890", "   ", 19.99));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new Item("AB1234567890", "   ", 19.99));

            Assert.Contains("The name cannot be blank or empty.", message);
        }
    }
    
    public class WhenCreatingAnItemWithEmptyName {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Item("AB1234567890", string.Empty, 19.99));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new Item("AB1234567890", string.Empty, 19.99));

            Assert.Contains("The name cannot be blank or empty.", message);
        }
    }
}