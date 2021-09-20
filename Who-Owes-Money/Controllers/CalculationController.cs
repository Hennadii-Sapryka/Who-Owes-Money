using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Who_Owes_Money.Data;
using Who_Owes_Money.Models;

namespace Who_Owes_Money.Controllers
{
    public class CalculationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userContext;
        public readonly  List<ResultCalculation> _results;

        public CalculationController(ApplicationDbContext context, UserManager<IdentityUser> userContext)
        {
            _context = context;
            _userContext = userContext;
            _results = new List<ResultCalculation>();
        }

        public IActionResult Calculation()
        {
            var products = _context.Product.ToList();

            if (products.FirstOrDefault() == null)
            {
                return View("NotFound");
            }

            double average = Math.Truncate(products.Sum(m => m.Price)
                / products.GroupBy(m => m.UserName).Count());

            Product[] buyers = products
                .GroupBy(m => m.UserName)
                .Select(g => new Product
                {
                    UserName = g.Key,
                    Price = Math.Truncate(g.Sum(p => p.Price))
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
            return View(_results);
        }
    }
}
