using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Who_Owes_Money.Data;
using Who_Owes_Money.Models;

namespace Who_Owes_Money.Controllers
{
    public class CalculationController : Controller
    {
        private readonly Product _product;
        private readonly ApplicationDbContext _context;
        public CalculationController(Product product, ApplicationDbContext context)
        {
            _product = product;
            _context = context;
        }

        public IActionResult Calculation()
        {
            if (!_context.Product.ToList().Any())
                return View("NotFound");

            return View(_product.getCalculationsFromBoughtProducts());
        }
    }
}
