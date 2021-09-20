using System.Collections.Generic;
using ShoppingCartModel;
using Xunit;

namespace ShoppingCartModelTests.Unit
{
    public class WhenTheDiscountDoesNotApply
    {
        private readonly DevOpsHandbookDiscount _sut;
        private readonly ShoppingCart _cart;
        
        public WhenTheDiscountDoesNotApply()
        {
            _sut = new DevOpsHandbookDiscount();
            _cart = new ShoppingCart(new List<IDiscount> { _sut });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void CallingDiscountAppliesReturnsFalse(int quantity)
        {
            for(var i = 0; i < quantity; i++)
            {
                _cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99));
            }

            var result = _sut.DiscountApplies(new ShoppingCart(new List<IDiscount>()));
            
            Assert.False(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void CallingGetDiscountAmountReturnsZero(int quantity)
        {
            for(var i = 0; i < quantity; i++)
            {
                _cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99));
            }

            var result = _sut.GetDiscountAmount(new ShoppingCart(new List<IDiscount>()));
            
            Assert.Equal(0, result);
        }
    }

    public class WhenTheDiscountDoesApply
    {
        private readonly DevOpsHandbookDiscount _sut;
        private readonly ShoppingCart _cart;
        
        public WhenTheDiscountDoesApply()
        {
            _sut = new DevOpsHandbookDiscount();
            _cart = new ShoppingCart(new List<IDiscount> { _sut });
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(10)]
        public void CallingDiscountAppliesReturnsTrue(int quantity)
        {
            _cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), quantity);

            var result = _sut.DiscountApplies(_cart);

            Assert.True(result);
        }

        [Theory]
        [InlineData(5, 7.45)]
        [InlineData(6, 7.45)]
        [InlineData(10, 14.90)]
        public void CallingGetDiscountAmountReturnsCorrectAmount(int quantity, double expectedDiscount)
        {
            _cart.Add(new Item("ABC1234567890123", "The DevOps Handbook", 19.99), quantity);
            
            var result = _sut.GetDiscountAmount(_cart);
            
            Assert.Equal(expectedDiscount, result);
        }
    }
}