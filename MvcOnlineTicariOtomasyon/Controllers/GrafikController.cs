using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class GrafikController : Controller
    {
        Context c = new Context();

        // manuel olarak grafik oluşturma sayfası
        public ActionResult Index()
        {
            return View();
        }


        //Controller içerisinde grafik oluşturma sayfası
        public ActionResult Index2()
        {
            var grafikciz = new Chart(600, 600);
            grafikciz.AddTitle("Kategori-Ürün Stok Sayısı").AddLegend("Stok").AddSeries("Değerler",xValue:new[] { "Mobilya","Ofis Eşyaları","Bilgisayar"},yValues:new[] { 85,66,98}).Write();


            return File(grafikciz.ToWebImage().GetBytes(),"image/jpeg");
        }

        //Grafiklere veritabanından veri çekme
        public ActionResult Index3()
        {
            ArrayList xvalue = new ArrayList(); //satır değerleri
            ArrayList yvalue = new ArrayList(); //sutun değerleri

            var sonuclar = c.Uruns.ToList();
            sonuclar.ToList().ForEach(x => xvalue.Add(x.UrunAd));

            sonuclar.ToList().ForEach(y => yvalue.Add(y.Stok));

            var grafik = new Chart(width: 800, height: 800).
                AddTitle("Stoklar").
                AddSeries(chartType: "Pie", name: "Stok", xValue: xvalue, yValues: yvalue);

            return File(grafik.ToWebImage().GetBytes(), "image/jpeg");
        }

        #region GoogleChart ile grafik çizme (statik şekliyle)

        public ActionResult Index4()
        {
            return View();
        }

        public ActionResult VisualizeUrunResult()
        {
            return Json(Urunlistesi(), JsonRequestBehavior.AllowGet);
        }

        public List<sinif1> Urunlistesi()
        {
            List<sinif1> snf = new List<sinif1>();
            snf.Add(new sinif1()
            {
                urunad = "Bilgisayar",
                stok = 120
            });
            snf.Add(new sinif1()
            {
                urunad = "Beyaz Eşya",
                stok = 150
            });
            snf.Add(new sinif1()
            {
                urunad = "Mobilya",
                stok = 700
            });
            snf.Add(new sinif1()
            {
                urunad = "Küçük Ev Aletleri",
                stok = 180
            });
            snf.Add(new sinif1()
            {
                urunad = "Mobil Cihazlar",
                stok = 90
            });

            return snf;
        }

        #endregion

        #region GoogleChart içerisine veritabanından veri çekerek grafik oluşturma


        

        public ActionResult VisualizeUrunResult2()
        {
            return Json(UrunListesi2(), JsonRequestBehavior.AllowGet);
        }

        public List<sinif2> UrunListesi2()
        {
            List<sinif2> snf = new List<sinif2>();

            using (var context=new Context())
            {
                snf = c.Uruns.Select(x => new sinif2
                {
                    urn = x.UrunAd,
                    stk = x.Stok
                }).ToList();
            }

            return snf;

        }

        //Column Grafik
        public ActionResult Index5()
        {
            return View();
        }

        //Pie Chart
        public ActionResult Index6()
        {
            return View();
        }

        //Line grafik
        public ActionResult Index7()
        {
            return View();
        }

        #endregion
    }
}