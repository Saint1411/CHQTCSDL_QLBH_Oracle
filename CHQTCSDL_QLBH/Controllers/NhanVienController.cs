using CHQTCSDL_QLBH.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CHQTCSDL_QLBH.Controllers
{
    public class NhanVienController : Controller
    {
        Entities db = new Entities();
        public ActionResult DanhSachNhanVien()
        {
            var emp = db.NHANVIENs.ToList();
            ViewBag.SoNhanVien = emp.Count;
            return View(emp);
        }

        public ActionResult HoatDongNhanVien()
        {
            var emp = db.VIEW_HOATDONGCUANV.ToList();
            ViewBag.SoNhanVien = emp.Count;
            return View(emp);
        }

        [HttpGet]
        public ActionResult ThemNhanVien()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNhanVien(NHANVIEN emp)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(emp.MANV))
                    ModelState.AddModelError(string.Empty, "Mã nhân viên không được để trống!");
                if (string.IsNullOrEmpty(emp.HOTEN))
                    ModelState.AddModelError(string.Empty, "Tên nhân viên không được để trống!");
                if (string.IsNullOrEmpty(emp.SDT))
                    ModelState.AddModelError(string.Empty, "Số điện thoại không được để trống!");
                if (string.IsNullOrEmpty(emp.NGAYSINH?.ToString()))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền ngày sinh!");
                if (string.IsNullOrEmpty(emp.NGAYLV?.ToString()))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền ngày làm việc!");
                var nhanVien = db.NHANVIENs.FirstOrDefault(k => k.MANV.Equals(emp.MANV));
                if (nhanVien != null)
                    ModelState.AddModelError(string.Empty, "Mã nhân viên đã tồn tại!");
                try
                {
                    emp.LUONG = 0;
                    emp.NGAYSINH?.ToString("dd-MM-yy");
                    emp.NGAYLV?.ToString("dd-MM-yy");
                    db.NHANVIENs.Add(emp);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachNhanVien");
                }
                catch
                {
                    TempData["Failed"] = "Thêm nhân viên thất bại...vui lòng kiểm tra tuổi nhân viên phải lớn hơn 18!";
                }
            }
            TempData["Failed"] = "Thêm nhân viên thất bại...vui lòng kiểm tra tuổi nhân viên phải lớn hơn 18!";
            return View(emp);
        }

        [HttpGet]
        public ActionResult CapNhatNhanVien(string MANV)
        {

            if (MANV == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NHANVIEN emp = db.NHANVIENs.Where(n => n.MANV == MANV).FirstOrDefault();
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CapNhatNhanVien(NHANVIEN emp)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(emp.HOTEN))
                    ModelState.AddModelError(string.Empty, "Tên nhân viên không được để trống!");
                if (string.IsNullOrEmpty(emp.SDT))
                    ModelState.AddModelError(string.Empty, "Số điện thoại không được để trống!");
                if (string.IsNullOrEmpty(emp.NGAYSINH?.ToString()))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền ngày sinh!");
                if (string.IsNullOrEmpty(emp.NGAYLV?.ToString()))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền ngày làm việc!");
                var nhanVien = db.NHANVIENs.FirstOrDefault(k => k.MANV.Equals(emp.MANV));
                try
                {
                    db.Entry(nhanVien).CurrentValues.SetValues(emp);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachNhanVien");
                }
                catch
                {
                    TempData["Failed"] = "Cập nhật nhân viên thất bại...vui lòng kiểm tra tuổi nhân viên phải lớn hơn 18!";
                }
            }
            TempData["Failed"] = "Cập nhật nhân viên thất bại...vui lòng kiểm tra tuổi nhân viên phải lớn hơn 18!";
            return View(emp);
        }
        public ActionResult ChiTietNhanVien(string MANV)
        {

            if (MANV == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIEW_HOATDONGCUANV emp = db.VIEW_HOATDONGCUANV.Find(MANV);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }
    }
}