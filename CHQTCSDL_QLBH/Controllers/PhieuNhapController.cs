using CHQTCSDL_QLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class PhieuNhapController : Controller
    {
        Entities db = new Entities();
        public ActionResult DanhSachPhieuNhap()
        {
            var coupon = db.VIEW_DANHSACHPHIEUNHAP.ToList();
            int tong = (int)db.VIEW_DANHSACHPHIEUNHAP.Sum(n => n.TONGTIENNHAP);
            ViewBag.TongTriGia = tong;
            return View(coupon);
        }

        [HttpGet]
        public ActionResult ThemPhieuNhap()
        {

            ViewBag.NhaCungCap = new SelectList(db.NHACUNGCAPs.ToList(), "MANCC", "TENNCC");
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemPhieuNhap(PHIEUNHAP coupon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = db.SP_THEMPHIEUNHAP(coupon.MANCC, coupon.MANV);
                    if (result != 0)
                        return RedirectToAction("DanhSachPhieuNhap");
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Thêm phiếu nhập thất bại: " + ex.Message;
                }
            }
            TempData["Failed"] = "Thêm phiếu nhập thất bại!";
            ViewBag.NhaCungCap = new SelectList(db.NHACUNGCAPs.ToList(), "MANCC", "TENNCC");
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN");
            return View(coupon);
        }

        [HttpGet]
        public ActionResult CapNhatPhieuNhap(string MAPN)
        {
            PHIEUNHAP coupon = db.PHIEUNHAPs.Find(MAPN);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            ViewBag.NhaCungCap = new SelectList(db.NHACUNGCAPs.ToList(), "MANCC", "TENNCC", coupon.MANCC);
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN", coupon.MANV);
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatPhieuNhap(PHIEUNHAP coupon)
        {
            try
            {
                int result = db.SP_CAPNHATPHIEUNHAP(coupon.MAPN, coupon.NGAYNHAP, coupon.MANCC, coupon.MANV);
                if (result != 0)
                {
                    TempData["Message"] = "Cập nhật phiếu nhập thành công";
                    ViewBag.NhaCungCap = new SelectList(db.NHACUNGCAPs.ToList(), "MANCC", "TENNCC", coupon.MANCC);
                    ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN", coupon.MANV);
                    return View(coupon);
                }
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Cập nhật phiếu nhập thất bại: " + ex.Message;
            }
            TempData["Failed"] = "Cập nhật phiếu nhập thất bại!";
            ViewBag.NhaCungCap = new SelectList(db.NHACUNGCAPs.ToList(), "MANCC", "TENNCC", coupon.MANCC);
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN", coupon.MANV);
            return View(coupon);
        }

        [HttpGet]
        public ActionResult XoaPhieuNhap(string MAPN)
        {
            if (MAPN == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUNHAP coupon = db.PHIEUNHAPs.Find(MAPN);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        [HttpPost, ActionName("XoaPhieuNhap")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoaPhieuNhap(string MAPN)
        {
            if (MAPN == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUNHAP coupon = db.PHIEUNHAPs.Find(MAPN);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            try
            {
                int result = db.SP_XOAPHIEUNHAP(coupon.MAPN);
                if (result != 0)
                    return RedirectToAction("DanhSachPhieuNhap");
            }
            catch
            {
                TempData["Failed"] = "Xóa sản phẩm thất bại: ...vui lòng xóa chi tiết phiếu nhập trước!";
            }
            return View(coupon);
        }
    }
}