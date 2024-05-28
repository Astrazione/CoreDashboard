using CoreDashboard.Extensions;
using CoreDashboard.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoreDashboard.Controllers
{
	[Authorize]
	public class AuthController(ApplicationContext context) : Controller
	{
		private readonly ApplicationContext _context = context;

		[AllowAnonymous]
		[HttpGet("/login")]
		public IActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost("/login")]
		public async Task<IResult> Login(string? returnUrl)
		{
			var form = HttpContext.Request.Form;

			if (!form.ContainsKey("login") || !form.ContainsKey("password"))
				return Results.BadRequest("Email и/или пароль не установлены");

			string email = form["login"];
			string password = form["password"];

			string hash = Cryptography.CreateSHA256(email + password);

			User? user = _context.Users
				.Include(u => u.UserType)
				.FirstOrDefault(u => u.UserEmail == email && u.UserPassword == hash);

			if (user is null) return Results.Unauthorized();

			var claims = new List<Claim>
			{
				new(ClaimsIdentity.DefaultNameClaimType, user.UserName),
				new(ClaimTypes.Email, user.UserEmail),
				new(ClaimsIdentity.DefaultRoleClaimType, user.UserType!.UserTypeName)
			};

			ClaimsIdentity claimsIdentity = new(claims, "Cookies");

			await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
			return Results.Redirect(returnUrl ?? "/");
		}

		[HttpGet("/logout")]
		public async Task<IResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Results.Redirect("/login");
		}
	}
}
