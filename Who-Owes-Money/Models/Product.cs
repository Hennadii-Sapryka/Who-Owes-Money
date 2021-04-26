using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhoOwesWhom.Models
{
    public class Product
    {
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "What buy")]
        public string Name { get; set; }

        [Required]
        //[MaxLength(10)]
        [Range(1, int.MaxValue, ErrorMessage = "лише числа які більше 0")]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Who buy")]
        public string User { get; set; }

        public List<IdentityUser> identytiUsers { get; set; }
    }
}
