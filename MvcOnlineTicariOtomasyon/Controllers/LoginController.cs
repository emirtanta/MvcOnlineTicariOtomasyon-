using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    [AllowAnonymous] //aşağıdaki alanalar global assax tarafında tanımlanan Authorize'nin dışında kalarak erişime açıktır
    public class LoginController : Controller
    {


        Context c = new Context();

        #region Login Bölgesi

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Register Cari Bölgesi

        //yeni cari kaydı bölümü için
        [HttpGet]
        public PartialViewResult Partial1()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult Partial1(Cariler p)
        {
            c.Carilers.Add(p);

            c.SaveChanges();

            return PartialView();
        }

        #endregion


        #region Cari Giriş Formu Giriş Bölgesi

        [HttpGet]
        public ActionResult CariLogin1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CariLogin1(Cariler p)
        {
            var bilgiler = c.Carilers.FirstOrDefault(x => x.CariMail == p.CariMail && x.CariSifre == p.CariSifre);

            if (bilgiler!=null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.CariMail, false);

                Session["CariMail"] = bilgiler.CariMail.ToString();

                return RedirectToAction("Index", "CariPanel");
            }

            else
            {
                return RedirectToAction("Index","Login");
            }

           
        }

        #endregion


        #region Personel(Admin) Login Bölgesi


        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(Admin p)
        {
            var bilgiler = c.Admins.FirstOrDefault(x => x.KullaniciAd == p.KullaniciAd && x.Sifre == p.Sifre);

            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.KullaniciAd, false);

                Session["KullaniciAd"] = bilgiler.KullaniciAd.ToString();

                return RedirectToAction("Index", "Kategori");
            }

            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        #endregion





    }
}