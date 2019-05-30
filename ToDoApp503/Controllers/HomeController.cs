using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoApp503.Models;

namespace ToDoApp503.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        AppDbContext db = new AppDbContext();
        public ActionResult Index()
        {
            
            ViewBag.CustomerCount = db.Customers.Count();
            ViewBag.StatusNewCount = db.ToDoItems.Count(x => x.Status == Status.New);
            ViewBag.StatusWaitingCount = db.ToDoItems.Count(x => x.Status == Status.Waiting);
            ViewBag.StatusCompletedCount = db.ToDoItems.Count(x => x.Status == Status.Completed);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var contact = new Contact();

            return View(contact);
        }
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreateDate = DateTime.Now;
                contact.CreatedBy = User.Identity.Name;
                contact.UpdateDate = DateTime.Now;
                contact.UpdatedBy = User.Identity.Name;
                db.Contacts.Add(contact);
                db.SaveChanges();
                Session["StatusMessage"] = "Kişi formu başarıyla kaydedildi.";
                return RedirectToAction("Index");
            }
            return View(contact);
        }
    }
}