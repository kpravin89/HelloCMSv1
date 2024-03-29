﻿using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace HelloCMS.Identity.Data.Dto.Users
{
    public record RegisterUserDto([Required, StringLength(10)] string Salutation,
                [Required, StringLength(50)] string FirstName,
                [Required, StringLength(50)] string LastName,
                [Required, StringLength(50)] string UserName,
                [Required, StringLength(50)] string Password,
                [Required, StringLength(50)] string ConfirmPassword,
                [EmailAddress, Required, StringLength(100)] string Email,
                [EmailAddress, StringLength(100)] string SecondaryEmail,
                [Required, StringLength(4)] string YearOfBirth,
                [StringLength(100)] string Website,
                [Required] string Role
        );
}
