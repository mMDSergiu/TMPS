namespace CoffeeShop.Models
{
    public class Order
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
    }
}
