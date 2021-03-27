﻿using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class FaturaController : Controller
    {
        Context c = new Context();

        // GET: Fatura
        public ActionResult Index()
        {
            var liste = c.Faturalars.ToList();

            return View(liste);
        }

        #region Fatura Ekleme Bölgesi

        public ActionResult FaturaEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FaturaEkle(Faturalar f)
        {
            c.Faturalars.Add(f);

            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        #region Fatura Düzenleme Bölgesi

        public ActionResult FaturaGetir(int id)
        {
            var fatura = c.Faturalars.Find(id);

            return View("FaturaGetir", fatura);
        }

        public ActionResult FaturaGuncelle(Faturalar f)
        {
            var fatura = c.Faturalars.Find(f.Faturaid);

            fatura.FaturaSeriNo = f.FaturaSeriNo;
            fatura.FaturaSiraNo = f.FaturaSiraNo;
            fatura.Tarih = f.Tarih;
            fatura.Saat = f.Saat;
            fatura.VergiDairesi = f.VergiDairesi;
            fatura.TeslimEden = f.TeslimEden;
            fatura.TeslimAlan = f.TeslimAlan;

            c.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        #region Fatura Detay Bölgesi

        //faturaya ait değerleri getirme
        public ActionResult FaturaDetay(int id)
        {
            var degerler = c.FaturaKalems.Where(x => x.Faturaid == id).ToList();

            return View(degerler);
        }

        #endregion

        #region FaturaDetay sayfasındaki yeni kalem girşi butonu için çalışma sayfası

        [HttpGet]
        public ActionResult YeniKalem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YeniKalem(FaturaKalem p)
        {
            c.FaturaKalems.Add(p);
            c.SaveChanges();

            return RedirectToAction("Index");
        }


        #endregion

        #region Dinamik Fatura Bölgesi

        public ActionResult Dinamik()
        {
            //bir sayfada birden fazla model çekmek için tanımladık

            Class4 cs = new Class4();
            cs.deger1 = c.Faturalars.ToList();
            cs.deger2 = c.FaturaKalems.ToList();

            return View(cs);
        }

        public ActionResult FaturaKaydet(string FaturaSeriNo,string FaturaSiraNo,DateTime Tarih,string VergiDairesi,string Saat,string TeslimEden,string TeslimAlan,string Toplam,FaturaKalem[] kalemler)
        {
            Faturalar f = new Faturalar();
            f.FaturaSeriNo = FaturaSeriNo;
            f.FaturaSiraNo = FaturaSiraNo;
            f.Tarih = Tarih;
            f.VergiDairesi = VergiDairesi;
            f.Saat = Saat;
            f.TeslimEden = TeslimEden;
            f.TeslimAlan = TeslimAlan;
            f.Toplam = decimal.Parse(Toplam);

            c.Faturalars.Add(f);

            foreach (var x in kalemler)
            {
                FaturaKalem fk = new FaturaKalem();
                fk.Aciklama = x.Aciklama;
                fk.BirimFiyat = x.BirimFiyat;
                fk.Faturaid = x.FaturaKalemid;
                fk.Miktar = x.Miktar;
                fk.Tutar = x.Tutar;

                c.FaturaKalems.Add(fk);
            }


            c.SaveChanges();

            return Json("İşlem Başarılı", JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}