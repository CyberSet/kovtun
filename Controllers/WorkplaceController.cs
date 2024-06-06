using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using kovtun.Models;
using kovtun.Data;
using System.Linq;

namespace kovtun.Controllers
{
    public class WorkplaceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Workplace
        public async Task<ActionResult> Index()
        {
            return View(await db.Workplaces.ToListAsync());
        }

        // GET: Workplace/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Workplace/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Workplace workplace)
        {
            if (ModelState.IsValid)
            {
                db.Workplaces.Add(workplace);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(workplace);
        }

        // GET: Workplace/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workplace workplace = await db.Workplaces.FindAsync(id);
            if (workplace == null)
            {
                return HttpNotFound();
            }
            return View(workplace);
        }

        // POST: Workplace/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Workplace workplace)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workplace).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(workplace);
        }

        // GET: Workplace/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workplace workplace = await db.Workplaces.FindAsync(id);
            if (workplace == null)
            {
                return HttpNotFound();
            }

            // Проверка на наличие связанных операций
            var hasOperations = db.Operations.Any(o => o.WorkplaceId == id);
            if (hasOperations)
            {
                ModelState.AddModelError(string.Empty, "Невозможно удалить это рабочее место, так как оно связано с существующими операциями.");
            }

            return View(workplace);
        }

        // GET: Workplace/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Workplace workplace = await db.Workplaces.FindAsync(id);
            if (workplace == null)
            {
                return HttpNotFound();
            }
            return View(workplace);
        }

        // POST: Workplace/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Workplace workplace = await db.Workplaces.FindAsync(id);

            // Проверка на наличие связанных операций
            var hasOperations = db.Operations.Any(o => o.WorkplaceId == id);
            if (hasOperations)
            {
                ModelState.AddModelError(string.Empty, "Невозможно удалить это рабочее место, так как оно связано с существующими операциями.");
                return View(workplace);
            }

            db.Workplaces.Remove(workplace);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
