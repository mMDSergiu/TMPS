using CoffeeShop.Extras;
using CoffeeShop.Models;

namespace CoffeeShop.Tests;

public class ExtrasTests
{
    [Fact]
    public void Extras_stacked_in_any_order_give_the_same_result()
    {
        IOrderItem first = new ToGo(new GiftBox(Make.Latte()));
        IOrderItem second = new GiftBox(new ToGo(Make.Latte()));

        Assert.Equal(40 + 15 + 3, first.Price);
        Assert.Equal(first.Price, second.Price);

        foreach (var item in new[] { first, second })
        {
            Assert.Contains("to go", item.Description);
            Assert.Contains("gift box", item.Description);
        }
    }

    [Fact]
    public void Extra_can_wrap_a_combo_and_keeps_the_parts()
    {
        var breakfast = Make.Breakfast();

        IOrderItem toGo = new ToGo(breakfast);

        Assert.Equal(58, toGo.Price);
        Assert.Equal("Breakfast combo, to go", toGo.Description);
        Assert.Equal(breakfast.Parts.Count, toGo.Parts.Count);
    }

    [Fact]
    public void Wrapped_item_can_be_cloned_with_its_extras()
    {
        IOrderItem gift = new GiftBox(new ToGo(Make.Latte()));

        var copy = gift.Clone();

        Assert.NotSame(gift, copy);
        Assert.Equal(gift.Price, copy.Price);
        Assert.Equal(gift.Description, copy.Description);
    }
}
