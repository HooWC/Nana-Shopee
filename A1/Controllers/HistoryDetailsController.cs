using A1.Data;
using A1.Models;
using Microsoft.AspNetCore.Mvc;

namespace A1.Controllers
{
    public class HistoryDetailsController : Controller
    {
        private readonly SaveData _context;

        public HistoryDetailsController(SaveData context)
        {
            _context = context;
        }

        public static List<Cart> cart = new List<Cart>();

        public IActionResult Details(int? id)
        {
            cart.Clear();
            //table
            var tr = _context.TrTable.Where(x => x.Id == id).FirstOrDefault();

            string[] str = tr.CartList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var i in str)
            {
                var c = _context.CartTable.Where(x => x.Id == Convert.ToInt32(i)).FirstOrDefault();
                cart.Add(c);
            }

            ViewBag.time = tr.PaidTime;
            ViewBag.Amount = tr.GrandTotal;
            ViewBag.tax = tr.Tax;
            ViewBag.Username = tr.UserName;
            ViewBag.Address = tr.BillingAddress;
            ViewBag.TrID = tr.TransactionId;
            return View(cart);
        }
    }
}
