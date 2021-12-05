using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloCMS.Identity.Data.Models
{
    public class AppIdentityUser : IdentityUser<int>
    {
        [Required, StringLength(10)]
        public string? Salutation { get; set; }

        [Required, StringLength(50)]
        public string? FirstName { get; set; }

        [Required, StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? SecondaryEmail { get; set; }

        [Required, StringLength(4)]
        public string? YearOfBirth { get; set; }
        [StringLength(50)]
        public string? Website { get; set; }

    }
}
