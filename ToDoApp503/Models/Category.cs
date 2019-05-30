using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp503.Models
{
    public class Category : BaseEntity
    {
        [StringLength(200,ErrorMessage ="200 karakterden fazla giriş yaptınız.")]
        [Required(ErrorMessage ="Kategori Adı zorunludur.")]
        [DisplayName("Kategori Adı")]
        public string Name { get; set; }
        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }
}