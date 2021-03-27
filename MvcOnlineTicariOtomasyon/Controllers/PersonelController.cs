using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class PersonelController : Controller
    {
        Context c = new Context();

        #region Personel Listesi Bölgesi

        public ActionResult Index()
        {
            var degerler = c.Personels.ToList();

            return View(degerler);
        }

        #endregion

        #region Personel Ekleme Bölgesi

        public ActionResult PersonelEkle()
        {
            //dropdown liste veri gönderme
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd, //ekranda görünecek kısım
                                               Value = x.Departmanid.ToString()
                                           }).ToList();

            //viewbag'a dropdownlist'i atadık
            ViewBag.dgr1 = deger1;

            return View();
        }

        [HttpPost]
        public ActionResult PersonelEkle(Personel p)
        {
            //resim yükleme için tanımlandı
            if (Request.Files.Count>0)
            {
                //dosya adını okuttuk
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                
                //dosyanın uzantısını aldık
                string uzanti = Path.GetExtension(Request.Files[0].FileName);

                string yol = "~/Image/"+dosyaadi+uzanti;

                //resmi klasörün içerisine attık
                Request.Files[0].SaveAs(Server.MapPath(yol));
                
                //veritabanına personel görseli eklendi
                p.PersonelGorsel = "/Image/" + dosyaadi + uzanti;
            }

            c.Personels.Add(p);
            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        #region Personel Güncelleme Bölgesi

        public ActionResult PersonelGetir(int id)
        {

            //dropdown liste veri gönderme
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd, //ekranda görünecek kısım
                                               Value = x.Departmanid.ToString()
                                           }).ToList();

            //viewbag'a dropdownlist'i atadık
            ViewBag.dgr1 = deger1;

            var prs = c.Personels.Find(id);

            return View("PersonelGetir",prs);
        }

        public ActionResult PersonelGuncelle(Personel p)
        {

            //resim yükleme için tanımlandı
            if (Request.Files.Count > 0)
            {
                //dosya adını okuttuk
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);

                //dosyanın uzantısını aldık
                string uzanti = Path.GetExtension(Request.Files[0].FileName);

                string yol = "~/Image/" + dosyaadi + uzanti;

                //resmi klasörün içerisine attık
                Request.Files[0].SaveAs(Server.MapPath(yol));

                //veritabanına personel görseli eklendi
                p.PersonelGorsel = "/Image/" + dosyaadi + uzanti;
            }

            var prsn = c.Personels.Find(p.Personelid);

            prsn.PersonelAd = p.PersonelAd;
            prsn.PersonelSoyad = p.PersonelSoyad;
            prsn.PersonelGorsel = p.PersonelGorsel;
            prsn.Departmanid = p.Departmanid;

            c.SaveChanges();

            return RedirectToAction("Index");


        }

        #endregion

        #region Personel Detay Bölgesi

        public ActionResult PersonelListe()
        {
            var sorgu = c.Personels.ToList();

            return View(sorgu);
        }

        #endregion

    }
}