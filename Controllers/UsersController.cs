using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using CoreDashboard.Extensions;
using CoreDashboard.Models.ViewModels;
using System.Security.Claims;

namespace CoreDashboard.Controllers
{
	[Authorize]
	public class UsersController(ApplicationContext context) : Controller
	{
		private readonly ApplicationContext _context = context;

		// GET: Users
		public async Task<IActionResult> Index()
		{
			var applicationContext = _context.Users.Include(u => u.UserType);
			return View(await applicationContext.ToListAsync());
		}

		// GET: Users/Create
		[Authorize(Roles = "Администратор")]
		public IActionResult Create()
		{
			ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeName");
			return View();
		}

		// POST: Users/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Администратор")]
		public async Task<IActionResult> Create([Bind("UserId,UserName,UserEmail,UserPassword,UserTypeId")] User user)
		{
			if (ModelState.IsValid)
			{
				user.UserPassword = Cryptography.CreateSHA256(user.UserEmail + user.UserPassword);

				_context.Add(user);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeName", user.UserTypeId);
			return View(user);
		}

		// GET: Users/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var user = await _context.Users.FindAsync(id);
			if (user == null)
				return NotFound();

			if (!(user.UserEmail == User.FindFirstValue(ClaimTypes.Email) || User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) == "Администратор"))
				return Forbid();

			TempData["OldPassword"] = user.UserPassword;
			TempData["OldUserType"] = user.UserTypeId;
			TempData["UserEmail"] = user.UserEmail;

			ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeName", user.UserTypeId);
			return View(user);
		}

		// POST: Users/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserEmail,UserPassword,UserTypeId")] UserUpdateViewModel userUpdateViewModel)
		{
			if (id != userUpdateViewModel.UserId)
				return NotFound();

			User user = new(userUpdateViewModel.UserId, userUpdateViewModel.UserName, userUpdateViewModel.UserEmail, userUpdateViewModel.UserPassword ?? "", userUpdateViewModel.UserTypeId);

			if (!(User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) == "Администратор" || user.UserEmail == User.FindFirstValue(ClaimTypes.Email)))
				return Forbid();

			if (ModelState.IsValid)
			{
				try
				{
					if (User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) is not "Администратор")
						user.UserTypeId = (int)(TempData["OldType"] ?? 2);

					user.UserEmail = TempData["UserEmail"] as string ?? user.UserEmail;
					string oldPassword = TempData["OldPassword"] as string ?? "";
					string newPassword = user.UserPassword is null ? "" : Cryptography.CreateSHA256(user.UserEmail + user.UserPassword);

					if (!string.IsNullOrWhiteSpace(newPassword) && newPassword != oldPassword)
						user.UserPassword = newPassword;
					else
						user.UserPassword = oldPassword;

					_context.Update(user);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(user.UserId))
						return NotFound();
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeId", user.UserTypeId);
			return View(user);
		}

		// GET: Users/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var user = await _context.Users
				.Include(u => u.UserType)
				.FirstOrDefaultAsync(m => m.UserId == id);
			if (user == null)
				return NotFound();

			if (!(User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) == "Администратор" || user?.UserEmail == User.FindFirstValue(ClaimTypes.Email)))
				return Forbid();

			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var user = await _context.Users
				.Include(u => u.UploadedDbs)
				.FirstOrDefaultAsync(u => u.UserId == id);

			if (user is null)
				return NotFound();

			if (!(User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType) == "Администратор" || user?.UserEmail == User.FindFirstValue(ClaimTypes.Email)))
				return Forbid();

			foreach (var uploadedDb in user!.UploadedDbs)
				uploadedDb.UserId = null;

			_context.Users.Remove(user);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.UserId == id);
		}
	}
}
