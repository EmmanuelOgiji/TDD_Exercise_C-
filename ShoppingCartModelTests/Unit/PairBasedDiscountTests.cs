using System;
using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using ShoppingCartModel;
using Xunit;
using Xunit.Abstractions;

namespace ShoppingCartModelTests.Unit
{
    public class WhenCreatingPairBasedDiscountWithNullSku
    {
        [Fact]
        public void ShouldThrowArgumentNullExceptionWithOneNullSKU()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PairBasedDiscount(null, Globals.Items.UnicornProject.SKU, 2));
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWithTwoNullSKU()
        {
            Assert.Throws<ArgumentNullException>(() => new PairBasedDiscount(null, null, 2));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => new PairBasedDiscount(null, null, 2));

            Assert.Contains("The SKU cannot be null", message);
        }
    }

    public class WhenCreatingPairBasedDiscountWithEmptySku
    {
        [Fact]
        public void ShouldThrowArgumentExceptionWithOneEmptySKU()
        {
            Assert.Throws<ArgumentException>(() =>
                new PairBasedDiscount(string.Empty, Globals.Items.UnicornProject.SKU, 2));
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWithTwoEmptySKU()
        {
            Assert.Throws<ArgumentException>(() => new PairBasedDiscount(string.Empty, string.Empty, 2));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message =
                TestUtils.GetExceptionMessage<ArgumentException>(() =>
                    new PairBasedDiscount(string.Empty, string.Empty, 2));

            Assert.Contains("The SKU cannot be empty or blank.", message);
        }
    }

    public class WhenCreatingPairBasedDiscountWithBlankSku
    {
        [Fact]
        public void ShouldThrowArgumentExceptionWithOneBlankSKU()
        {
            Assert.Throws<ArgumentException>(() => new PairBasedDiscount("   ", Globals.Items.PhoenixProject.SKU, 2));
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWithTwoBlankSKU()
        {
            Assert.Throws<ArgumentException>(() => new PairBasedDiscount("   ", " ", 2));
        }

        [Fact]
        public void ShouldReturnMeaningfulErrorMessage()
        {
            var message =
                TestUtils.GetExceptionMessage<ArgumentException>(() => new PairBasedDiscount("   ", "   ", 2));

            Assert.Contains("The SKU cannot be empty or blank.", message);
        }
    }

    public class WhenCreatingPairBasedDiscountWithDiscountAmountZero
    {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new PairBasedDiscount(Globals.Items.PhoenixProject.SKU, Globals.Items.UnicornProject.SKU, 0));
        }
    }

    public class WhenCreatingPairBasedDiscountWithNegativeDiscountAmount
    {
        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new PairBasedDiscount(Globals.Items.PhoenixProject.SKU, Globals.Items.UnicornProject.SKU, -1));
        }
    }

    public class WhenCallingPairBasedDiscountWithNullShoppingCart
    {
        private readonly PairBasedDiscount _sut;

        public WhenCallingPairBasedDiscountWithNullShoppingCart()
        {
            _sut = new PairBasedDiscount(Globals.Items.PhoenixProject.SKU, Globals.Items.UnicornProject.SKU, 2);
        }

        [Fact]
        public void DiscountAppliesShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.DiscountApplies(null));
        }

        [Fact]
        public void DiscountAppliesShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => _sut.DiscountApplies(null));

            Assert.Contains("The cart cannot be null", message);
        }

        [Fact]
        public void GetDiscountAmountShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.GetDiscountAmount(null));
        }

        [Fact]
        public void GetDiscountAmountShouldReturnMeaningfulErrorMessage()
        {
            var message = TestUtils.GetExceptionMessage<ArgumentException>(() => _sut.GetDiscountAmount(null));

            Assert.Contains("The cart cannot be null", message);
        }
    }

    public class WhenCallingPairBasedDiscountWithEmptyShoppingCart
    {
        private readonly ShoppingCart _cart;
        private readonly PairBasedDiscount _sut;

        public WhenCallingPairBasedDiscountWithEmptyShoppingCart()
        {
            _cart = new ShoppingCart(new List<IDiscount>());
            _sut = _sut = new PairBasedDiscount(Globals.Items.PhoenixProject.SKU, Globals.Items.UnicornProject.SKU, 2);
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

    public class PairBasedDiscountTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public PairBasedDiscountTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        [Theory]
        [ClassData(typeof(PairBasedDiscountTestData))]
        public void DiscountAppliesReturnsExpectedValue(ShoppingCart cart, String sku1, String sku2, bool expectedDiscountApplies, double expectedDiscountAmount, string testScenario)
        {
            _outputHelper.WriteLine("The test scenario running is:" + testScenario);
            var sut = new PairBasedDiscount(sku1, sku2, 2);

            var result = sut.DiscountApplies(cart);

            Assert.Equal(expectedDiscountApplies, result);
        }

        [Theory]
        [ClassData(typeof(PairBasedDiscountTestData))]
        public void GetDiscountAmountReturnsCorrectValue(ShoppingCart cart, String sku1, String sku2, bool expectedDiscountApplies, double expectedDiscountAmount, string testScenario)
        {
            _outputHelper.WriteLine("The test scenario running is:" + testScenario);
            var sut = new PairBasedDiscount(sku1, sku2, 2);

            var result = sut.GetDiscountAmount(cart);

            Assert.Equal(expectedDiscountAmount, result);
        }
    }

    public class PairBasedDiscountTestData : TheoryData<ShoppingCart, String, String, bool, double, string>
    {
        public PairBasedDiscountTestData()
        {
            PhoenixProjectOnly();
            EmptyCart();
            UnicornProjectOnly();
            DevOpsHandBookOnly();
            UnicornProjectAndDevOpsHandbook();
            UnicornProjectAndPhoenixProject();
            UnicornProjectAndPhoenixProject2();
            UnicornProjectAndPhoenixProject2Copies();
            AllThreeBooks();
        }

        private void PhoenixProjectOnly()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var onlyPhoenixCart = new ShoppingCart(new List<IDiscount>());
            onlyPhoenixCart.Add(Globals.Items.PhoenixProject);
            Add(onlyPhoenixCart, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, false, 0, methodName);
        }

        private void EmptyCart()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            Add(new ShoppingCart(new List<IDiscount>()), Globals.Items.UnicornProject.SKU,
                Globals.Items.PhoenixProject.SKU, false, 0, methodName);
        }

        private void UnicornProjectOnly()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var onlyUnicornCart = new ShoppingCart(new List<IDiscount>());
            onlyUnicornCart.Add(Globals.Items.UnicornProject);
            Add(onlyUnicornCart, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, false, 0, methodName);
        }

        private void DevOpsHandBookOnly()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var onlyDevOpsCart = new ShoppingCart(new List<IDiscount>());
            onlyDevOpsCart.Add(Globals.Items.DevOpsHandbook);
            Add(onlyDevOpsCart, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, false, 0, methodName);
        }

        private void UnicornProjectAndDevOpsHandbook()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var unicornDevOpsCart = new ShoppingCart(new List<IDiscount>());
            unicornDevOpsCart.Add(Globals.Items.UnicornProject);
            unicornDevOpsCart.Add(Globals.Items.DevOpsHandbook);
            Add(unicornDevOpsCart, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, false, 0, methodName);
        }

        private void UnicornProjectAndPhoenixProject()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var phoenixUnicornCart = new ShoppingCart(new List<IDiscount>());
            phoenixUnicornCart.Add(Globals.Items.PhoenixProject);
            phoenixUnicornCart.Add(Globals.Items.UnicornProject);
            Add(phoenixUnicornCart, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, true, 2, methodName);
        }
        
        private void UnicornProjectAndPhoenixProject2()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var phoenixUnicornCart2 = new ShoppingCart(new List<IDiscount>());
            phoenixUnicornCart2.Add(Globals.Items.UnicornProject);
            phoenixUnicornCart2.Add(Globals.Items.PhoenixProject);
            Add(phoenixUnicornCart2, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, true, 2, methodName);
        }

        private void UnicornProjectAndPhoenixProject2Copies()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var doublePhoenixUnicornCart = new ShoppingCart(new List<IDiscount>());
            doublePhoenixUnicornCart.Add(Globals.Items.PhoenixProject, 2);
            doublePhoenixUnicornCart.Add(Globals.Items.UnicornProject, 2);
            Add(doublePhoenixUnicornCart, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, true, 2, methodName);
        }

        private void AllThreeBooks()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var allBooksCart = new ShoppingCart(new List<IDiscount>());
            allBooksCart.Add(Globals.Items.PhoenixProject);
            allBooksCart.Add(Globals.Items.UnicornProject);
            allBooksCart.Add(Globals.Items.DevOpsHandbook);
            Add(allBooksCart, Globals.Items.UnicornProject.SKU, Globals.Items.PhoenixProject.SKU, true, 2, methodName);
        }
    }

    public class WhenPairBasedDiscountIsNoLongerValid
    {
        //TODO: Add tests to drive out Discount dates
        [Fact]
        public void DiscountAppliesReturnsFalse()
        {
            //Arrange:
            var cart = new ShoppingCart(new List<IDiscount>());
            cart.Add(Globals.Items.PhoenixProject);
            cart.Add(Globals.Items.UnicornProject);
            var sut = new PairBasedDiscount(Globals.Items.PhoenixProject.SKU, Globals.Items.UnicornProject.SKU, 2, "test");
            
            //Act
            var result = sut.DiscountApplies(cart);
            
            //Assert
            Assert.False(result);
        }
    }
}