using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp503.Models
{
    public class Contact :BaseEntity
    {
        [StringLength(200,ErrorMessage ="200 karakterden fazla giriş yaptınız.")]
        [Required(ErrorMessage ="Bu alan zorunludur.")]
        [DisplayName("Ad")]
        public string FirstName { get; set; }

        [StringLength(200, ErrorMessage = "200 karakterden fazla giriş yaptınız.")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Soyad")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "50 karakterden fazla giriş yaptınız.")]
        [DisplayName("Eposta")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "20 karakterden fazla giriş yaptınız.")]
        [DisplayName("Telefon")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public virtual ICollection<ToDoItem> ToDoItems { get; set; }


    }
}