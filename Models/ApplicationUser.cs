using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppDev.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string FullName { get; set; } = null!;

        [StringLength(100)]
        public string HomeAddress { get; set; } = null!;
    }
}
