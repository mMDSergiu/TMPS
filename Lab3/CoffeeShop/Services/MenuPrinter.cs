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

        output.WriteLine("COMBOS");
        foreach (var combo in menu.Combos)
        {
            var inside = string.Join(" + ", combo.Parts.Select(p => p.Description));
            output.WriteLine($"  {combo.Description,-17}{combo.Price,5} lei   ({inside})");
        }
    }
}
