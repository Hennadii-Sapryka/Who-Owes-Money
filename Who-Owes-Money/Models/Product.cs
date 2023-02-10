using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Who_Owes_Money.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "What bought")]
        public string ProductName { get; set; }

    
        [Range(0.01, 99999.99, ErrorMessage = "лише числа які більше 0")]
        public decimal Price { get; set; }

        public string UserName { get; set; }
    }
}
