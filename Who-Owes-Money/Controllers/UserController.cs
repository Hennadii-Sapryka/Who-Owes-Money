using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Who_Owes_Money.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _identityUser;

        public UserController(UserManager<IdentityUser> identityUser)
        {
            _identityUser = identityUser;
        }
        public IActionResult Index()=> View();
    }
}
