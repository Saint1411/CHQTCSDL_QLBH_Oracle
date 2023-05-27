using CHQTCSDL_QLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class HoaDonController : Controller
    {
        Entities db = new Entities();
        public ActionResult DanhSachHoaDon()
        {
            var order = db.VIEW_DANHSACHHOADON.OrderByDescending(n => n.NGAY).ToList();
            int tong = (int)db.VIEW_DANHSACHHOADON.Sum(n => n.TONGTG);
            ViewBag.TongTriGia = tong;
            return View(order);
        }
        [HttpGet]
        public ActionResult ThemHoaDon()
        {
            
            ViewBag.KhachHang = new SelectList(db.KHACHHANGs.ToList(), "MAKH", "HOTEN");
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHoaDon(HOADON order)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int result = db.SP_THEMHOADON(order.MAKH, order.MANV);
                    if (result != 0)
                    {
                        TempData["Success"] = "Thêm hóa đơn thành công!";
                        ViewBag.KhachHang = new SelectList(db.KHACHHANGs.ToList(), "MAKH", "HOTEN");
                        ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN");
                        return View(order);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Thêm hóa đơn thất bại: " + ex.Message;
                }
            }
            TempData["Failed"] = "Thêm hóa đơn thất bại!";
            ViewBag.KhachHang = new SelectList(db.KHACHHANGs.ToList(), "MAKH", "HOTEN");
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN");
            return View(order);
        }

        [HttpGet]
        public ActionResult CapNhatHoaDon(string SOHD)
        {
            HOADON order = db.HOADONs.Find(SOHD);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhachHang = new SelectList(db.KHACHHANGs.ToList(), "MAKH", "HOTEN", order.MAKH);
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN", order.MANV);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatHoaDon(HOADON order)
        {
            try
            {
                int result = db.SP_CAPNHATHOADON(order.SOHD, order.NGAY, order.MAKH, order.MANV, order.TONGTG);
                if (result != 0)
                {
                    TempData["Message"] = "Cập nhật sản phẩm thành công";
                    ViewBag.KhachHang = new SelectList(db.KHACHHANGs.ToList(), "MAKH", "HOTEN", order.MAKH);
                    ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN", order.MANV);
                    return View(order);
                }
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Cập nhật hóa đơn thất bại: " + ex.Message;
            }
            TempData["Failed"] = "Cập nhật hóa đơn thất bại!";
            ViewBag.KhachHang = new SelectList(db.KHACHHANGs.ToList(), "MAKH", "HOTEN", order.MAKH);
            ViewBag.NhanVien = new SelectList(db.NHANVIENs.ToList(), "MANV", "HOTEN", order.MANV);
            return View(order);
        }

        [HttpGet]
        public ActionResult XoaHoaDon(string SOHD)
        {
            if (SOHD == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON order = db.HOADONs.Find(SOHD);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("XoaHoaDon")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoaHoaDon(string SOHD)
        {
            if (SOHD == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON order = db.HOADONs.Find(SOHD);
            if (order == null)
            {
                return HttpNotFound();
            }
            try
            {
                int result = db.SP_XOAHOADON(order.SOHD);
                if (result != 0)
                    return RedirectToAction("DanhSachHoaDon");
            }
            catch
            {
                TempData["Failed"] = "Xóa sản phẩm thất bại...vui lòng xóa chi tiết hóa đơn trước!";
            }
            return View(order);
        }
    }
}