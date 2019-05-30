using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp503.Models
{
    public class Customer : BaseEntity
    {
        [StringLength(200, ErrorMessage = "200 karakterden fazla girdiniz.")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Ad")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "50 karakterden fazla girdiniz.")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Eposta")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "20 karakterden fazla girdiniz.")]
        [DisplayName("Telefon")]
        public string Phone { get; set; }

        [StringLength(20, ErrorMessage = "20 karakterden fazla girdiniz.")]
        [DisplayName("Faks")]
        public string Fax { get; set; }

        [StringLength(50, ErrorMessage = "20 karakterden fazla girdiniz.")]
        [DisplayName("Website")]
        public string Website { get; set; }

        [StringLength(450, ErrorMessage = "450 karakterden fazla girdiniz.")]
        [DisplayName("Adres")]
        public string Address { get; set; }

        public virtual ICollection<ToDoItem> ToDoItems { get; set; }

    }
}