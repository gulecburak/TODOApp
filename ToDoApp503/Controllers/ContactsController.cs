using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoApp503.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace ToDoApp503.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            return View(await db.Contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            var contact = new Contact();
            return View(contact);
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Email,Phone,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreateDate = DateTime.Now;
                contact.CreatedBy = User.Identity.Name;
                contact.UpdateDate = DateTime.Now;
                contact.UpdatedBy = User.Identity.Name;
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Email,Phone,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.UpdateDate = DateTime.Now;
                contact.UpdatedBy = User.Identity.Name;
                db.Entry(contact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            db.Contacts.Remove(contact);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public void ExportToExcel()
        {
            var grid = new GridView();
            grid.DataSource = (from data in db.Contacts
                               select new { Ad = data.FirstName, Soyad = data.LastName, Eposta = data.Email, Telefon = data.Phone, O_Tarihi = data.CreateDate, O_Kullanici = data.CreatedBy, G_Tarihi = data.UpdateDate, G_Kullanici = data.UpdatedBy }).ToList();
            grid.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Kisiler.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grid.RenderControl(hw);
            Response.Write(sw.ToString());
            Response.End();

        }

        public void ExportToCsv()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("Ad,Soyad,Eposta,Telefon,O_Tarihi,O_Kullanici,G_Tarihi,G_Kullanici");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Kisi.csv");
            Response.ContentType = "text/csv";
            var contact = db.Contacts;
            foreach (var Contact in contact)
            {
                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", Contact.FirstName, Contact.LastName, Contact.Email, Contact.Phone, Contact.CreateDate, Contact.CreatedBy, Contact.UpdateDate, Contact.UpdatedBy));
            }
            Response.Write(sw.ToString());
            Response.End();


        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
