﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.Siniflar
{
    public class Personel
    {
        [Key]
        public int Personelid { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30)]
        [Display(Name ="Personel Adı")]
        public string PersonelAd { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30)]
        [Display(Name = "Personel Soyadı")]
        public string PersonelSoyad { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        [Display(Name = "Görsel")]
        public string PersonelGorsel { get; set; }

        public ICollection<SatisHareket> SatisHarekets { get; set; }

        public int Departmanid { get; set; }

        public virtual Departman Departman { get; set; }

    }
}