using CHQTCSDL_QLBH.Models;
using System;
using PagedList;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class HomeController : Controller
    {
        Entities db = new Entities();
        public ActionResult Home(int? page, string txtseatch)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            Entities db = new Entities();
            var pros = db.VIEW_DANHSACHSANPHAM.ToList();

            if (!string.IsNullOrEmpty(txtseatch))
            {
                pros = pros.Where(p => p.TENSP.ToLower().Contains(txtseatch.ToLower())).ToList();
            }

            return View(pros.ToPagedList(pageNum, pageSize));
        }


        public ActionResult ThongKeSanPham()
        {
            ViewBag.Message = "Thống kê lãi xuất";
            int tong = (int)db.VIEW_LAIXUATCUASANPHAM.Sum(n => n.LAIXUAT);
            ViewBag.TongLaiXuat = tong;
            var pro = db.VIEW_LAIXUATCUASANPHAM.ToList();
            return View(pro);
        }

        [HttpGet]
        public ActionResult ThemSanPham()
        {
            ViewBag.CategoryID = new SelectList(db.LOAISANPHAMs.ToList(), "MALOAI", "TENLOAI");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSanPham(SANPHAM pro)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(pro.TENSP))
                    ModelState.AddModelError(string.Empty, "Tên sản phẩm không được để trống!");
                if (pro.SLTON == 0)
                    ModelState.AddModelError(string.Empty, "Số lượng tồn phải khác 0!");
                if (string.IsNullOrEmpty(pro.DVT))
                    ModelState.AddModelError(string.Empty, "Đơn vị tính không được để trống!");
                if (pro.DONGIA == 0)
                    ModelState.AddModelError(string.Empty, "Đơn giá phải khác 0!");
                try
                {
                    int result = db.SP_THEMSANPHAM(pro.TENSP, pro.SLTON, pro.DVT, pro.DONGIA, pro.LOAISP);
                    if (result != 0)
                    {
                        return RedirectToAction("Home");
                    }
                    else
                    {
                        TempData["Failed"] = "Thêm sản phẩm thất bại";
                        return View(pro);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Thêm sản phẩm thất bại: " + ex.Message;
                }
            }
            TempData["Failed"] = "Thêm sản phẩm thất bại: ";
            ViewBag.CategoryID = new SelectList(db.LOAISANPHAMs, "MALOAI", "TENLOAI");
            return View(pro);
        }
        
        [HttpGet]
        public ActionResult CapNhatSanPham(string MASP)
        {
            SANPHAM pro = db.SANPHAMs.Find(MASP);
            if (pro == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.LOAISANPHAMs.ToList(), "MALOAI", "TENLOAI", pro.LOAISP);
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatSanPham(SANPHAM pro)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(pro.TENSP))
                    ModelState.AddModelError(string.Empty, "Tên sản phẩm không được để trống!");
                if ((int)pro.SLTON < 0)
                    ModelState.AddModelError(string.Empty, "Số lượng tồn phải lớn hơn 0!");
                if (string.IsNullOrEmpty(pro.DVT))
                    ModelState.AddModelError(string.Empty, "Đơn vị tính không được để trống!");
                try
                {
                    int result = db.SP_CAPNHATSANPHAM(pro.MASP, pro.TENSP, pro.SLTON, pro.DVT, pro.DONGIA, pro.LOAISP);
                    if (result != 0)
                    {
                        TempData["Message"] = "Cập nhật sản phẩm thành công";
                        ViewBag.CategoryID = new SelectList(db.LOAISANPHAMs, "MALOAI", "TENLOAI");
                        return View(pro);
                    }
                    else
                    {
                        TempData["Failed"] = "Cập nhật sản phẩm thất bại";
                        return View(pro);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Cập nhật sản phẩm thất bại: " + ex.Message;
                }
            }
            TempData["Failed"] = "Cập nhật sản phẩm thất bại: ";
            ViewBag.CategoryID = new SelectList(db.LOAISANPHAMs, "MALOAI", "TENLOAI");
            return View(pro);
        }

        [HttpGet]
        public ActionResult Delete(string MASP)
        {
            if (MASP == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM pro = db.SANPHAMs.Find(MASP);
            if (pro == null)
            {
                return HttpNotFound();
            }
            return View(pro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string MASP)
        {
            if (MASP == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM pro = db.SANPHAMs.Find(MASP);
            if (pro == null)
            {
                return HttpNotFound();
            }
            try
            {
                int result = db.SP_XOASANPHAM(pro.MASP);
                if (result != 0)
                    return RedirectToAction("Home");
            }
            catch (Exception ex)
            {
                TempData["Failed"] = "Xóa sản phẩm thất bại: " + ex.Message;
            }
            return View(pro);
        }
    }
}