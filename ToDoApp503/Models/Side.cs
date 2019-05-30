using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp503.Models
{
    public class Side:BaseEntity
    {
        [StringLength(150)]
        [Required(ErrorMessage ="Bu alan zorunludur.")]
        [DisplayName("Taraf Adı")]
        public string Name { get; set; }

        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }
}