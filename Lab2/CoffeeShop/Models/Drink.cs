namespace CoffeeShop.Models;

public enum DrinkSize { Small, Medium, Large }

public class Drink : IPrototype<Drink>
{
    public string Name { get; internal set; } = "";
    public DrinkSize Size { get; internal set; } = DrinkSize.Medium;
    public string? Milk { get; internal set; }

    public int ExtraShots { get; internal set; }
    public List<string> Syrups { get; internal set; } = new List<string>();
    public decimal Price { get; internal set; }


    public Drink Clone()
    {
        var copy = (Drink)MemberwiseClone();
        copy.Syrups = new List<string>(Syrups);
        return copy;
    }

    public override string ToString()
    {
        string s = $"{Name} {Size.ToString().ToLower()}";
        if (Milk != null) s += ", " + Milk + " milk";
        if (ExtraShots > 0)
            s += $", +{ExtraShots} shot";
        foreach (var syrup in Syrups)
            s += ", " + syrup;
        return s;
    }
}
