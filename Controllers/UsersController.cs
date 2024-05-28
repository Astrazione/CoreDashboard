using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using CoreDashboard.Extensions;

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
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["UserTypeId"] = new SelectList(_context.UserTypes, "UserTypeId", "UserTypeName", user.UserTypeId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserEmail,UserPassword,UserTypeId")] User user)
        {
            if (id != user.UserId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
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
