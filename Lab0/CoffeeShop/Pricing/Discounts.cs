namespace CoffeeShop.Pricing;


public class NoDiscount : IDiscount
{
    public string Name => "none";
    public decimal Apply(decimal price) => price;
}

public class StudentDiscount : IDiscount
{
    public string Name => "student 10%";
    public decimal Apply(decimal price)
    {
        return price - price * 0.1m;
    }
}


public class HappyHourDiscount : IDiscount
{
    public string Name => "happy hour 20%";

    public decimal Apply(decimal price)
    {
        return price * 0.8m;
    }
}
