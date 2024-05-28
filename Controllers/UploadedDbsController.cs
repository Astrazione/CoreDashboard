using CoreDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreDashboard.Controllers
{
    public class UploadedDbsController : Controller
    {
        private readonly ApplicationContext _context;

        public UploadedDbsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: UploadedDbs
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.UploadedDbs
                .Include(u => u.Discipline)
                .Include(u => u.User)
                .ThenInclude(u => u!.UserType);
            return View(await applicationContext.ToListAsync());
        }

        // GET: UploadedDbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var uploadedDb = await _context.UploadedDbs.FindAsync(id);

            if (uploadedDb == null)
                return NotFound();

            return View(uploadedDb);
        }

        // POST: UploadedDbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("uploadedDbName")] string uploadedDbName)
        {
            var uploadedDb = await _context.UploadedDbs.FindAsync(id);

            if (uploadedDb is null)
                return NotFound();

            uploadedDb.UploadedDbName = uploadedDbName;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UploadedDbs.Update(uploadedDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UploadedDbExists(id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(uploadedDb);
        }

        // GET: UploadedDbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var uploadedDb = await _context.UploadedDbs
                .Include(u => u.Discipline)
                .Include(u => u.User)
                .ThenInclude(u => u!.UserType)
                .FirstOrDefaultAsync(m => m.UploadedDbId == id);
            if (uploadedDb == null)
            {
                return NotFound();
            }

            return View(uploadedDb);
        }

        // POST: UploadedDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uploadedDb = await _context.UploadedDbs.FindAsync(id);
            if (uploadedDb != null)
            {
                //var db = await _context.UploadedDbs
                //    .Include(u => u.UploadedDbResults)
                //    .ThenInclude(r => r.UploadedDbRecords)
                //    .FirstAsync(db => db.UploadedDbId == id);

                //var resultsToRemove = db.UploadedDbResults;
                //var recordsToRemove = new List<UploadedDbRecord>();

                //foreach (var r in resultsToRemove)
                //{

                //}

                //_context.RemoveRange(db.UploadedDbResults.Select(dbr => dbr.UploadedDbRecords));

                _context.UploadedDbs.Remove(uploadedDb);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UploadedDbExists(int id)
        {
            return _context.UploadedDbs.Any(e => e.UploadedDbId == id);
        }
    }
}
