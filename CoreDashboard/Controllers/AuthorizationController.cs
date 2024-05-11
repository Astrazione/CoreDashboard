using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Npgsql;
using System.Security.Claims;
using CoreDashboard.Models;

namespace CoreDashboard.Controllers
{
	public class AuthorizationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		string connStr = "Host=localhost; Database=core_dashboard; Username=postgres; Password=1234";
		NpgsqlConnection conn;
		NpgsqlCommand cmd;

		[HttpPost]
		public IActionResult Login(string login, string password)
		{
			string text = "";
			try
			{
				conn = new NpgsqlConnection(connStr);
				if (conn.State == System.Data.ConnectionState.Closed)
				{
					conn.Open();
				}

				cmd = conn.CreateCommand();
				cmd.CommandText = "SELECT user_name, user_type_id FROM public.\"user\"\r\nWHERE user_email = @login\r\nAND user_password = @password;";
				cmd.Parameters.AddWithValue("@login", login);
				cmd.Parameters.AddWithValue("@password", password);
				string userName = "";
				int userTypeId = 0;
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						// Получение данных из колонок user_name и user_type_id
						userName = reader.GetString(0); // Индекс 0 соответствует колонке user_name
						userTypeId = reader.GetInt32(1);   // Индекс 1 соответствует колонке user_type_id
					}
					else
					{
						// Пользователь с указанными учетными данными не найден
					}
				}

				cmd.CommandText = "SELECT user_type_name FROM public.\"user_type\"\r\nWHERE user_type_id = @userTypeId;";
				cmd.Parameters.AddWithValue("@userTypeId", userTypeId);
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						string tmp = reader.GetString(0);
						if (tmp == "admin") TempData["Role"] = "Администратор";
						else TempData["Role"] = "Куратор";
					}
					else
					{
						// Пользователь с указанными учетными данными не найден
					}
				}
				TempData["UserName"] = userName;
				conn.Close();
			}
			catch (Exception ex)
			{
				if (conn.State == System.Data.ConnectionState.Open)
				{
					conn.Close();
				}
			}
			return RedirectToAction("Index", "Home");
		}
	
		public IActionResult Logout()
		{
			TempData.Remove("UserName");
			TempData.Remove("Role");
			return RedirectToAction("Index", "Home");
		}
	}
}
