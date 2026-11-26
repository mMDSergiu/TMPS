namespace CoffeeShop.Payments;

public interface IPaymentMethod
{
    bool Pay(decimal amount);
}
