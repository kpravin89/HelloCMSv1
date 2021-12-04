using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HelloCMS.LoginApi.Data.Models
{
    public class AppIdentityUser : IdentityUser
    {

        [Required]
        public string? Salutation { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? SecondaryEmail { get; set; }
        [Required]
        public string? YearOfBirth { get; set; }

        public string? Website { get; set; }

    }
}
