using CoffeeShop.Models;

namespace CoffeeShop.Tests;

public class ComboTests
{
    [Fact]
    public void Single_items_and_combos_are_used_through_the_same_interface()
    {
        var items = new List<IOrderItem> { Make.Latte(), new Pastry("Cookie", 15), Make.Breakfast() };

        decimal total = items.Sum(i => i.Price);

        Assert.Equal(40 + 15 + 55, total);
        Assert.All(items, i => Assert.False(string.IsNullOrEmpty(i.Description)));
        Assert.Empty(items[0].Parts);
        Assert.Equal(2, items[2].Parts.Count);
    }

    [Fact]
    public void Combo_inside_a_combo_is_priced_from_all_parts()
    {
        var family = new Combo("Family", 12)
            .Add(Make.Breakfast())
            .Add(Make.Breakfast())
            .Add(new Pastry("Cookie", 15));

        Assert.Equal(55 + 55 + 15 - 12, family.Price);
    }

    [Fact]
    public void Cloned_combo_does_not_share_its_parts()
    {
        var breakfast = Make.Breakfast();
        var copy = (Combo)breakfast.Clone();

        copy.Add(new Pastry("Cookie", 15));

        Assert.Equal(2, breakfast.Parts.Count);
        Assert.Equal(3, copy.Parts.Count);
        Assert.NotSame(breakfast.Parts[0], copy.Parts[0]);
    }
}
