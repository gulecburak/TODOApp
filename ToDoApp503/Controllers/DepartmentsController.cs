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
    public class DepartmentsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Departments
        public async Task<ActionResult> Index()
        {
            return View(await db.Departments.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            var department = new Department();
            return View(department);
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DepartmentName,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.CreateDate = DateTime.Now;
                department.CreatedBy = User.Identity.Name;
                department.UpdateDate = DateTime.Now;
                department.UpdatedBy = User.Identity.Name;
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DepartmentName,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.UpdateDate = DateTime.Now;
                department.UpdatedBy = User.Identity.Name;
                db.Entry(department).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Department department = await db.Departments.FindAsync(id);
            db.Departments.Remove(department);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public void ExportToExcel()
        {
            var grid = new GridView();
            grid.DataSource = (from data in db.Departments
                               select new { Ad = data.DepartmentName, O_Tarihi = data.CreateDate, O_Kullanici = data.CreatedBy, G_Tarihi = data.UpdateDate, G_Kullanici = data.UpdatedBy }).ToList();
            grid.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Departmanlar.xls");
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
            sw.WriteLine("Departman_Adi,O_Tarihi,O_Kullanici,G_Tarihi,G_Kullanici");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Departman.csv");
            Response.ContentType = "text/csv";
            var department = db.Departments;
            foreach (var Department in department)
            {
                sw.WriteLine(string.Format("{0},{1},{2},{3},{4}", Department.DepartmentName, Department.CreateDate, Department.CreatedBy, Department.UpdateDate, Department.UpdatedBy));
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
