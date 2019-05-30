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
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace ToDoApp503.Controllers
{
    [Authorize]
    public class MediaController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Media
        public async Task<ActionResult> Index()
        {
            return View(await db.Medias.ToListAsync());
        }

        // GET: Media/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Media media = await db.Medias.FindAsync(id);
            if (media == null)
            {
                return HttpNotFound();
            }
            return View(media);
        }

        // GET: Media/Create
        public ActionResult Create()
        {
            var media = new Media();
            return View(media);
        }

        // POST: Media/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,Description,Extension,FilePath,FileSize,Year,Month,ContentType,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Media media)
        {
            if (ModelState.IsValid)
            {
                media.CreateDate = DateTime.Now;
                media.CreatedBy = User.Identity.Name;
                media.UpdateDate = DateTime.Now;
                media.UpdatedBy = User.Identity.Name;

                //upload işlemleri
                if (!String.IsNullOrEmpty(media.FilePath))
                {
                    FileInfo fileInfo = new FileInfo(Server.MapPath("~" + media.FilePath));
                    media.FileSize = ((float)fileInfo.Length) / ((float)1024);
                    media.Extension = fileInfo.Extension;
                    media.ContentType = fileInfo.Extension;

                }
                db.Medias.Add(media);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(media);
        }

        // GET: Media/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Media media = await db.Medias.FindAsync(id);
            if (media == null)
            {
                return HttpNotFound();
            }
            return View(media);
        }

        // POST: Media/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Extension,FilePath,FileSize,Year,Month,ContentType,CreateDate,CreatedBy,UpdateDate,UpdatedBy")] Media media)
        {
            if (ModelState.IsValid)
            {

                media.UpdateDate = DateTime.Now;
                media.UpdatedBy = User.Identity.Name;

                //upload işlemleri
                if (!String.IsNullOrEmpty(media.FilePath))
                {
                    FileInfo fileInfo = new FileInfo(Server.MapPath("~" + media.FilePath));
                    media.FileSize = ((float)fileInfo.Length) / ((float)1024);
                    media.Extension = fileInfo.Extension;
                    media.ContentType = fileInfo.Extension;

                }

                db.Entry(media).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(media);
        }

        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            string categoryFolder = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        var uploadLocation = Server.MapPath("~/Uploads");
                        categoryFolder = "/" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "/";
                        fName = file.FileName;
                        var extension = Path.GetExtension(fName).ToLower();
                        var contentType = file.ContentType;
                        float fileSize = ((float)file.ContentLength) / ((float)1024);
                        if (!Directory.Exists(uploadLocation + categoryFolder))
                        {
                            Directory.CreateDirectory(uploadLocation + categoryFolder);
                        }
                        if (!System.IO.File.Exists(uploadLocation + categoryFolder + fName))
                        {
                            file.SaveAs(uploadLocation + categoryFolder + fName);
                        }
                        else
                        {
                            throw new Exception("Dosya zaten var.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = "/Uploads" + categoryFolder + fName, success = true }); // JavaScript Object Notation = JSON
            }
            else
            {
                return Json(new { Message = "Hata oluştu! Dosya kaydedilemedi.", success = false });
            }

        }

        // GET: Media/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Media media = await db.Medias.FindAsync(id);
            if (media == null)
            {
                return HttpNotFound();
            }
            return View(media);
        }

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Media media = await db.Medias.FindAsync(id);
            db.Medias.Remove(media);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public void ExportToExcel()
        {
            var grid = new GridView();
            grid.DataSource = (from data in db.Medias
                               select new { Ad = data.Name,Aciklama=data.Description,Uzanti=data.Extension,Dosya_Yolu=data.FilePath,Dosya_Boyutu=data.FileSize,Yil=data.Year,Ay=data.Month,Icerik_Tipi=data.ContentType, O_Tarihi = data.CreateDate, O_Kullanici = data.CreatedBy, G_Tarihi = data.UpdateDate, G_Kullanici = data.UpdatedBy }).ToList();
            grid.DataBind();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Medyalar.xls");
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
            sw.WriteLine("Adi,Aciklama,Uzanti,Dosya_Yolu,Dosya_Boyutu,Yil,Ay,Icerik_Tipi,O_Tarihi,O_Kullanici,G_Tarihi,G_Kullanici");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Medya.csv");
            Response.ContentType = "text/csv";
            var media = db.Medias;
            foreach (var Media in media)
            {
                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", Media.Name,Media.Description,Media.Extension,Media.FilePath,Media.FileSize,Media.Year,Media.Month,Media.ContentType, Media.CreateDate, Media.CreatedBy, Media.UpdateDate, Media.UpdatedBy));
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
