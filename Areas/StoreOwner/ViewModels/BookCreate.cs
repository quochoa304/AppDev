using System.ComponentModel.DataAnnotations;

namespace AppDev.Areas.StoreOwner.ViewModels
{
    public class BookCreate
    {
        [StringLength(255)]
        public string Title { get; set; } = null!;

        public int CategoryId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be bigger than zero.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public string StoreId { get; set; } = null!;

        public IFormFile UploadImage { get; set; } = null!;
    }
}
