using System.ComponentModel.DataAnnotations;

namespace AppDev.Models
{
    public class Category
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; } = null!;

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
