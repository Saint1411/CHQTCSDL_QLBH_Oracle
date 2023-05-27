using CHQTCSDL_QLBH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class ChiTietHoaDonController : Controller
    {
        Entities db = new Entities();
        public ActionResult DanhSachCTHD(string SOHD)
        {
            var orderDetail = db.CTHDs.Where(h => h.SOHD.Trim().Equals(SOHD.Trim())).ToList();
            ViewBag.HoaDon = SOHD;
            return View(orderDetail);
        }

        [HttpGet]
        public ActionResult ThemCTHD(string SOHD)
        {
            ViewBag.SanPham = new SelectList(db.SANPHAMs.Where(s => s.SLTON > 0).ToList(), "MASP", "TENSP");
            ViewBag.HoaDon = SOHD;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemCTHD(CTHD ct)
        {
            if (ModelState.IsValid)
            {
                var cthd = db.CTHDs.FirstOrDefault(k => k.MASP.Equals(ct.MASP) && k.SOHD.Equals(ct.SOHD));
                if (cthd != null)
                    ModelState.AddModelError(string.Empty, "Sản phẩm đã tồn tại trong hóa đơn!");
                try
                {
                    int result = db.SP_THEMCTHD(ct.SOHD, ct.MASP, ct.SOLUONG);
                    if (result != 0)
                    {
                        return RedirectToAction("DanhSachCTHD", new { SOHD = ct.SOHD });
                    }
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Thêm chi tiết hóa đơn thất bại: " + ex.Message;
                }
            }
            else
            {
                TempData["Failed"] = "Thêm chi tiết hóa đơn thất bại!";
            }
            ViewBag.SanPham = new SelectList(db.SANPHAMs.Where(s => s.SLTON > 0).ToList(), "MASP", "TENSP");
            ViewBag.HoaDon = ct.SOHD;
            return View(ct);
        }


        [HttpGet]
        public ActionResult CapNhatCTHD(string SOHD, string MASP)
        {
            CTHD ct = db.CTHDs.SingleOrDefault(c => c.SOHD == SOHD && c.MASP == MASP);
            if (ct == null)
            {
                return HttpNotFound();
            }
            ViewBag.SanPham = ct.MASP;
            ViewBag.HoaDon = SOHD;
            return View(ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatCTHD(CTHD ct)
        {
            if (ModelState.IsValid)
            {
                if (ct.SOLUONG <= 0)
                    ModelState.AddModelError(string.Empty, "Số lượng phải lớn hơn 0!");
                try
                {
                    int result = db.SP_CAPNHATCTHD(ct.SOHD, ct.MASP, ct.SOLUONG);
                    if (result != 0)
                    {
                        TempData["Message"] = "Cập nhật sản chi tiết hóa đơn thành công";
                        ViewBag.SanPham = ct.MASP;
                        ViewBag.HoaDon = ct.SOHD;
                        return View(ct);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Cập nhật chi tiết hóa đơn thất bại: " + ex.Message;
                }
            }
            TempData["Failed"] = "Cập nhật chi tiết hóa đơn thất bại!";
            ViewBag.SanPham = ct.MASP;
            ViewBag.HoaDon = ct.SOHD;
            return View(ct);
        }

        [HttpGet]
        public ActionResult XoaCTHD(string SOHD, string MASP)
        {
            if (MASP == null || SOHD == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD ct = db.CTHDs.SingleOrDefault(c => c.SOHD == SOHD && c.MASP == MASP);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }

        [HttpPost, ActionName("XoaCTHD")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string SOHD, string MASP)
        {
            if (MASP == null || SOHD == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD ct = db.CTHDs.SingleOrDefault(c => c.SOHD == SOHD && c.MASP == MASP);
            if (ct == null)
            {
                return HttpNotFound();
            }
            try
            {
                int result = db.SP_XOACTHD(ct.SOHD, ct.MASP);
                if (result != 0)
                    return RedirectToAction("DanhSachCTHD", new { SOHD = ct.SOHD });
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Xóa chi tiết hóa đơn thất bại: " + ex.Message;
            }
            return View(ct);
        }
    }
}