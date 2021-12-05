using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloCMS.Identity.Data.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string? Token { get; set; }
        public string? JwtId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateExpire { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppIdentityUser? User { get; set; }
    }
}
