﻿using System.ComponentModel.DataAnnotations;

namespace HelloCMS.LoginApi.Data.ViewModels
{
    /// <summary>
    /// Login View Model
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    public record LoginVM(
                [Required] string UserName,
                [Required] string Password
        );
}
