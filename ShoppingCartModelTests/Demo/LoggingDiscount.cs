using System;
using ShoppingCartModel;
using Xunit.Abstractions;

namespace ShoppingCartModelTests.Demo
{
    /// <summary>
    /// This class decorates an instance of <see cref="IDiscount"/>, adding logging functionality.
    /// </summary>
    /// <remarks>
    /// <para>This class is for demonstration purposes only.</para>
    /// <para>It demonstrates use of the Decorator pattern to add functionality in
    /// a way the complies with the Open-Closed Principle.</para>
    /// <para>Normally, classes such as this would be placed in the ShoppingCartModel
    /// assembly, i.e. beside the rest of the application code.  However, we want
    /// to use an instance of <see cref="ITestOutputHelper"/> to write our log messages
    /// to the test output window (Console.Log won't work).  Therefore, I've put the class
    /// in the test assembly because I don't want our application assembly to have to take
    /// a reference to xunit.</para>
    /// </remarks>
    public class LoggingDiscount : IDiscount
    {
        private readonly IDiscount _innerDiscount;
        private readonly ITestOutputHelper _testOutputHelper;
        
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <remarks>
        /// <para>Note that we take an instance of <see cref="IDiscount"/> in the constructor.
        /// This is the class we will 'decorate'.  Because this class also implements <see cref="IDiscount"/>
        /// we can call those methods on the decorated instance, if appropriate.</para>
        /// <para>In this case, we are simply wrapping the calls to the decorated class with
        /// log messages.</para></remarks>
        /// <param name="innerDiscount">The instance of <see cref="IDiscount"/> that we want to 'decorate'.</param>
        /// <param name="testOutputHelper">A helper that allows us to write to the Test Output window.</param>
        public LoggingDiscount(IDiscount innerDiscount, ITestOutputHelper testOutputHelper)
        {
            _innerDiscount = innerDiscount ?? throw new ArgumentNullException(nameof(innerDiscount), "The inner discount cannot be null.");
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper), "The test output helper cannot be null.");
        }

        /// <summary>
        /// Wraps the call to the decorated class with log messages.
        /// </summary>
        /// <remarks>
        /// <para>Note the call to the same method on the 'decorated' class.  This is the
        /// Decorator pattern in action.</para>
        /// </remarks>
        /// <param name="cart">The cart to be checked for applying discounts.</param>
        /// <returns>True, if the discount applies. Otherwise, false.</returns>
        public bool DiscountApplies(ShoppingCart cart)
        {
            _testOutputHelper.WriteLine($"About to check if {_innerDiscount.GetType()} applies to the basket.");

            var result = _innerDiscount.DiscountApplies(cart);

            if (result) 
            {
                _testOutputHelper.WriteLine($"The discount {_innerDiscount.GetType()} does apply to the cart.");
            }
            else
            {
                _testOutputHelper.WriteLine($"The discount {_innerDiscount.GetType()} does not apply to the cart.");
            }

            return result;
        }

        /// <summary>
        /// Wraps the call to the decorated class with log messages.
        /// </summary>
        /// <remarks>
        /// <para>Note the call to the same method on the 'decorated' class.  This is the
        /// Decorator pattern in action.</para>
        /// </remarks>
        /// <param name="cart">The cart to get the discount amount for.</param>
        /// <returns>The discount amount.</returns>
        public double GetDiscountAmount(ShoppingCart cart)
        {
            _testOutputHelper.WriteLine($"About to get the discount amount for the cart.");
            var result = _innerDiscount.GetDiscountAmount(cart);
            _testOutputHelper.WriteLine($"A discount of {result} has been applied to the cart.");
            
            return result;
        }
    }
}