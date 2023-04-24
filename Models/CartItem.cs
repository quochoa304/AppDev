namespace AppDev.Models
{
    public class CartItem
    {
        public string CustomerId { get; set; } = null!;
        public ApplicationUser Customer { get; set; } = null!;

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public int Quantity { get; set; } = 1;
    }
}
