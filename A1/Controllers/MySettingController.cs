using A1.Data;
using Microsoft.AspNetCore.Mvc;

namespace A1.Controllers
{
    public class MySettingController : Controller
    {
        private readonly SaveData _context;

        public MySettingController(SaveData context)
        {
            _context = context;
        }

        public static string Myback = "snow";

        //设定界面
        public IActionResult Setting(string back)
        {
            //back = (Myback == "black") ? "1" : "0";
            if (MyHomePageController.user.Count() == 0)
            {
                return RedirectToAction("LoginPage", "MyHomePage");
            }

            var u = MyHomePageController.user.FirstOrDefault();
            var newuser = _context.UserTable.Where(x => x.Id == u.Id).FirstOrDefault();
            if (back != null)
            {
                //第二次以上
                if (back == "1")
                {
                    ViewBag.MyBackground = "black";
                    Myback = ViewBag.MyBackground;
                }
                else
                {
                    ViewBag.MyBackground = "snow";
                    Myback = ViewBag.MyBackground;
                }
            }
            else
            {
                //一开始 snow
                if (Myback == "black")
                {
                    ViewBag.MyBackground = "black";
                    Myback = ViewBag.MyBackground;
                }
                else
                {
                    ViewBag.MyBackground = "snow";
                    Myback = ViewBag.MyBackground;
                }
            }



            return View(newuser);
        }

        //关于我们
        public IActionResult ContactUs()
        {
            return View();
        }

    }
}
