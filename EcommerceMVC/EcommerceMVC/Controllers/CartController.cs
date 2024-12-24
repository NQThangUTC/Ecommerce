using EcommerceMVC.Data;
using Microsoft.AspNetCore.Mvc;
using EcommerceMVC.Helpers;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;
        public CartController(Hshop2023Context context) 
        {
            db = context;
        }
        const string Cart_key = "MyCart";
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(Cart_key) ?? new List<CartItem>();
        
        public IActionResult Index()
        {
            return View(Cart);
        }
        
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p=>p.MaHH == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if(hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHH = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(Cart_key, gioHang);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p=>p.MaHH == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(Cart_key, gioHang);
            }
            return RedirectToAction("Index");
        }
    }
}
