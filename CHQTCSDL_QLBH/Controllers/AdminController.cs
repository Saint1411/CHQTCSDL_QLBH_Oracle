using CHQTCSDL_QLBH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class AdminController : Controller
    {
        Entities db = new Entities();
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("DangNhap");
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(QUANTRI admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.TAIKHOAN))
                    ModelState.AddModelError(string.Empty, "Tài khoản không được để trống");
                if (string.IsNullOrEmpty(admin.MATKHAU))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                string hashedPassword = null;
                ObjectParameter hashedPasswordParam = new ObjectParameter("p_HASHEDPASSWORD", typeof(string));
                var result = db.SP_MAHOAMATKHAU(admin.MATKHAU, hashedPasswordParam);
                if(result != 0)
                    hashedPassword = hashedPasswordParam.Value.ToString();
                var adminDB = db.QUANTRIs.SingleOrDefault(ad => ad.TAIKHOAN == admin.TAIKHOAN && ad.MATKHAU.Equals(hashedPassword));
                if (adminDB != null)
                {
                    Session["admin"] = adminDB;
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    TempData["Message"] = "Tài khoản hoặc mật khẩu không đúng";
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult DangXuat()
        {
            Session.Remove("admin");
            return RedirectToAction("DangNhap");
        }
    }
}