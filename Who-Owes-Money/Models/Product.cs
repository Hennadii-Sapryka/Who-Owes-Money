using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Who_Owes_Money.Data;

namespace Who_Owes_Money.Models
{
    public class Product
    {
        private readonly ApplicationDbContext _context;
        private readonly List<ResultCalculation> _results;

        [Display(Name = "№")]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "What bought")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString ="{0:c2}")]
        public double Price { get; set; }

        public string UserName { get; set; }


        public List<ResultCalculation> GetListOfBuers()
        {
            var products = _context.Product.ToList();

            double average = Math.Round(products.Sum(m => m.Price))
                / products.GroupBy(m => m.UserName).Count();

            Product[] buyers = products
                .GroupBy(m => m.UserName)
                .Select(g => new Product
                {
                    UserName = g.Key,
                    Price = g.Sum(p => p.Price)
                }).ToArray();

            Product[] buyersOverPaid = buyers.Where(m => m.Price > average).ToArray();
            Product[] buyersUnderPaid = buyers.Where(m => m.Price < average).ToArray();

            for (int i = 0; i < buyersOverPaid.Length; i++)
            {
                for (int j = 0; j < buyersUnderPaid.Length; j++)
                {
                    int count = 0;
                    while (buyersUnderPaid[j].Price != average)
                    {
                        if (buyersOverPaid[i].Price != average)
                        {
                            buyersOverPaid[i].Price -= 1;
                            buyersUnderPaid[j].Price += 1;
                            count++;
                        }
                        if (buyersUnderPaid[j].Price == average)
                        {
                            _results.Add(new ResultCalculation
                            {
                                UserUnderPaid = buyersUnderPaid[j].UserName,
                                UserOverPaid = buyersOverPaid[i].UserName,
                                Money = count,
                                Average = (int)average,
                            });
                        }
                        else if (buyersOverPaid[i].Price == average)
                        {
                            _results.Add(new ResultCalculation
                            {
                                UserUnderPaid = buyersUnderPaid[j].UserName,
                                UserOverPaid = buyersOverPaid[i].UserName,
                                Money = count
                            });
                            i++;
                            count = 0;
                        }
                    }
                }
            }

            return _results;
        }
    }

}
