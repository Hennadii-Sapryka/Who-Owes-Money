using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Who_Owes_Money.Data;
using Who_Owes_Money.Models;

namespace Who_Owes_Money.Controllers
{
    public class CalculationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userContext;
        private readonly List<ResultOfCalculation> _results;

        public CalculationController(ApplicationDbContext context, UserManager<IdentityUser> userContext)
        {
            _context = context;
            _userContext = userContext;
            _results = new List<ResultOfCalculation>();
        }

        public IActionResult Calculation()
        {
            var products = _context.Product.ToList();

            if (products.FirstOrDefault() == null)
            {
                return View("NotFound");
            }

            decimal average = products
                .Sum(m => m.Price)
                / products
                .GroupBy(m => m.UserName)
                .Count();

            Product[] people = products
                .GroupBy(m => m.UserName)
                .Select(g => new Product
                {
                    UserName = g.Key,
                    Price = g.Sum(p => p.Price)
                }).ToArray();

            Product[] overPay = people.Where(m => m.Price > average).ToArray();
            Product[] underPay = people.Where(m => m.Price < average).ToArray();

            for (int i = 0; i < overPay.Length; i++)
            {
                for (int j = 0; j < underPay.Length; j++)
                {
                    decimal count = 0;
                    while (underPay[j].Price != average)
                    {
                        if (overPay[i].Price != average)
                        {
                            overPay[i].Price -= 0.01m;
                            underPay[j].Price += 0.01m;
                            count++;
                        }
                        if (underPay[j].Price == average)
                        {
                            _results.Add(new ResultOfCalculation
                            {
                                UnderPay = underPay[j].UserName,
                                OverPay = overPay[i].UserName,
                                Amount = count/100,
                                Average = average,
                            });
                        }
                        else if (overPay[i].Price == average)
                        {
                            _results.Add(new ResultOfCalculation
                            {
                                UnderPay = underPay[j].UserName,
                                OverPay = overPay[i].UserName,
                                Amount = count/100
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
