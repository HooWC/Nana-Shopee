using Microsoft.AspNetCore.Mvc;

namespace A1.Controllers
{
    public class SuccessPageController : Controller
    {
        public IActionResult SuccessBuy()
        {
            return View();
        }
    }
}
