using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AppDev.Models
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Title { get; set; } = null!;

        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Price must be bigger than zero.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public string StoreId { get; set; } = null!;

        [ValidateNever]
        public Store Store { get; set; } = null!;

        public int? ImageId { get; set; }

        public Image? Image { get; set; } = null!;
    }
}
