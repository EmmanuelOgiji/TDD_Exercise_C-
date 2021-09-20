using System;

namespace ShoppingCartModel
{
    public class Money
    {
        private readonly double _value;
        
        public double Value => Math.Floor(_value * 100) / 100;

        public Money(double value)
        {
            _value = value;
        }
    }
}