using AppDev.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AppDev.Models
{
    public class Image
    {
        public int Id { get; set; }

        public Book Book { get; set; } = null!;

        [StringLength(255)]
        public string Name { get; set; } = null!;

        [StringLength(10)]
        public string Extension { get; set; } = null!;

        [StringLength(1000)]
        public string Path { get; private set; } = null!;

        [StringLength(1000)]
        public string Href { get; private set; } = null!;

        private Image() { }

        public Image(string name, string extension)
        {
            Name = name;
            Extension = extension;

            var randomFileName = $"{System.IO.Path.GetRandomFileName()}.{extension}";
            Path = System.IO.Path.Combine(FileUploadHelper.BookImageBaseDirectory, randomFileName);
            Href = System.IO.Path.Combine(FileUploadHelper.BookImageBaseHref, randomFileName);
        }
    }
}
