using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using kovtun.Models;
using kovtun.Data;
using OfficeOpenXml;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace kovtun.Controllers
{
    public class OperationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Operation
        public async Task<ActionResult> Index()
        {
            return View(await db.Operations.Include(o => o.Workplace).Include(o => o.Employee).ToListAsync());
        }

        // GET: Operation/Create
        public ActionResult Create()
        {
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "Id", "Name");
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        // POST: Operation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,WorkplaceId,EmployeeId")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                db.Operations.Add(operation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "Id", "Name", operation.WorkplaceId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", operation.EmployeeId);
            return View(operation);
        }

        // GET: Operation/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = await db.Operations.FindAsync(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "Id", "Name", operation.WorkplaceId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", operation.EmployeeId);
            return View(operation);
        }

        // POST: Operation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,WorkplaceId,EmployeeId")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(operation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "Id", "Name", operation.WorkplaceId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", operation.EmployeeId);
            return View(operation);
        }

        // GET: Operation/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = await db.Operations.FindAsync(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        // POST: Operation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Operation operation = await db.Operations.FindAsync(id);
            db.Operations.Remove(operation);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
