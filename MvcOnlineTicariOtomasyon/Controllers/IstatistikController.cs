using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class IstatistikController : Controller
    {
        Context c = new Context();

        // GET: Istatistik
        public ActionResult Index()
        {
            //toplam cari sayısı
            var deger1 = c.Carilers.Count().ToString();
            ViewBag.d1 = deger1;

            //toplam ürün sayısı
            var deger2 = c.Uruns.Count().ToString();
            ViewBag.d2 = deger2;

            //toplam personel sayısı
            var deger3 = c.Personels.Count().ToString();
            ViewBag.d3 = deger3;

            //tüm kategori sayısı
            var deger4 = c.Kategoris.Count().ToString();
            ViewBag.d4 = deger4;

            //toplam stok sayısı
            var deger5 = c.Uruns.Sum(x=>x.Stok).ToString();
            ViewBag.d5 = deger5;

            //tekrarsız marka sayısı
            var deger6 = (from x in c.Uruns select x.Marka).Distinct().Count().ToString();
            ViewBag.d6 = deger6;

            //kritik seviyedeki stok sayısını getirme
            var deger7 = c.Uruns.Count(x=>x.Stok<=20).ToString();
            ViewBag.d7 = deger7;

            //en yüksek fiyatlı ürünün adını getir
            var deger8 = (from x in c.Uruns orderby x.SatisFiyati descending select x.UrunAd).FirstOrDefault();
            ViewBag.d8 = deger8;

            //en az fiyatlı ürünün adı getirme
            var deger9 = (from x in c.Uruns orderby x.SatisFiyati ascending select x.UrunAd).FirstOrDefault();
            ViewBag.d9 = deger9;

            //buzdolabı sayısı
            var deger10 = c.Uruns.Count(x => x.UrunAd =="Buzdolabi").ToString();
            ViewBag.d10 = deger10;

            //laptop sayısı buzdolabı sayısı sorgusu ile aynı olduğu için yazmaya gerek yok

            //en yüksek sayıdaki marka adını getir
            var deger12 = c.Uruns.Where(u => u.Urunid == (c.SatisHarekets.GroupBy(x => x.Urunid).OrderByDescending(z => z.Count()).Select(y => y.Key).FirstOrDefault())).Select(k=>k.UrunAd).FirstOrDefault();
            ViewBag.d12 = deger12;

            //en çok satılan ürünü getir
            var deger13 = c.SatisHarekets.GroupBy(x => x.Urunid).OrderByDescending(z => z.Count()).Select(y => y.Key).FirstOrDefault();
            ViewBag.d13 = deger13;

            //kasadaki tutar
            var deger14 = c.SatisHarekets.Sum(x => x.ToplamTutar).ToString();
            ViewBag.d14 = deger14;

            //bugünkü satış sayısı
            DateTime bugun = DateTime.Today;
            var deger15 = c.SatisHarekets.Count(x => x.Tarih ==bugun).ToString();
            ViewBag.d15 = deger15;

            //bugünkü kasa toplam tutarı (decimal?) tanımlanmasının sebebi toplam değer boş olduğunda hata vermemesi içindir
            var deger16 = c.SatisHarekets.Where(x => x.Tarih==bugun).Sum(y=>(decimal?)y.ToplamTutar).ToString();
            ViewBag.d16 = deger16;

            return View();
        }

        public ActionResult KolayTablolar()
        {
            //şehirlere göre müşteri sayısını getiren sorgu
            var sorgu = from x in c.Carilers
                        group x by x.CariSehir into g
                        select new SinifGrup
                        {
                            Sehir = g.Key,
                            Sayi = g.Count()
                        };

            return View(sorgu.ToList());
        }

        public PartialViewResult Partial1()
        {
            //persolenin ilgili departman içerisinde kaç kişi olduğunu gösteren sorgu
            var sorgu2 =from x in c.Personels
                        group x by x.Departman.DepartmanAd into g
                        select new SinifGrup2
                        {
                            Departman=g.Key,
                            Sayi=g.Count()
                        };

            return PartialView(sorgu2.ToList());
        }

        //carilerin listesini getirdik
        public PartialViewResult Partial2()
        {
            var sorgu = c.Carilers.ToList();
            return PartialView(sorgu);
        }

        //Urunleri listeleyen Partial
        public PartialViewResult Partial3()
        {
            var sorgu = c.Uruns.ToList();
            return PartialView(sorgu);
        }

        // markaların sayısını getiren Partial
        public PartialViewResult Partial4()
        {
            var sorgu = from x in c.Uruns
                         group x by x.Marka into g
                         select new SinifGrup3
                         {
                             Marka = g.Key,
                             Sayi = g.Count()
                         };

            return PartialView(sorgu.ToList());
        }
    }
}