using System.ComponentModel.DataAnnotations;

namespace AppDev.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerId { get; set; } = null!;
        public ApplicationUser Customer { get; set; } = null!;

        [StringLength(255)]
        public string Address { get; set; } = null!;

        [StringLength(50)]
        public string FullName { get; set; } = null!;

        [StringLength(20)]
        public string PhoneNumber { get; set; } = null!; public string StoreId { get; set; } = null!;

        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public Store Store { get; set; } = null!;

        [DataType(DataType.Currency)]
        public double TotalPrice { get; set; }

        public List<OrderItem> OrderItems { get; set; } = null!;

        private Order() { }

        public Order(string customerId, string storeId, IEnumerable<CartItem> cartItems)
        {
            CustomerId = customerId;
            StoreId = storeId;
            OrderItems = cartItems.Select(ci => new OrderItem()
            {
                OrderId = Id,
                BookId = ci.BookId,
                Book = ci.Book,
                Quantity = ci.Quantity,
                Price = ci.Book.Price

            }).ToList();

            TotalPrice = OrderItems.Sum(oi => oi.Price * oi.Quantity);
        }

        public Order(string customerId, string storeId, IEnumerable<OrderItem> items)
        {
            CustomerId = customerId;
            StoreId = storeId;

            OrderItems = items.ToList();

            TotalPrice = OrderItems.Sum(oi => oi.Price);
        }
    }
}
