using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class KargoController : Controller
    {
        Context c = new Context();

        // GET: Kargo
        public ActionResult Index(string p)
        {
            var k = from x in c.KargoDetays select x;

            //kargo takip numarasına gçre arama işlemi için tanımlandı
            if (!string.IsNullOrEmpty(p))
            {
                k = k.Where(y => y.TakipKodu.Contains(p));
            }

            return View(k.ToList());
        }

        #region Kargo Ekle Bölgesi

        [HttpGet]
        public ActionResult YeniKargo()
        {

            #region Rastgele TakiKodu oluşturuldu

            Random rnd = new Random();

            string[] karakterler = { "A", "B", "C", "D" };
            int k1, k2, k3;
            k1 = rnd.Next(0, 4);
            k2 = rnd.Next(0, 4);
            k3 = rnd.Next(0, 4);

            int s1, s2, s3;
            s1 = rnd.Next(100, 1000);
            s2 = rnd.Next(10, 99);
            s3 = rnd.Next(10, 99);

            string kod = s1.ToString() + karakterler[k1] + s2 + karakterler[k2] + s3 + karakterler[k3];

            ViewBag.takipkod = kod;

            #endregion


            return View();
        }

        [HttpPost]
        public ActionResult YeniKargo(KargoDetay d)
        {
            c.KargoDetays.Add(d);
            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        #region Kargo Takip koduna kargo durumunu getirme

        public ActionResult KargoTakip(string id)
        {

            var degerler = c.KargoTakips.Where(x => x.TakipKodu == id).ToList();

            return View(degerler);
        }

        #endregion
    }
}