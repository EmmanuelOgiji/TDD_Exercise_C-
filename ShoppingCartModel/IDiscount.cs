namespace ShoppingCartModel
{
    public interface IDiscount
    {
        bool DiscountApplies(ShoppingCart cart);

        double GetDiscountAmount(ShoppingCart cart);
    }
}