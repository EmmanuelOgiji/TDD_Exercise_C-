using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using ShoppingCartModel;
using Xunit;

namespace ShoppingCartModelTests.Unit
{
    public class WhenRetrievingValueOfMoney
    {
        [Theory]
        [InlineData(0.0, 0.0)]
        [InlineData(0.103, 0.1)]
        [InlineData(1.999, 1.99)]
        [InlineData(2.999, 2.99)]
        [InlineData(5.47896, 5.47)]
        public void ShouldRoundDown(double initialValue, double expectedValue)
        {
            var sut = new Money(initialValue);

            var result = sut.Value;
            
            Assert.Equal(result, expectedValue);
        }
    }
}