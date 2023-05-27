using CHQTCSDL_QLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class ChiTietPhieuNhapController : Controller
    {
        Entities db = new Entities();
        public ActionResult DanhSachCTPN(string MAPN)
        {
            var receipt = db.CTPNs.Where(h => h.MAPN.Trim().Equals(MAPN.Trim())).ToList();
            ViewBag.PhieuNhap = MAPN;
            return View(receipt);
        }

        [HttpGet]
        public ActionResult ThemCTPN(string MAPN)
        {
            ViewBag.SanPham = new SelectList(db.SANPHAMs.Where(s=>s.SLTON > 0).ToList(), "MASP", "TENSP");
            ViewBag.PhieuNhap = MAPN;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemCTPN(CTPN ct)
        {
            if (ModelState.IsValid)
            {
                var ctpn = db.CTPNs.FirstOrDefault(k => k.MASP.Equals(ct.MASP) && k.MAPN.Equals(ct.MAPN));
                if (ctpn != null)
                    ModelState.AddModelError(string.Empty, "Sản phẩm đã tồn tại trong phiếu nhập!");
                try
                {
                    int result = db.SP_THEMCTPN(ct.MAPN, ct.MASP, ct.SLNHAP, ct.DONGIANHAP);
                    if (result != 0)
                    {
                        return RedirectToAction("DanhSachCTPN", new { MAPN = ct.MAPN });
                    }
                }
                catch
                {
                    TempData["Failed"] = "Thêm chi tiết phiếu nhập thất bại!";
                }
            }
            else
            {
                TempData["Failed"] = "Thêm chi tiết phiếu nhập thất bại!";
            }
            ViewBag.SanPham = new SelectList(db.SANPHAMs.Where(s => s.SLTON > 0).ToList(), "MASP", "TENSP");
            ViewBag.PhieuNhap = ct.MAPN;
            return View(ct);
        }


        [HttpGet]
        public ActionResult CapNhatCTPN(string MAPN, string MASP)
        {
            CTPN ct = db.CTPNs.SingleOrDefault(c => c.MAPN == MAPN && c.MASP == MASP);
            if (ct == null)
            {
                return HttpNotFound();
            }
            ViewBag.SanPham = ct.MASP;
            ViewBag.PhieuNhap = MAPN;
            return View(ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatCTPN(CTPN ct)
        {
            if (ModelState.IsValid)
            {
                if (ct.SLNHAP <= 0)
                    ModelState.AddModelError(string.Empty, "Số lượng phải lớn hơn 0!");
                try
                {
                    int result = db.SP_CAPNHATCTPN(ct.MAPN, ct.MASP, ct.SLNHAP);
                    if (result != 0)
                    {
                        TempData["Message"] = "Cập nhật sản chi tiết phiếu nhập thành công";
                        ViewBag.SanPham = ct.MASP;
                        ViewBag.PhieuNhap = ct.MAPN;
                        return View(ct);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Cập nhật chi tiết phiếu nhập thất bại: " + ex.Message;
                }
            }
            TempData["Failed"] = "Cập nhật chi tiết phiếu nhập thất bại!";
            ViewBag.SanPham = ct.MASP;
            ViewBag.PhieuNhap = ct.MAPN;
            return View(ct);
        }

        [HttpGet]
        public ActionResult XoaCTPN(string MAPN, string MASP)
        {
            if (MASP == null || MAPN == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTPN ct = db.CTPNs.SingleOrDefault(c => c.MAPN == MAPN && c.MASP == MASP);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }

        [HttpPost, ActionName("XoaCTPN")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string MAPN, string MASP)
        {
            if (MASP == null || MAPN == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTPN ct = db.CTPNs.SingleOrDefault(c => c.MAPN == MAPN && c.MASP == MASP);
            if (ct == null)
            {
                return HttpNotFound();
            }
            try
            {
                int result = db.SP_XOACTPN(ct.MAPN, ct.MASP);
                if (result != 0)
                    return RedirectToAction("DanhSachCTPN", new { MAPN = ct.MAPN });
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Xóa chi tiết phiếu nhập thất bại: " + ex.Message;
            }
            return View(ct);
        }
    }
}