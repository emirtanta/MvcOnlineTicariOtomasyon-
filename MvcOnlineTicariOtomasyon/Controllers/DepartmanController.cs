using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar; 

namespace MvcOnlineTicariOtomasyon.Controllers
{
    //[Authorize] globalassac içerisinde Authorize tanımlandığı için çakışma olduğundan yorum satırı haline getirildi
    public class DepartmanController : Controller
    {
        Context c=new Context();

        #region Departman Listesi Bölgesi

       
        public ActionResult Index()
        {
            var degerler = c.Departmans.Where(x => x.Durum == true).ToList();

            return View(degerler);
        }

        #endregion


        #region Departman Ekle Bölgesi

        //[Authorize(Roles ="A")]
        [HttpGet]
        public ActionResult DepartmanEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DepartmanEkle(Departman d)
        {
            c.Departmans.Add(d);
            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        #region Departman Detay Bölgesi

        public ActionResult DepartmanDetay(int id)
        {
            //Personele ait departmanları listeleme
            var degerler = c.Personels.Where(x => x.Departmanid == id).ToList();

            //ViewBag ile departmanın adını taşıdık
            var dpt = c.Departmans.Where(x => x.Departmanid == id).Select(y => y.DepartmanAd).FirstOrDefault();

            ViewBag.d = dpt;

            return View(degerler);
        }

        //Personel Satış Sayfası
        public ActionResult DepartmanPersonelSatis(int id)
        {
            var degerler = c.SatisHarekets.Where(x => x.Personelid == id).ToList();

            //başlık kısmına personelin ad-soya bilgisini getirdik
            var per = c.Personels.Where(x => x.Personelid == id).Select(y => y.PersonelAd +" "+ y.PersonelSoyad).FirstOrDefault();

            ViewBag.dpers = per;

            return View(degerler);
        }

        #endregion



        #region Departman Düzenle Bölgesi

        public ActionResult DepartmanGetir(int id)
        {
            var dpt = c.Departmans.Find(id);

            return View("DepartmanGetir", dpt);
        }

        public ActionResult DepartmanGuncelle(Departman p)
        {
            var dept = c.Departmans.Find(p.Departmanid);
            dept.DepartmanAd = p.DepartmanAd;

            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion


        #region Departman Silme Bölgesi

        public ActionResult DepartmanSil(int id)
        {
            var dep = c.Departmans.Find(id);

            dep.Durum = false;

            c.SaveChanges();

            return RedirectToAction("Index");

        }

        #endregion
    }
}