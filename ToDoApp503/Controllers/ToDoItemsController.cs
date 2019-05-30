using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToDoApp503.Models;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ToDoApp503.Controllers

{
    [Authorize]
    public class ToDoItemsController : Controller
    {
       
        private AppDbContext db = new AppDbContext();
        // GET: ToDoItems
        public async Task<ActionResult> Index()
        {
            var toDoItems = db.ToDoItems.Include(z => z.Category).Include(x => x.Customer).Include(x => x.Department).Include(x => x.Side).Include(x => x.Manager).Include(x => x.Organizator);

            return View(await toDoItems.ToListAsync());
        }


        public ActionResult Create()
        {
            var todoitem = new ToDoItem();
            todoitem.MeetingDate = DateTime.Now;
            todoitem.FinishDate = DateTime.Now;
            todoitem.PlannedDate = DateTime.Now;
            todoitem.ReviseDate = DateTime.Now;
            todoitem.ScheduleOrganizationDate = DateTime.Now;

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName");
            ViewBag.ManagerId = new SelectList(db.Contacts, "Id", "FirstName");
            ViewBag.OrganizatorId = new SelectList(db.Contacts, "Id", "FirstName");
            ViewBag.SideId = new SelectList(db.Sides, "Id", "Name");
            return View(todoitem);

        }

        [HttpPost]
        public async Task<ActionResult> Create(ToDoItem todoitem)
        {
            if (ModelState.IsValid)
            {
                todoitem.CreateDate = DateTime.Now;
                todoitem.CreatedBy = User.Identity.Name;
                todoitem.UpdateDate = DateTime.Now;
                todoitem.UpdatedBy = User.Identity.Name;
                db.ToDoItems.Add(todoitem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", todoitem.CategoryId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", todoitem.CustomerId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", todoitem.DepartmentId);
            ViewBag.ManagerId = new SelectList(db.Contacts, "Id", "FirstName", todoitem.ManagerId);
            ViewBag.OrganizatorId = new SelectList(db.Contacts, "Id", "FirstName", todoitem.OrganizatorId);
            ViewBag.SideId = new SelectList(db.Sides, "Id", "Name", todoitem.SideId);

            return View(todoitem);

        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem todoitem = await db.ToDoItems.FindAsync(id);

            if (todoitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", todoitem.CategoryId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", todoitem.CustomerId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", todoitem.DepartmentId);
            ViewBag.ManagerId = new SelectList(db.Contacts, "Id", "FirstName", todoitem.ManagerId);
            ViewBag.OrganizatorId = new SelectList(db.Contacts, "Id", "FirstName", todoitem.OrganizatorId);
            ViewBag.SideId = new SelectList(db.Sides, "Id", "Name", todoitem.SideId);

            return View(todoitem);

        }

        [HttpPost]
        public async Task<ActionResult> Edit(ToDoItem todoitem)
        {
            if (ModelState.IsValid)
            {


                todoitem.UpdateDate = DateTime.Now;
                todoitem.UpdatedBy = User.Identity.Name;

                db.Entry(todoitem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", todoitem.CategoryId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", todoitem.CustomerId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "DepartmentName", todoitem.DepartmentId);
            ViewBag.ManagerId = new SelectList(db.Contacts, "Id", "FirstName", todoitem.ManagerId);
            ViewBag.OrganizatorId = new SelectList(db.Contacts, "Id", "FirstName", todoitem.OrganizatorId);
            ViewBag.SideId = new SelectList(db.Sides, "Id", "Name", todoitem.SideId);

            return View(todoitem);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem todoitem = await db.ToDoItems.FindAsync(id);
            if (todoitem==null)
            {
                return HttpNotFound();
            }

            return View(todoitem);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem todoitem = await db.ToDoItems.FindAsync(id);
            if (todoitem == null)
            {
                return HttpNotFound();
            }

            return View(todoitem);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ToDoItem todoitem = await db.ToDoItems.FindAsync(id);
            db.ToDoItems.Remove(todoitem);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public void ExportToExcel()
        {
            var grid = new GridView();
            grid.DataSource = (from data in db.ToDoItems
                               select new { Baslik = data.Title,Tanim=data.Description ,Toplanti_Tarihi=data.MeetingDate,Bitis_Tarihi=data.FinishDate,O_Tarihi = data.CreateDate, O_Kullanici = data.CreatedBy, G_Tarihi = data.UpdateDate, G_Kullanici = data.UpdatedBy }).ToList();
            grid.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Yapılacaklar.xls");
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
            sw.WriteLine("Baslik,Tanim,Toplanti_Tarihi,Bitis_Tarihi,O_Tarihi,O_Kullanici,G_Tarihi,G_Kullanici");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Yapılacaklar.csv");
            Response.ContentType = "text/csv";
            var todoitem = db.ToDoItems;
            foreach (var ToDoItem in todoitem)
            {
                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", ToDoItem.Title,ToDoItem.Description,ToDoItem.MeetingDate,ToDoItem.FinishDate ,ToDoItem.CreateDate, ToDoItem.CreatedBy, ToDoItem.UpdateDate, ToDoItem.UpdatedBy));
            }
            Response.Write(sw.ToString());
            Response.End();


        }
    }
}