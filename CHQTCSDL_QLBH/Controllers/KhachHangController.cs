using CHQTCSDL_QLBH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class KhachHangController : Controller
    {
        Entities db = new Entities();
        public ActionResult DanhSachKhachHang()
        {
            var cus = db.VIEW_DS_KHACHHANG.ToList();
            int doanhSo = (int)db.VIEW_DS_KHACHHANG.Sum(n => n.DOANHSO);
            ViewBag.TongDoanhSo = doanhSo;
            ViewBag.SoKhachHang = cus.Count;
            return View(cus);
        }

        [HttpGet]
        public ActionResult ThemKhachHang()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemKhachHang(KHACHHANG cus)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cus.HOTEN))
                    ModelState.AddModelError(string.Empty, "Tên khách hàng không được để trống!");
                if (string.IsNullOrEmpty(cus.SODT))
                    ModelState.AddModelError(string.Empty, "Số điện thoại không được để trống!");
                var khachHang = db.KHACHHANGs.FirstOrDefault(k => k.SODT.Equals(cus.SODT));
                if (khachHang != null)
                    ModelState.AddModelError(string.Empty, "Số điện thoại đã tồn tại!");
                try
                {
                    cus.MAKH = "";
                    cus.DOANHSO = 0;
                    cus.DIEMTICHLUY = 0;
                    db.KHACHHANGs.Add(cus);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachKhachHang");
                }
                catch
                {
                    TempData["Failed"] = "Thêm khách hàng thất bại...vui lòng nhập số điện thoại hợp lệ!";
                }
            }
            TempData["Failed"] = "Thêm khách hàng thất bại...vui lòng nhập số điện thoại hợp lệ!";
            return View(cus);
        }

        [HttpGet]
        public ActionResult CapNhatKhachHang(string MAKH)
        {

            if (MAKH == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KHACHHANG cus = db.KHACHHANGs.Where(n => n.MAKH == MAKH).FirstOrDefault();
            if (cus == null)
            {
                return HttpNotFound();
            }
            return View(cus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CapNhatKhachHang(KHACHHANG cus)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cus.HOTEN))
                    ModelState.AddModelError(string.Empty, "Tên khách hàng không được để trống!");
                if (string.IsNullOrEmpty(cus.SODT))
                    ModelState.AddModelError(string.Empty, "Số điện thoại không được để trống!");
                var khachHang = db.KHACHHANGs.FirstOrDefault(k => k.MAKH.Equals(cus.MAKH));
                try
                {
                    db.Entry(khachHang).CurrentValues.SetValues(cus);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachKhachHang");
                }
                catch
                {
                    TempData["Failed"] = "Cập nhật khách hàng thất bại...vui lòng nhập số điện thoại hợp lệ!";
                }
            }
            TempData["Failed"] = "Cập nhật khách hàng thất bại...vui lòng nhập số điện thoại hợp lệ!";
            return View(cus);
        }
        public ActionResult ChiTietKhachHang(string MAKH)
        {

            if (MAKH == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIEW_DS_KHACHHANG cus = db.VIEW_DS_KHACHHANG.Find(MAKH);
            if (cus == null)
            {
                return HttpNotFound();
            }
            return View(cus);
        }

        public ActionResult DanhSachNCC()
        {
            var cus = db.NHACUNGCAPs.ToList();
            ViewBag.SoKhachHang = cus.Count;
            return View(cus);
        }
        [HttpGet]
        public ActionResult ThemNCC()
        {
            var supp = db.NHACUNGCAPs.ToList();
            string mancc = "N" + (supp.Count + 1).ToString();
            var model = new NHACUNGCAP { MANCC = mancc };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNCC(NHACUNGCAP supplier)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(supplier.TENNCC))
                    ModelState.AddModelError(string.Empty, "Tên nhà cung cấp không được để trống!");
                if (string.IsNullOrEmpty(supplier.SDT))
                    ModelState.AddModelError(string.Empty, "Số điện thoại không được để trống!");
                if (string.IsNullOrEmpty(supplier.DIACHI))
                    ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống!");
                try
                {
                    var supp = db.NHACUNGCAPs.ToList();
                    string mancc = "N" + (supp.Count + 1).ToString();
                    supplier.MANCC = mancc;
                    db.NHACUNGCAPs.Add(supplier);
                    db.SaveChanges();
                    TempData["Success"] = "Thêm nhà cung cấp thành công!";
                    return RedirectToAction("DanhSachNCC");
                }
                catch
                {
                    TempData["Failed"] = "Thêm nhà cung cấp thất bại...!";
                }
            }
            TempData["Failed"] = "Thêm nhà cung cấp thất bại...!";
            return View(supplier);
        }
        [HttpGet]
        public ActionResult CapNhatNCC(string MANCC)
        {

            if (MANCC == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NHACUNGCAP cus = db.NHACUNGCAPs.Where(n => n.MANCC == MANCC).FirstOrDefault();
            if (cus == null)
            {
                return HttpNotFound();
            }
            var countries = new List<string> { "Việt Nam", "Nhật Bản", "Trung Quốc", "Vương Quốc Anh", "Đức", "Ý", "Nga", "Hàn Quốc", "Thái Lan" };
            ViewBag.Countries = countries;
            return View(cus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CapNhatNCC(NHACUNGCAP cus)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cus.TENNCC))
                    ModelState.AddModelError(string.Empty, "Tên khách hàng không được để trống!");
                if (string.IsNullOrEmpty(cus.SDT))
                    ModelState.AddModelError(string.Empty, "Số điện thoại không được để trống!");
                if (string.IsNullOrEmpty(cus.DIACHI))
                    ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống!");
                var nhacc = db.NHACUNGCAPs.FirstOrDefault(k => k.MANCC.Equals(cus.MANCC));
                try
                {
                    db.Entry(nhacc).CurrentValues.SetValues(cus);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachNCC");
                }
                catch
                {
                    ModelState.AddModelError("", "Có lỗi khi cập nhật nhà cung cấp.");
                }
            }
            TempData["Failed"] = "Cập nhật nhà cung cấp thành công!";
            var countries = new List<string> { "Việt Nam", "Nhật Bản", "Trung Quốc", "Vương Quốc Anh", "Đức", "Ý", "Nga", "Hàn Quốc", "Thái Lan" };
            ViewBag.Countries = countries;
            return View(cus);
        }

        [HttpGet]
        public ActionResult XoaNCC(string MANCC)
        {
            if (MANCC == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHACUNGCAP supp = db.NHACUNGCAPs.Find(MANCC);
            if (supp == null)
            {
                return HttpNotFound();
            }
            return View(supp);
        }

        [HttpPost, ActionName("XoaNCC")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoaNCC(string MANCC)
        {
            if (MANCC == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHACUNGCAP supp = db.NHACUNGCAPs.Find(MANCC);
            if (supp == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.NHACUNGCAPs.Remove(supp);
                db.SaveChanges();
                return RedirectToAction("DanhSachNCC");
            }
            catch
            {
                TempData["Failed"] = "Xóa nhà cung cấp thất bại!";
            }
            TempData["Failed"] = "Xóa nhà cung cấp thất bại!";
            return View(supp);
        }
    }
}