using CoffeeShop.Output;

namespace CoffeeShop.Services;

public class MenuPrinter
{
    private readonly IOutput output;

    public MenuPrinter(IOutput output)
    {
        this.output = output;
    }

    public void Print(IMenu menu)
    {
        output.WriteLine("MENU");
        foreach (var p in menu.Prices)
            output.WriteLine($"  {p.Key,-12}{p.Value,5} lei");
    }
}
