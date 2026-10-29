namespace CoffeeShop.Models
{
    public class Order : IPrototype<Order>
    {
        static int counter = 1;


        public int Id { get; private set; }


        public string CustomerName { get; set; }
        public List<Drink> Drinks { get; } = new List<Drink>();

        public Order(string customerName)
        {
            Id = counter++;
            CustomerName = customerName;
        }

        public Order Clone()
        {
            var copy = new Order(CustomerName);
            foreach (var d in Drinks)
                copy.Drinks.Add(d.Clone());

            return copy;
        }
    }
}
