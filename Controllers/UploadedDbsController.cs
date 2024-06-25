using CoreDashboard.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoreDashboard.Controllers
{
	[Authorize]
	public class UploadedDbsController : Controller
	{
		private readonly ApplicationContext _context;

		public UploadedDbsController(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var applicationContext = _context.UploadedDbs
				.Include(u => u.Discipline)
				.Include(u => u.User)
				.ThenInclude(u => u!.UserType);
			return View(await applicationContext.ToListAsync());
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id is null)
				return NotFound();

			var uploadedDb = await _context.UploadedDbs.Include(db => db.User).FirstOrDefaultAsync(db => db.UploadedDbId == id);

			if (uploadedDb is null)
				return NotFound();

			if (!(User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) == "Администратор" || uploadedDb.User?.UserEmail == User.FindFirstValue(ClaimTypes.Email)))
				return Forbid();

			return View(uploadedDb);
		}

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

			if (id is null)
				return NotFound();

			var uploadedDb = await _context.UploadedDbs
				.Include(u => u.Discipline)
				.Include(u => u.User)
				.ThenInclude(u => u!.UserType)
				.FirstOrDefaultAsync(m => m.UploadedDbId == id);

			if (uploadedDb is null)
				return NotFound();

			if (!(User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) == "Администратор" || uploadedDb.User?.UserEmail == User.FindFirstValue(ClaimTypes.Email)))
				return Forbid();

			return View(uploadedDb);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var uploadedDb = await _context.UploadedDbs
				.Include(db => db.UploadedDbResults)
				.ThenInclude(dbRes => dbRes.UploadedDbRecords)
				.FirstOrDefaultAsync(db => db.UploadedDbId == id);

			if (uploadedDb is null)
				return NotFound();

			if (!(User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) == "Администратор" || uploadedDb.User?.UserEmail == User.FindFirstValue(ClaimTypes.Email)))
				return Forbid();

			foreach (var result in uploadedDb.UploadedDbResults)
			{
				_context.UploadedDbRecords.RemoveRange(result.UploadedDbRecords);
				_context.UploadedDbResults.Remove(result);
			}
			await _context.SaveChangesAsync();

			_context.UploadedDbs.Remove(uploadedDb);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool UploadedDbExists(int id)
		{
			return _context.UploadedDbs.Any(e => e.UploadedDbId == id);
		}
	}
}
