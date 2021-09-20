using System;

namespace ShoppingCartModelTests
{
    public static class TestUtils
    {
        public static string GetExceptionMessage<TException>(Func<object> func) where TException : Exception
        {
            var message = string.Empty;

            try
            {
                func.Invoke();
            }
            catch (TException e)
            {
                message = e.Message;
            }

            return message;
        }
    }
}