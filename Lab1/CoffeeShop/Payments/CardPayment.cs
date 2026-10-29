using CoffeeShop.Output;

namespace CoffeeShop.Payments;

public class CardPayment : IPaymentMethod
{
    private string cardNumber;
    private readonly IOutput output;

    public CardPayment(string cardNumber, IOutput output)
    {
        this.cardNumber = cardNumber;
        this.output = output;
    }

    public bool Pay(decimal amount)
    {
        string last4 = cardNumber.Substring(cardNumber.Length - 4);

        output.WriteLine($"Paid {amount} lei with card ****{last4}");
        return true;
    }
}
