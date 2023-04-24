using System.ComponentModel.DataAnnotations;

namespace AppDev.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }
    }
}
