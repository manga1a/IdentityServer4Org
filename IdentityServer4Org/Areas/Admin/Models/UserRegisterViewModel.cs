﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServer4Org.Areas.Admin.Models
{
    public class UserRegisterViewModel
    {
        [Required]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
