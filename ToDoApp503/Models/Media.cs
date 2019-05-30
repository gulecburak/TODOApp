using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp503.Models
{
    public class Media : BaseEntity
    {

        [StringLength(200, ErrorMessage = "200 karakterden fazla girdiniz.")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Ad")]
        public string Name { get; set; }

       
        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [StringLength(200, ErrorMessage = "200 karakterden fazla girdiniz.")]
        [DisplayName("Uzantı")]
        public string Extension { get; set; }

        [StringLength(200, ErrorMessage = "200 karakterden fazla girdiniz.")]
        [DisplayName("Dosya Yolu")]
        public string FilePath { get; set; }

        [DisplayName("Dosya Boyutu")]
        public float FileSize { get; set; }

        [DisplayName("Yıl")]
        public int Year { get; set; }

        [DisplayName("Ay")]
        public int Month { get; set; }

        [StringLength(200, ErrorMessage = "200 karakterden fazla girdiniz.")]
        [DisplayName("İçerik Türü")]
        public string ContentType { get; set; }


    }
}