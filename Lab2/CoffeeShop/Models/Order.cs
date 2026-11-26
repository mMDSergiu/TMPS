namespace CoffeeShop.Models
{
    public class Order : ICopyable<Order>
    {
        static int counter = 1;


        public int Id { get; private set; }


        public string CustomerName { get; set; }
        public List<IOrderItem> Items { get; } = new List<IOrderItem>();

        public Order(string customerName)
        {
            Id = counter++;
            CustomerName = customerName;
        }

        public Order Clone()
        {
            var copy = new Order(CustomerName);
            foreach (var item in Items)
                copy.Items.Add(item.Clone());

            return copy;
        }
    }
}
