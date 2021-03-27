using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class CariPanelController : Controller
    {
        Context c = new Context();

        // GET: CariPanel
        [Authorize]
        public ActionResult Index()
        {
            //Session tanımladık
            var mail = (string)Session["CariMail"];

            var degerler = c.mesajlars.Where(x => x.Alici == mail).ToList();

            ViewBag.m = mail;

            //
            var mailid = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.Cariid).FirstOrDefault();
            ViewBag.mid = mailid;

            //toplamda kaç satış yapıldığını getirir
            var toplamsatis = c.SatisHarekets.Where(x => x.Cariid == mailid).Count();
            ViewBag.toplamsatis = toplamsatis;

            //carinin ödediği toplam tutarı getirir (decimal hatası verdi)
            //var toplamtutar = c.SatisHarekets.Where(x => x.Cariid == mailid).Sum(y => y.ToplamTutar);
            //ViewBag.toplamtutar = toplamtutar;

            //hata verdi int32
            //var toplamurunsayisi = c.SatisHarekets.Where(x => x.Cariid == mailid).Sum(y => y.Adet);
            //ViewBag.toplamurunsayisi = toplamurunsayisi;

            //carinin nad-soyad bilgilerini viewbag ile taşıdık
            var adsoyad = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.adsoyad = adsoyad;

            //carinin mail adresini taşıdık
            

            return View(degerler);
        }

        //Siparişlerim bölümü
        [Authorize]
        public ActionResult Siparislerim()
        {
            var mail = (string)Session["CariMail"];

            //sisteme giriş yapan carinin mail adresine göre id değerini getirerek siparişlerine ulaşmasını sağladık
            var id = c.Carilers.Where(x => x.CariMail == mail.ToString()).Select(y => y.Cariid).FirstOrDefault();

            //Satış haretekt tablosundaki cariye ait bilgileri getirmeyi sağladık
            var degerler = c.SatisHarekets.Where(x => x.Cariid == id).ToList();

            return View(degerler);
        }

        #region Cari Mesajlar kısmı için tanımlandı

        public ActionResult GelenMesajlar()
        {
            var mail = (string)Session["CariMail"]; //sisteme giriş yapan carilerin mail adresini tutar
            var mesajlar = c.mesajlars.Where(x=>x.Alici==mail).OrderByDescending(x=>x.MesajID).ToList();

            //gelen mesajları saymak için tanımladık
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;

            //giden mesajları saymak için tanımladık
            var gidensayisi = c.mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;

            return View(mesajlar);
        }

        public ActionResult GidenMesajlar()
        {
            var mail = (string)Session["CariMail"]; //sisteme giriş yapan carilerin mail adresini tutar
            var mesajlar = c.mesajlars.Where(x => x.Alici == mail).OrderByDescending(x => x.MesajID).ToList();

            //gelen mesajları saymak için tanımladık
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;

            //giden mesajları saymak için tanımladık
            var gidensayisi = c.mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;

            return View(mesajlar);
        }

        [Authorize]
        public ActionResult MesajDetay(int id)
        {

            var degerler = c.mesajlars.Where(x => x.MesajID == id).ToList();

            var mail = (string)Session["CariMail"]; //sisteme giriş yapan carilerin mail adresini tutar
            //gelen mesajları saymak için tanımladık
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;

            //giden mesajları saymak için tanımladık
            var gidensayisi = c.mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;

            return View(degerler);
        }

        [HttpGet]
        public ActionResult YeniMesaj()
        {
            var mail = (string)Session["CariMail"]; //sisteme giriş yapan carilerin mail adresini tutar
            //gelen mesajları saymak için tanımladık
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;

            //giden mesajları saymak için tanımladık
            var gidensayisi = c.mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;

            return View();
        }

        [HttpPost]
        public ActionResult YeniMesaj(mesajlar m)
        {
            var mail = (string)Session["CariMail"];
            m.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            m.Gonderici = mail;


            c.mesajlars.Add(m);
            c.SaveChanges();

            return View();
        }

        #endregion

        #region Kargo Takip ve Kargo Detay Bölgesi

        public ActionResult KargoTakip(string p)
        {
            var k = from x in c.KargoDetays select x;

            //kargo takip numarasına göre arama işlemi için tanımlandı
            k = k.Where(y => y.TakipKodu.Contains(p));

            return View(k.ToList());
        }

        //Kargo Detay sayfası
        public ActionResult CariKargoTakip(string id)
        {
            var degerler = c.KargoTakips.Where(x => x.TakipKodu == id).ToList();

            return View(degerler);
        }

        #endregion

        #region Çıkış Yap Bölgesi

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Abandon();

            return RedirectToAction("Index", "Login");
        }

        #endregion


        #region cari profil partial bölgesi

        public PartialViewResult Partial1()
        {

            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.Cariid).FirstOrDefault();

            var caribul = c.Carilers.Find(id);

            return PartialView("Partial1",caribul);
        }

        #endregion

        #region Duyurular bölgeis

        public PartialViewResult Partial2()
        {
            var veriler = c.mesajlars.Where(x => x.Gonderici == "admin").ToList();

            return PartialView(veriler);
        }

        #endregion

        #region Cari Profil Güncelleme Bölgesi

        public ActionResult CariBilgiGuncelle(Cariler cr)
        {
            var cari = c.Carilers.Find(cr.Cariid);
            cari.CariAd = cr.CariAd;
            cari.CariSoyad = cr.CariSoyad;
            cari.CariSifre = cr.CariSifre;

            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion





    }
}