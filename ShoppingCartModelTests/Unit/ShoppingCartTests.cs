using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShoppingCartModel;
using Xunit;

namespace ShoppingCartModelTests.Unit
{
    public class WhenTheShoppingCartIsEmpty
    {
        [Fact]
        public void TheSubTotalShouldBeZero()
        {
            var sut = new ShoppingCart(new List<IDiscount>());
            var result = sut.SubTotal;
            
            Assert.Equal(0, result);
        }

        [Fact]
        public void TheItemsCollectionShouldBeEmpty()
        {
            var sut = new ShoppingCart(new List<IDiscount>());

            var result = sut.GetAllItems().Count();
            
            Assert.Equal(0, result);
        }

        [Fact]
        public void GettingTheSubtotalShouldAskAllDiscountsIfTheyApply()
        {
            var mockDiscount = new Mock<IDiscount>();
            var mockDiscount2 = new Mock<IDiscount>();
            var sut = new ShoppingCart(new List<IDiscount> {mockDiscount.Object, mockDiscount2.Object});

            var subtotal = sut.SubTotal;
            
            mockDiscount.Verify(x => x.DiscountApplies(It.IsAny<ShoppingCart>()), Times.Once());
            mockDiscount2.Verify(x => x.DiscountApplies(It.IsAny<ShoppingCart>()), Times.Once());
        }
    }

    public class WhenTheShoppingCartContainsOneItem
    {
        [Fact]
        public void TheSubTotalShouldMatchThePriceOfTheItem()
        {
            var sut = new ShoppingCart(new List<IDiscount>());
            sut.Add(new ShoppingCartModel.Item("ABC1234567890123", "The DevOps Handbook", 9.99));
            
            var result = sut.SubTotal;
            
            Assert.Equal(9.99 ,result);
        }

        [Fact]
        public void TheItemsCollectionShouldContainThatItem()
        {
            var sut = new ShoppingCart(new List<IDiscount>());
            sut.Add(new ShoppingCartModel.Item("ABC1234567890123", "The DevOps Handbook", 19.99));

            var items = sut.GetAllItems();

            Assert.Contains(items, x => x.Name.Equals("The DevOps Handbook"));
        }
    }

    public class WhenAddingMoreThanOneItemToTheShoppingCart
    {
        [Fact]
        public void TheSubTotalShouldBeTheSumOfThePriceOfBothItems()
        {
            // Arrange
            var sut = new ShoppingCart(new List<IDiscount>());
            sut.Add(new ShoppingCartModel.Item("ABC1234567890123", "The DevOps Handbook",9.99));
            sut.Add(new ShoppingCartModel.Item("ABC1234567890123", "The DevOps Handbook", 10));

            // Act
            var result = sut.SubTotal;
            
            // Assert
            Assert.Equal(19.99, result);
        }
    }

    public class WhenAddingMoreThanOneItemOfTheSameTypeToTheShoppingCart
    {
        [Fact]
        public void TheSubTotalShouldBeTheSumOfThePriceOfAllItems()
        {
            // Arrange
            var sut = new ShoppingCart(new List<IDiscount>());
            sut.Add(new ShoppingCartModel.Item("ABC1234567890123", "The DevOps Handbook",9.99), 3);
            
            // Act
            var result = sut.SubTotal;
            
            // Assert
            Assert.Equal(29.97, result);
        }
    }

    public class WhenAddingANullItemToTheShoppingCart
    {
        [Fact]
        public void ShouldThrowANullArgumentException()
        {
            //Arrange
            var sut = new ShoppingCart(new List<IDiscount>());
            
            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.Add(null));
        }

        [Fact]
        public void ShouldProvideAMeaningfulErrorDescription()
        {
            var message = string.Empty;
            
            //Arrange
            var sut = new ShoppingCart(new List<IDiscount>());
            
            // Act
            try
            {
                sut.Add(null);
            }
            catch (ArgumentNullException anex)
            {
                message = anex.Message;
            }
            
            // Assert
            Assert.Contains("Cannot add a null item to the shopping cart.", message);
        }
    }

    public class WhenAddingMoreThanOneNullItemToTheShoppingCart
    {
        [Fact]
        public void ShouldThrowAnArgumentNullException()
        {
            //Arrange
            var sut = new ShoppingCart(new List<IDiscount>());
            
            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => sut.Add(null, 1));
        }
        
        [Fact]
        public void ShouldProvideAMeaningfulErrorDescription()
        {
            var message = string.Empty;
            
            //Arrange
            var sut = new ShoppingCart(new List<IDiscount>());
            
            // Act
            try
            {
                sut.Add(null, 1);
            }
            catch (ArgumentNullException anex)
            {
                message = anex.Message;
            }
            
            // Assert
            Assert.Contains("Cannot add a null item to the shopping cart.", message);
        }
    }

    public class WhenRetrievingTheListOfItemsFromTheShoppingCart
    {
        [Fact]
        public void AddingAnItemToTheListShouldNotAffectTheCart()
        {
            // Arrange
            var sut = new ShoppingCart(new List<IDiscount>());
            sut.Add(new ShoppingCartModel.Item("ABC1234567890123", "The DevOps Handbook", 19.99));
            var items = sut.GetAllItems();
            
            // Act
            items.Add(new ShoppingCartModel.Item("DEF1234567890123", "The Phoenix Project", 14.99));
            
            Assert.Contains(sut.GetAllItems(), x => x.Name.Equals("The DevOps Handbook"));
            Assert.Equal(1, sut.GetAllItems().Count);
        }
    }
    
    public class WhenAddingAnItemToTheShoppingCartWithAQuantityOfZero
    {
        
    }
    
    public class WhenAddingAnItemToTheShoppingCartWithANegativeQuantity
    {
        
    }
}
