using CoffeeShop.Output;

namespace CoffeeShop.Payments;

public class CashPayment : IPaymentMethod
{
    private decimal cash;
    private readonly IOutput output;

    public CashPayment(decimal cash, IOutput output)
    {
        this.cash = cash;
        this.output = output;
    }

    public bool Pay(decimal amount)
    {
        if (cash < amount)
        {
            output.WriteLine("Not enough money! need " + amount + " lei, got " + cash);
            return false;
        }
        output.WriteLine($"Paid cash: {cash} lei, change: {cash - amount} lei");
        return true;
    }
}
