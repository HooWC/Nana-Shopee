using Microsoft.AspNetCore.Mvc;

namespace A1.Controllers
{
    public class WaitingController : Controller
    {
        public IActionResult Wait()
        {
            return View();
        }

        public IActionResult WaitHistory()
        {
            return View();
        }

        public IActionResult WaitShoppingCart()
        {
            return View();
        }

        public IActionResult WaitProduct()
        {
            return View();
        }

        public IActionResult WaitSetting()
        {
            return View();
        }

    }
}
