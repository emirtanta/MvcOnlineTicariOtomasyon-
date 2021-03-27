using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class UrunController : Controller
    {
        Context c = new Context();

        #region Ürünler Listesi ve PDF/Excel Formatında İndirme Sayfası


        //p parametresi arama işlemi için kullanıldı
        public ActionResult Index(string p)
        {
            //var urunler = c.Uruns.Where(x => x.Durum == true).ToList();

            var urunler = from x in c.Uruns select x;
            
            //arama işlemi için tanımlandı
            if (!string.IsNullOrEmpty(p))
            {
                urunler = urunler.Where(y => y.UrunAd.Contains(p));
            }

            return View(urunler.ToList());
        }

        //PDF/Excel formatlarında indirme sayfası
        public ActionResult UrunListesi()
        {
            var degerler = c.Uruns.ToList();

            return View(degerler);
        }

        #endregion



        #region Ürün Ekleme Bölgesi

        [HttpGet]
        public ActionResult YeniUrun()
        {
            //dropdown liste veri gönderme
            List<SelectListItem> deger1=(from x in c.Kategoris.ToList() select new SelectListItem
            {
                Text=x.KategoriAd, //ekranda görünecek kısım
                Value=x.KategoriID.ToString()
            }).ToList();

            //viewbaga dropdownlist'i atadık
            ViewBag.dgr1 = deger1;

            return View();
        }

        [HttpPost]
        public ActionResult YeniUrun(Urun p)
        {
            c.Uruns.Add(p);
            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion


        #region Ürün Güncelleme Bölgesi

        public ActionResult UrunGetir(int id)
        {

            //dropdown liste veri gönderme
            List<SelectListItem> deger1 = (from x in c.Kategoris.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.KategoriAd, //ekranda görünecek kısım
                                               Value = x.KategoriID.ToString()
                                           }).ToList();

            //viewbaga dropdownlist'i atadık
            ViewBag.dgr1 = deger1;

            var urundeger = c.Uruns.Find(id);

            return View("UrunGetir", urundeger);
        }

        [HttpPost]
        public ActionResult UrunGuncelle(Urun p)
        {
            var urn = c.Uruns.Find(p.Urunid);

            urn.AlisFiyati = p.AlisFiyati;
            urn.Durum = p.Durum;
            urn.Kategoriid = p.Kategoriid;
            urn.Marka = p.Marka;
            urn.SatisFiyati = p.SatisFiyati;
            urn.Stok = p.Stok;
            urn.UrunAd = p.UrunAd;
            urn.UrunGorsel = p.UrunGorsel;

            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion


        #region Ürün Silme Bölgesi

        public ActionResult UrunSil(int id)
        {
            var deger = c.Uruns.Find(id);

            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion


        #region Üründen Satış Yap Bölgesi

        public ActionResult SatisYap(int id)
        {
            //personelin ad soyadını droprown ile getirme
            List<SelectListItem> deger3 = (from x in c.Personels.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.PersonelAd + " " + x.PersonelSoyad,
                                               Value = x.Personelid.ToString()
                                           }).ToList();

            ViewBag.dgr3 = deger3;

            var deger1 = c.Uruns.Find(id);
            ViewBag.dgr1 = deger1.Urunid;

            //toplam tutar değerini adet durumuna göre ekrana aktardık
            ViewBag.dgr2 = deger1.SatisFiyati;

            return View();
        }

        [HttpPost]
        public ActionResult SatisYap(SatisHareket p)
        {
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());

            c.SatisHarekets.Add(p);
            c.SaveChanges();

            return View("Index","Satis");
        }

        #endregion

    }
}