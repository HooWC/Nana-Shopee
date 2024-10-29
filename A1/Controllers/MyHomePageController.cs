using A1.CreditCardFunction;
using A1.Data;
using A1.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace A1.Controllers
{
    public class MyHomePageController : Controller
    {
        #region 全局变量
        private readonly DtoStripeSecrets _stripeSecrets;

        static DtoStripeSecrets stripeSecrets = new DtoStripeSecrets()
        {
            SecretKey = "sk_test_51LFrBPCDMjmOjphDkCD5knDL4oLcb1bb6XMIOHIZ3AizXlVnzdS6xcgVYbkxPTdg35VrT5Rd5xME04Cq5PoOt7D100pS55JR49",
            PublishableKey = "pk_test_51LFrBPCDMjmOjphDmhCWgsFP5QXCXxv0p1oFE0ySjBUHQ0CwKAeuxeCNEYl8gtkXo4NZsYTDiD8Hf0KPQKVUA2mJ00FWY79rOk"
        };

        public List<Models.Product> pd = new List<Models.Product>();

        public static List<Tr> tr = new List<Tr>();

        public List<Cart> cart = new List<Cart>();

        public static List<User> user = new List<User>();

        public static List<Master> Mas = new List<Master>();

        public static List<Comment> com = new List<Comment>();

        private static double Subtotal = 0;

        private static string CurrentTransactionId = "";

        private static string logincheng = "null";

        private readonly SaveData _context;

        public MyHomePageController(SaveData context)
        {
            _context = context;
        }

        #endregion

        #region 方法
        //主页
        public IActionResult Index()
        {
            ViewBag.background = MySettingController.Myback;
            ViewBag.loginchanges = logincheng;
            pd = _context.ProductTable.Where(x => x.Quantity >= 1).ToList();
            return View(pd);
        }

        //产品
        public IActionResult ProductPage(string filterType)
        {
            ViewBag.background = MySettingController.Myback;
            ViewBag.loginchanges = logincheng;
            if (filterType == "1")
            {
                pd = _context.ProductTable.OrderBy(x => x.Price).ToList();
                return View(pd);
            }
            else if (filterType == "2")
            {
                pd = _context.ProductTable.OrderByDescending(x => x.Price).ToList();
                return View(pd);
            }
            else if (filterType == "0")
            {
                pd = _context.ProductTable.Where(x => x.Quantity >= 1).ToList();
                return View(pd);
            }
            else
            {
                pd = _context.ProductTable.Where(x => x.Quantity >= 1).ToList();
                return View(pd);
            }
        }

        //搜寻产品
        [HttpPost, ActionName("ProductPage")]
        [ValidateAntiForgeryToken]
        public IActionResult ProductPagePOST(string search)
        {
            if (search == null)
            {
                TempData["error"] = "Can't Enter Empty Value";
                return RedirectToAction("ProductPage");
            }
            pd = _context.ProductTable.Where(x => x.Name.Contains(search)).ToList();
            return View(pd);
        }

        //注册
        public IActionResult SignUpPage()
        {
            MySettingController.Myback = "snow";
            logincheng = "null";
            return View();
        }

        //注册 信息
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpPage(User user, string p1)
        {
            if (user != null)
            {
                if (user.UserName == null || user.Address == null || user.Email == null || user.AddressZip == null)
                {

                    return View(user);
                }

                if (user.AddressZip.Length > 5)
                {
                    TempData["error"] = "Address cannot exceed 5 digits.";
                    return View(user);
                }

                if (user.UserName.Length > 8)
                {
                    TempData["error"] = "Name cannot exceed 8 digits.";
                    return View(user);
                }

                bool b = int.TryParse(p1, out int pass);
                if (b == true)
                {
                    if (p1.Length < 6)
                    {
                        TempData["error"] = "Password must be more than 6 digits.";
                        return View(user);
                    }

                    var vx1 = _context.UserTable.Where(x => x.Email == user.Email).FirstOrDefault();
                    var vx2 = _context.UserTable.Where(x => x.Password == p1).FirstOrDefault();

                    if (vx1 != null)
                    {
                        TempData["error"] = "Please use another gmail.";
                        return View(user);
                    }
                    else if (vx2 != null)
                    {
                        TempData["error"] = "Please use another password.";
                        return View(user);
                    }

                    user.Password = p1;
                    user.CreateDate = DateTime.Now;
                    user.Hurl = "himg.png";
                    user.Money = 0;
                    _context.UserTable.Add(user);
                    _context.SaveChanges();
                    TempData["success"] = "Created Successfully";
                    return RedirectToAction("LoginPage");
                }
                else
                {
                    TempData["error"] = "Password can only enter numbers.";
                    return View(user);
                }
            }

            return View(user);
        }

        //登入
        public IActionResult LoginPage(string clear)
        {
            MySettingController.Myback = "snow";
            logincheng = "null";
            if (clear == "true")
            {
                user.Clear();
            }
            return View();
        }

        //登入 信息
        [HttpPost]
        public IActionResult LoginPage(string Email, string Password, string signout)
        {

            var vs = _context.UserTable.Where(x => x.Email == Email && x.Password == Password).FirstOrDefault();

            if (signout == "Sign Out")
            {
                logincheng = "null";
                var vv = user.FirstOrDefault();
                _context.SaveChanges();
                user.Clear();
                TempData["success"] = "Sign Out Success";
                return RedirectToAction("LoginPage");
            }

            if (Email == "Hoo9096844hwc" && Password == "084487")
            {
                logincheng = "null";
                user.Clear();
                return RedirectToAction("OldMaster");
            }

            if (vs != null)
            {
                user = _context.UserTable.Where(x => x.Email == Email && x.Password == Password).ToList();
                _context.SaveChanges();
                TempData["success"] = "Login Success";
                logincheng = "success";
                return RedirectToAction("WaitHistory", "Waiting");
            }
            else
            {
                logincheng = "null";
                TempData["error"] = "Login Failed";
                return RedirectToAction("LoginPage");
            }


        }

        //登出
        public IActionResult Signoutpage()
        {
            logincheng = "null";
            user.Clear();
            TempData["success"] = "Sign Out Success";
            return RedirectToAction("LoginPage");
        }

        //添加产品
        public IActionResult BuyPage(int? Id)
        {
            ViewBag.loginchanges = logincheng;
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _context.ProductTable.Find(Id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //添加产品  进入  购物车里
        [HttpPost, ActionName("BuyPage")]
        [ValidateAntiForgeryToken]
        public IActionResult BuyPagePOST(int? id)
        {
            if (user.Count() == 0)
            {
                TempData["error"] = "Please Login";
                return RedirectToAction("LoginPage");
            }

            var o = _context.ProductTable.Find(id);
            if (o == null)
            {
                return NotFound();
            }

            var u = user.FirstOrDefault();
            var gm = _context.CartTable.Where(x => x.UserID == u.Id && x.Product_ID == o.Product_ID).FirstOrDefault();

            //加入购物车
            if (gm == null)
            {
                _context.CartTable.Add(new Cart()
                {
                    UserID = u.Id,
                    UserName = u.UserName,
                    Img = o.Img,
                    ProductName = o.Name,
                    Product_ID = o.Product_ID,
                    Quantity = 1,
                    LastQuantity = 1,
                    Product_Price = o.Price,
                    Buy = false
                });

                o.Quantity -= 1;
                o.BuyCount += 1;

            }
            else
            {
                if (gm.Buy == true)
                {
                    gm.Quantity += 1;
                    gm.LastQuantity = 1;
                    gm.Buy = false;
                }
                else if (gm.Buy == false)
                {
                    gm.Quantity += 1;
                    gm.LastQuantity += 1;
                    gm.Buy = false;
                }

                o.Quantity -= 1;
                o.BuyCount += 1;
            }

            _context.SaveChanges();

            TempData["success"] = "Add Success";
            return RedirectToAction("ShoppingCart");
        }

        //忘记密码
        public IActionResult ForgetPassword()
        {
            return View();
        }

        //更换密码
        [HttpPost]
        public IActionResult ForgetPassword(string Email, string p1)
        {
            var sq = _context.UserTable.Where(x => x.Email == Email).FirstOrDefault();

            if (sq == null)
            {
                TempData["error"] = "Do Not Have This Account";
                return RedirectToAction("LoginPage");
            }
            else
            {
                bool b = int.TryParse(p1, out int num);
                if (b == true)
                {
                    if (p1.Length < 6)
                    {
                        TempData["error"] = "Password must be more than 6 digits.";
                        return RedirectToAction("LoginPage");
                    }

                    //密码不要一样  判断
                    var pass = _context.UserTable.Where(x => x.Password == p1).FirstOrDefault();
                    if (pass != null)
                    {
                        TempData["error"] = "Please use another password.";
                        return RedirectToAction("LoginPage");
                    }

                    sq.Password = p1;
                    _context.SaveChanges();

                    //success
                    TempData["success"] = "Success";
                    return RedirectToAction("LoginPage");
                }
                else
                {
                    TempData["error"] = "Password can only enter numbers.";
                    return RedirectToAction("LoginPage");
                }
            }

            return View();
        }

        //购物车
        public IActionResult ShoppingCart()
        {
            ViewBag.background = MySettingController.Myback;
            ViewBag.loginchanges = logincheng;
            if (user.Count() == 0)
            {
                return RedirectToAction("LoginPage");
            }

            var vv = user.FirstOrDefault();

            cart = _context.CartTable.Where(x => x.UserID == vv.Id && x.UserName == vv.UserName && x.Buy == false).ToList();

            if (cart.Count() == 0)
            {
                List<Cart> c1 = new List<Cart>()
                {
                    new Cart{Img = "No Data"}
                };
                ViewBag.No = "No Data";
                return View(c1);
            }
            var c = cart.FirstOrDefault();
            var bc = _context.ProductTable.Where(x => x.Product_ID == c.Product_ID).FirstOrDefault();
            ViewBag.bqu = bc.Quantity;
            return View(cart);
        }

        //购物车  产品  增加
        [HttpPost]
        public IActionResult ShoppingCart(int ID, int UserID, string PID, string add)
        {
            ViewBag.background = MySettingController.Myback;
            ViewBag.loginchanges = logincheng;
            var addcart = _context.CartTable.Where(x => x.Id == ID && x.UserID == UserID).FirstOrDefault();

            if (add == "-")
            {

                var product = _context.ProductTable.Where(x => x.Product_ID == PID).FirstOrDefault();
                product.Quantity += 1;
                product.BuyCount -= 1;
                addcart.LastQuantity -= 1;
                addcart.Quantity -= 1;

                if (addcart.Quantity == 0 && addcart.LastQuantity == 0)
                {
                    _context.Remove(addcart);
                }

                if (addcart.Quantity != 0 && addcart.LastQuantity == 0)
                {
                    addcart.Buy = true;
                }

            }
            else if (add == "+")
            {
                var product = _context.ProductTable.Where(x => x.Product_ID == PID).FirstOrDefault();
                if (product.Quantity == 0)
                {
                    TempData["error"] = "Insufficient Number Of Products";
                    return RedirectToAction("ShoppingCart");
                }
                else
                {
                    product.Quantity -= 1;
                    product.BuyCount += 1;
                    addcart.LastQuantity += 1;
                    addcart.Quantity += 1;
                }
            }

            _context.SaveChanges();

            var vv = user.FirstOrDefault();

            cart = _context.CartTable.Where(x => x.UserID == vv.Id && x.UserName == vv.UserName && x.Buy == false).ToList();

            if (cart.Count() == 0)
            {
                List<Cart> c1 = new List<Cart>()
                {
                    new Cart{Img = "No Data"}
                };
                ViewBag.No = "No Data";
                return View(c1);
            }

            return View(cart);
        }

        //购买记录 和 个人信息
        public IActionResult History(string fileName, string block)
        {
            ViewBag.background = MySettingController.Myback;
            ViewBag.loginchanges = logincheng;
            if (user.Count() == 0)
            {
                return RedirectToAction("LoginPage");
            }

            var vv = user.FirstOrDefault();

            var newuserinfo = _context.UserTable.Where(x => x.Id == vv.Id && x.Password == vv.Password).FirstOrDefault();

            tr = _context.TrTable.Where(x => x.UserID == vv.Id && x.UserName == vv.UserName).ToList();
            ViewBag.Hurl = newuserinfo.Hurl;
            ViewBag.Name = newuserinfo.UserName;
            ViewBag.ID = newuserinfo.Id;
            var ds = _context.CommentTable.Where(x => x.User_Id == vv.Id).ToList();
            if (ds.Count() > 0)
            {
                foreach (var i in ds)
                {
                    i.Head = newuserinfo.Hurl;
                    _context.CommentTable.Update(i);
                }
            }
            _context.SaveChanges();

            if (block != null)
            {
                if (block == "block")
                {
                    ViewBag.display = "display:block";
                }
                else if (block == "none")
                {
                    ViewBag.display = "display:none";
                }

                if (tr.Count() == 0)
                {
                    List<Tr> tr2 = new List<Tr>()
                    {
                        new Tr{Id = 0,UserName = vv.UserName,TotalPrice = 0}
                    };
                    ViewBag.money = newuserinfo.Money;
                    return View(tr2);
                }
                else
                {
                    tr = _context.TrTable.Where(x => x.UserID == vv.Id && x.UserName == vv.UserName).ToList();
                    ViewBag.money = newuserinfo.Money;
                    return View(tr);
                }
            }

            if (fileName == null)
            {

                if (tr.Count() == 0)
                {
                    List<Tr> tr2 = new List<Tr>()
                    {
                        new Tr{Id = 0,UserName = vv.UserName,TotalPrice = 0}
                    };
                    ViewBag.money = newuserinfo.Money;
                    ViewBag.display = "display:none";
                    return View(tr2);
                }
            }
            else
            {
                var v3 = _context.UserTable.Where(x => x.UserName == vv.UserName && x.Password == vv.Password).FirstOrDefault();

                if (tr.Count() == 0)
                {
                    List<Tr> tr2 = new List<Tr>()
                    {
                        new Tr{Id = 0,UserName = v3.UserName,TotalPrice = 0}
                    };
                    ViewBag.money = newuserinfo.Money;
                    ViewBag.display = "display:none";
                    return View(tr2);
                }
            }

            //更新输出
            tr = _context.TrTable.Where(x => x.UserID == vv.Id && x.UserName == vv.UserName).ToList();
            var v2 = user.FirstOrDefault();
            ViewBag.money = newuserinfo.Money;
            ViewBag.display = "display:none";
            return View(tr);
        }

        //个人头像
        [HttpPost]
        public async Task<IActionResult> History(IFormFile file)
        {
            if (file == null)
            {
                return RedirectToAction("History");
            }
            else
            {
                var vv = user.FirstOrDefault();
                string filePath = $@"wwwroot/Img/TX/{vv.Id + vv.Email + file.FileName}";
                var v2 = _context.UserTable.Where(x => x.UserName == vv.UserName && x.Password == vv.Password).FirstOrDefault();
                v2.Hurl = vv.Id + vv.Email + file.FileName;
                string src = vv.Id + vv.Email + file.FileName;
                _context.SaveChanges();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                };
                //返回同一页
                return RedirectToAction("History", new { fileName = src });
            }
        }

        //充值
        [HttpPost]
        public IActionResult AddMoney(int money, string card, string n1)
        {
            var u = user.FirstOrDefault();
            if (u.Password == n1)
            {
                if (card != "4242424242424242")
                {
                    TempData["error"] = "Wrong Card Number";
                    return RedirectToAction("History");
                }

                if (money <= 0)
                {
                    TempData["error"] = "Wrong Amount";
                    return RedirectToAction("History");
                }
                var vs = _context.UserTable.Where(x => x.Password == u.Password).FirstOrDefault();
                vs.Money += money;
                vs.OutMoney += money;
                _context.SaveChanges();
                TempData["success"] = "Add Success";
                return RedirectToAction("History");
            }
            else
            {
                TempData["error"] = "Wrong Password";
                return RedirectToAction("History");
            }
        }

        //删除购物车产品
        public IActionResult DelProduct(int ID, int UserID, string PID)
        {
            var delcart = _context.CartTable.Where(x => x.Id == ID && x.UserID == UserID).FirstOrDefault();
            var product = _context.ProductTable.Where(x => x.Product_ID == PID).FirstOrDefault();
            product.Quantity += delcart.LastQuantity;

            if (delcart.Quantity - delcart.LastQuantity <= 0)
            {
                _context.CartTable.Remove(delcart);
            }
            else if (delcart.Quantity - delcart.LastQuantity != 0)
            {
                delcart.Buy = true;
                delcart.LastQuantity = 0;
                delcart.Quantity -= delcart.LastQuantity;
            }
            _context.SaveChanges();

            return RedirectToAction("ShoppingCart");
        }

        //表情
        public IActionResult AddFace(int? id, string face)
        {

            if (user.Count() == 0)
            {
                return RedirectToAction("LoginPage");
            }

            var pro = _context.ProductTable.Where(x => x.Id == id).FirstOrDefault();
            if (face == "1")
            {
                pro.P1 += 1;
            }
            else if (face == "2")
            {
                pro.P2 += 1;
            }
            else if (face == "3")
            {
                pro.P3 += 1;
            }
            else if (face == "4")
            {
                pro.P4 += 1;
            }

            _context.SaveChanges();

            return RedirectToAction("BuyPage", new { Id = id });
        }

        //选择 Master
        public IActionResult OldMaster()
        {
            Mas = _context.MasterTable.ToList();
            return View(Mas);
        }

        //Master 界面
        public IActionResult MasterHome(int? id)
        {
            Mas = _context.MasterTable.Where(x => x.Id == id).ToList();
            var mytotal = _context.TrTable.Sum(x => x.GrandTotal);
            ViewBag.total = mytotal.ToString("0.00");
            var ucount = _context.UserTable.Count();
            ViewBag.ucount = ucount;
            ViewBag.bqcount = (Convert.ToDouble(ucount) / 100).ToString("0.00");
            var pcount = _context.ProductTable.Count();
            ViewBag.pcount = pcount;
            ViewBag.bcount = (Convert.ToDouble(pcount) / 100).ToString("0.00");
            return View(Mas);
        }

        //全部客户 
        public IActionResult MasterUser()
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;
            user = _context.UserTable.ToList();
            return View(user);
        }

        //Search客户
        [HttpPost]
        public IActionResult MasterUser(string search)
        {
            if (search == null)
            {
                TempData["error"] = "Can't Enter Empty Value";
                return RedirectToAction("MasterUser");
            }
            user = _context.UserTable.Where(x => x.UserName.Contains(search)).ToList();
            return View(user);
        }

        //客户信息
        public IActionResult MasterInfoUser(int? id)
        {
            var id2 = Mas.FirstOrDefault();
            ViewBag.Mas_id = id2.Id;
            user = _context.UserTable.Where(x => x.Id == id).ToList();
            return View(user);
        }

        //更改客户信息
        public IActionResult MasterEditUser(int? id)
        {
            var id2 = Mas.FirstOrDefault();
            ViewBag.Mas_id = id2.Id;
            user = _context.UserTable.Where(x => x.Id == id).ToList();
            return View(user);
        }

        //更改客户信息
        [HttpPost]
        public IActionResult MasterEditUser(int Uid, User us, string p1)
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;
            var myuser = _context.UserTable.Where(x => x.Id == Uid).FirstOrDefault();

            if (us.UserName == null)
            {
                TempData["error"] = "Wrong Name";
                return RedirectToAction("MasterInfoUser", new { id = Uid });
            }

            myuser.Gender = us.Gender;
            myuser.UserName = us.UserName;

            if (us.Money < 0)
            {
                TempData["error"] = "Wrong Amount";
                return RedirectToAction("MasterInfoUser", new { id = Uid });
            }

            myuser.CreateDate = myuser.CreateDate;
            myuser.Address = us.Address;
            myuser.Money = us.Money;
            myuser.OutMoney += us.Money;

            bool b = int.TryParse(p1, out int num);
            if (b == true)
            {
                if (p1.Length < 6)
                {
                    TempData["error"] = "Password must be more than 6 digits.";
                    return RedirectToAction("MasterInfoUser", new { id = Uid });
                }

                //如果客户 gmail 跟之前一样
                var vx1 = _context.UserTable.Where(x => x.Email == us.Email).FirstOrDefault();
                var vx2 = _context.UserTable.Where(x => x.Password == p1).FirstOrDefault();

                //新的gmail
                if (vx1 == null)
                {
                    myuser.Email = us.Email;
                }

                if (myuser.Email == us.Email)
                {
                    myuser.Email = us.Email;
                }
                else if (vx1 != null)
                {
                    TempData["error"] = "Please use another gmail.";
                    return RedirectToAction("MasterInfoUser", new { id = Uid });
                }

                if (vx2 == null)
                {
                    myuser.Password = p1;
                }

                if (myuser.Password == p1)
                {
                    myuser.Password = p1;
                }
                else if (vx2 != null)
                {
                    TempData["error"] = "Please use another password.";
                    return RedirectToAction("MasterInfoUser", new { id = Uid });
                }

                //myuser.Email = us.Email;
                //myuser.Password = p1;
            }
            else
            {
                TempData["error"] = "Password can only enter numbers.";
                return RedirectToAction("MasterInfoUser", new { id = Uid });
            }

            _context.SaveChanges();
            return RedirectToAction("MasterInfoUser", new { id = Uid });
        }

        //删除客户
        public IActionResult MasterDeleteUser(int? id)
        {
            var id2 = Mas.FirstOrDefault();
            ViewBag.Mas_id = id2.Id;
            var vs = _context.UserTable.Where(x => x.Id == id).FirstOrDefault();
            _context.UserTable.Remove(vs);
            _context.SaveChanges();
            TempData["success"] = "Delete Success";
            return RedirectToAction("MasterUser");
        }

        //全部产品
        public IActionResult MasterProduct()
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;
            pd = _context.ProductTable.ToList();
            return View(pd);
        }

        //Search产品
        [HttpPost]
        public IActionResult MasterProduct(string search)
        {
            if (search == null)
            {
                TempData["error"] = "Can't Enter Empty Value";
                return RedirectToAction("MasterProduct");
            }
            pd = _context.ProductTable.Where(x => x.Name.Contains(search)).ToList();
            return View(pd);
        }

        //产品信息
        public IActionResult MasterProductInfo(int? id)
        {
            var id2 = Mas.FirstOrDefault();
            ViewBag.Mas_id = id2.Id;
            pd = _context.ProductTable.Where(x => x.Id == id).ToList();
            return View(pd);
        }

        //更改产品信息
        public IActionResult MasterEditProduct(int? id)
        {
            var id2 = Mas.FirstOrDefault();
            ViewBag.Mas_id = id2.Id;
            pd = _context.ProductTable.Where(x => x.Id == id).ToList();
            return View(pd);
        }

        //更改产品信息
        [HttpPost]
        public IActionResult MasterEditProduct(int Pid, Models.Product pro)
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;
            var product = _context.ProductTable.Where(x => x.Id == Pid).FirstOrDefault();
            product.Name = pro.Name;
            if (pro.Price > pro.DelPrice)
            {
                TempData["error"] = "Wrong Price";
                return RedirectToAction("MasterProductInfo", new { id = Pid });
            }
            else if (pro.Quantity < 0)
            {
                TempData["error"] = "Wrong Quantity";
                return RedirectToAction("MasterProductInfo", new { id = Pid });
            }
            product.Price = pro.Price;
            product.DelPrice = pro.DelPrice;
            product.Quantity = pro.Quantity;
            _context.SaveChanges();

            TempData["success"] = "Edit Success";
            return RedirectToAction("MasterProductInfo", new { id = Pid });
        }

        //删除产品
        public IActionResult MasterDeleteProduct(int? id)
        {
            var id2 = Mas.FirstOrDefault();
            ViewBag.Mas_id = id2.Id;

            var vs = _context.ProductTable.Where(x => x.Id == id).FirstOrDefault();
            _context.ProductTable.Remove(vs);
            var video = _context.VideoTable.Where(x => x.Product_ID == id).FirstOrDefault();
            _context.VideoTable.Remove(video);
            _context.SaveChanges();
            TempData["success"] = "Delete Success";
            return RedirectToAction("MasterProduct");
        }

        //评论
        public IActionResult Com(int? id)
        {
            if (user.Count() == 0)
            {
                return RedirectToAction("LoginPage");
            }

            var master = _context.MasterTable.Where(x => x.Id == 1).FirstOrDefault();
            ViewBag.masimg = master.Img;
            ViewBag.masname = master.Mas;
            var u = user.FirstOrDefault();
            var u2 = _context.UserTable.Where(x => x.Id == u.Id).FirstOrDefault();
            ViewBag.uid = u2.Id;
            ViewBag.uname = u2.UserName;
            ViewBag.uhurl = u2.Hurl;
            var video = _context.VideoTable.Where(x => x.Product_ID == id).FirstOrDefault();
            ViewBag.src = video.Src;
            ViewBag.img = video.Img;
            ViewBag.good = video.Good;
            ViewBag.bad = video.Bad;
            var product = _context.ProductTable.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.pid = product.Id;
            ViewBag.Pname = product.Name;
            ViewBag.info = product.Info;
            com = _context.CommentTable.Where(x => x.Product_ID == id).ToList();
            ViewBag.count = com.Count();
            return View(com);
        }

        //加入评论
        [HttpPost]
        public IActionResult AddComment(int uid, int pid, string com)
        {
            var u = _context.UserTable.Where(x => x.Id == uid).FirstOrDefault();
            var product = _context.ProductTable.Where(x => x.Id == pid).FirstOrDefault();

            if (com == null)
            {
                TempData["error"] = "Can't enter empty value";
                return RedirectToAction("Com", new { id = pid });
            }

            _context.CommentTable.Add(new Comment()
            {
                User_Id = u.Id,
                UserName = u.UserName,
                Head = u.Hurl,
                Good = 0,
                Bad = 0,
                CreatedDate = DateTime.Now,
                Product_ID = product.Id,
                comment = com
            });
            _context.SaveChanges();

            return RedirectToAction("Com", new { id = pid });
        }

        //点赞Video
        public IActionResult AddGoodVideo(string gb, int id)
        {
            var video = _context.VideoTable.Where(x => x.Product_ID == id).FirstOrDefault();
            if (gb == "+")
            {
                video.Good += 1;

            }
            else if (gb == "-")
            {
                video.Bad += 1;
            }

            _context.SaveChanges();

            return RedirectToAction("Com", new { id = id });
        }

        //点赞客户
        public IActionResult AddComment(string gb, int id, int comid)
        {
            var com = _context.CommentTable.Where(x => x.Product_ID == id && x.Id == comid).FirstOrDefault();
            if (gb == "+")
            {
                com.Good += 1;

            }
            else if (gb == "-")
            {
                com.Bad += 1;
            }

            _context.SaveChanges();

            return RedirectToAction("Com", new { id = id });
        }

        //删除评论
        public IActionResult DelComment(int id, int comid)
        {
            var com = _context.CommentTable.Where(x => x.Id == comid && x.Product_ID == id).FirstOrDefault();
            _context.CommentTable.Remove(com);
            _context.SaveChanges();
            TempData["success"] = "Delete Success";
            return RedirectToAction("Com", new { id = id });
        }

        //添加界面
        public IActionResult AddPage()
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;
            return View();
        }

        //添加客户
        public IActionResult AddUser()
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;
            return View();
        }

        //添加客户
        [HttpPost]
        public IActionResult AddUser(User us)
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;


            bool b = int.TryParse(us.Password, out int num);
            if (b == true)
            {
                if (us.Password.Length < 6)
                {
                    TempData["error"] = "Password must be more than 6 digits";
                    return View();
                }

                var m1 = _context.UserTable.Where(x => x.Password == us.Password).FirstOrDefault();
                var m2 = _context.UserTable.Where(x => x.Email == us.Email).FirstOrDefault();
                if (m1 != null)
                {
                    TempData["error"] = "Please use another password";
                    return View();
                }
                if (m2 != null)
                {
                    TempData["error"] = "Please use another gmail";
                    return View();
                }

                _context.UserTable.Add(new User()
                {
                    UserName = us.UserName,
                    Email = us.Email,
                    Password = us.Password,
                    Gender = us.Gender,
                    Address = us.Address,
                    AddressZip = us.AddressZip,
                    CreateDate = DateTime.Now,
                    City = us.City,
                    Hurl = "himg.png",
                    Role = us.Role,
                    Money = 0
                });

                _context.SaveChanges();

            }
            else
            {
                TempData["error"] = "Password can only enter numbers";
                return View();
            }

            TempData["success"] = "Create Success";
            return RedirectToAction("AddPage");
        }

        //添加产品
        public IActionResult AddProduct()
        {
            var id = Mas.FirstOrDefault();
            ViewBag.Mas_id = id.Id;

            return View();
        }

        //添加产品
        [HttpPost]
        public async Task<IActionResult> AddProduct(IFormFile Img, Models.Product pro, string src)
        {
            //https://www.youtube.com/embed/ZAAALpCRmPY
            string s = null;
            if (src.Contains("https://www.youtube.com"))
            {
                s = src;
            }
            else if (src.ToLower() == "no")
            {
                s = src;
            }
            else
            {
                TempData["error"] = "Video Src Wrong";
                return RedirectToAction("AddProduct");
            }

            if (Img == null)
            {
                TempData["error"] = "Product Image No Data";
                return RedirectToAction("AddProduct");
            }
            else
            {
                var v = _context.UserTable.OrderByDescending(x => x.Id).FirstOrDefault();
                Random rnd = new Random();
                var num = rnd.Next(0, 10000);
                string filePath = $@"wwwroot/Img/Product/{v.Id + num + Img.FileName}";

                var newFilename = v.Id + num + Img.FileName;

                _context.SaveChanges();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await Img.CopyToAsync(stream);
                };

                if (pro.Price <= 0 || pro.DelPrice <= 0 || pro.Quantity <= 0)
                {
                    TempData["error"] = "Wrong";
                    return RedirectToAction("AddProduct");
                }

                if (pro.Price > pro.DelPrice)
                {
                    TempData["error"] = "Wrong Price";
                    return RedirectToAction("AddProduct");
                }

                var lastProductID = _context.ProductTable.OrderByDescending(x => x.Id).FirstOrDefault();

                string ProductID = null;
                if (lastProductID == null)
                {
                    ProductID = "P-001";
                }
                else
                {
                    string b = lastProductID.Product_ID;

                    var x = Convert.ToInt32(b.Substring(2)) + 1;
                    if (x < 10)
                    {
                        string xx = "P-000" + x.ToString();
                        ProductID = xx;
                    }
                    else if (x >= 10 && x < 100)
                    {
                        string xx = "P-00" + x.ToString();
                        ProductID = xx;
                    }
                    else if (x >= 100 && x < 1000)
                    {
                        string xx = "P-0" + x.ToString();
                        ProductID = xx;
                    }
                    else if (x >= 1000 && x < 10000)
                    {
                        string xx = "P-" + x.ToString();
                        ProductID = xx;
                    }
                    else if (x == 10000)
                    {
                        TempData["error"] = "Not Enough Space";
                        return View();
                    }
                }

                string p = null;
                if (pro.Category == "Phone")
                {
                    p = "Phonethree";
                }
                else if (pro.Category == "TV")
                {
                    p = "TVthree";
                }
                else if (pro.Category == "Earphone")
                {
                    p = "Ethree";
                }
                else if (pro.Category == "Bicycle")
                {
                    p = "Jone";
                }
                else if (pro.Category == "Computer")
                {
                    p = "Cthree";
                }
                else if (pro.Category == "Washer")
                {
                    p = "Hometwo";
                }
                else if (pro.Category == "Refrigerator")
                {
                    p = "Homethree";
                }
                else if (pro.Category == "Pizza")
                {
                    p = "P1";
                }
                else
                {
                    p = "Othree";
                }

                _context.ProductTable.Add(new Models.Product()
                {
                    Img = "Img/Product/" + newFilename,
                    Name = pro.Name,
                    Info = pro.Info,
                    Price = pro.Price,
                    DelPrice = pro.DelPrice,
                    Quantity = pro.Quantity,
                    Category = pro.Category,
                    Page = p,
                    Product_ID = ProductID,
                    P1 = 0,
                    P2 = 0,
                    P3 = 0,
                    P4 = 0
                });

                _context.SaveChanges();

                var pp = _context.ProductTable.OrderByDescending(x => x.Id).FirstOrDefault();
                _context.VideoTable.Add(new Video()
                {
                    Src = s,
                    Img = "Img/Product/" + newFilename,
                    Good = 0,
                    Bad = 0,
                    Product_ID = pp.Id
                });
                _context.SaveChanges();

                TempData["success"] = "Create Success";
                return RedirectToAction("AddPage");

            }
        }

        //客户个人信息
        public IActionResult Information(int? id)
        {
            user = _context.UserTable.Where(x => x.Id == id).ToList();
            return View(user);
        }

        //购买界面
        public IActionResult CardNumb(double Total, string CartList)
        {
            var u = user.FirstOrDefault();
            var newuser = _context.UserTable.Where(x => x.Id == u.Id).FirstOrDefault();
            Subtotal = Total;
            double amount = Subtotal * 1.06;
            if (newuser.Money <= amount)
            {
                TempData["error"] = "Insufficient Amount";
                return RedirectToAction("ShoppingCart");
            }

            var lastTr = _context.TrTable.OrderByDescending(x => x.Id).FirstOrDefault();

            string Tr_ID = null;
            if (lastTr == null)
            {
                Tr_ID = "T-1";
            }
            else
            {
                string b = lastTr.TransactionId;

                var x = Convert.ToInt32(b.Substring(2)) + 1;
                Tr_ID = "T-" + x;
            }

            var n = Convert.ToDouble(Total.ToString("0.00"));
            var n1 = Total.ToString("0.00");

            Tr t = new Tr()
            {
                PaidTime = "Not Yet Update",
                BillingAddress = "Not Yet Update",
                CartList = CartList,
                TotalPrice = Convert.ToDouble(Total.ToString("0.00")),
                Tax = Convert.ToDouble((Total * 0.06).ToString("0.00")),
                GrandTotal = Convert.ToDouble((Total * 1.06).ToString("0.00")),
                TransactionStatus = "Pending",
                TransactionId = $"T-{_context.TrTable.Count() + 1}",
                UserID = newuser.Id,
                UserName = newuser.UserName,
                Date = DateTime.Now
            };


            //保存id  计算
            CurrentTransactionId = t.TransactionId;

            _context.TrTable.Add(t);
            _context.SaveChanges();

            //观看
            ViewBag.total = (Total * 1.06).ToString("0.00");
            return View();
        }

        //购买
        [HttpPost]
        public IActionResult CardNumb(double Total, CreditCard data)
        {
            var tr = _context.TrTable.Where(x => x.TransactionId == CurrentTransactionId).FirstOrDefault();
            var u = user.FirstOrDefault();
            var newuser = _context.UserTable.Where(x => x.Id == u.Id).FirstOrDefault();

            try
            {
                //初始subtotal   不能有点数
                long amount = Convert.ToInt64((Subtotal * 1.06) * 100);

                //发送
                StripePayment stripePayment = new StripePayment(new CreditCard
                {
                    Name = data.Name,
                    Email = data.Email,
                    AddressLine1 = newuser.Address,
                    AddressLine2 = newuser.Address,
                    AddressCity = newuser.City,
                    AddressState = "True",
                    AddressZip = data.AddressZip,//地址邮编
                    Descripcion = $"Purchase on {DateTime.Now}",
                    DetailsDescripcion = $"test {DateTime.Now:d}",
                    Amount = amount, //2000 = 20.00   10000 == ??
                    Currency = "MYR",
                    Number = data.number,//4242 4242 4242 4242
                    ExpMonth = data.ExpMonth,
                    ExpYear = data.ExpYear,
                    Cvc = data.Cvc //123
                }, stripeSecrets);
                Charge charge = stripePayment.ProccessPayment();

                newuser.Money -= Convert.ToDouble((Subtotal * 1.06).ToString("0.00"));
                newuser.Money = Convert.ToDouble(newuser.Money.ToString("0.00"));

                //Cart  True
                var selectedCart = _context.CartTable.Where(x => x.UserID == newuser.Id && x.UserName == newuser.UserName).ToList();
                selectedCart.ForEach(x => x.Buy = true);
                selectedCart.ForEach(x => x.LastQuantity = 0);

                tr.PaidTime = DateTime.Now.ToString();
                tr.BillingAddress = newuser.Address;
                tr.TransactionStatus = "Paid";

                _context.SaveChanges();

                return RedirectToAction("SuccessBuy", "SuccessPage");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            _context.SaveChanges();
            ViewBag.total = Total.ToString("0.00");
            return View();
        }

        //返回cart
        public IActionResult OuttoCardNumb()
        {
            var tr = _context.TrTable.Where(x => x.TransactionId == CurrentTransactionId).FirstOrDefault();
            _context.TrTable.Remove(tr);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCart");
        }
        #endregion







    }
}
