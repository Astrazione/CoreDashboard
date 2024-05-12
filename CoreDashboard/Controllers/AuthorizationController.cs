using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Npgsql;
using System.Security.Claims;
using CoreDashboard.Models;
using System.Security.Cryptography;

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

		public bool CompareHash(byte[] savedHash, byte[] currentHash)
		{
			//побайтовое сравнение результатов хэширования
			int ok = 1;
			for(int i = 0; i < 20; i++) 
				if (savedHash[i + 16] != currentHash[i])
					ok = 0;

			if(ok == 1) return true;

			return false;
		}

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
				/*cmd.CommandText = "SELECT user_name, user_type_id FROM public.\"user\"\r\nWHERE user_email = @login\r\nAND user_password = @password;";*/
				
				//находим пользователя по почте, получаем хэш его пароля и соль
				cmd.CommandText = "SELECT user_name, user_type_id, user_password, salt FROM public.\"user\"\r\nWHERE user_email = @login;";
				cmd.Parameters.AddWithValue("@login", login);

				string userName = "", savedPwdHash;
				int userTypeId = 0;
				bool isPwdCorrect = false;
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						// Получение данных из колонок user_name и user_type_id
						userName = reader.GetString(0); // Индекс 0 соответствует колонке user_name
						userTypeId = reader.GetInt32(1);   // Индекс 1 соответствует колонке user_type_id

						savedPwdHash = reader.GetString(2);
						byte[] hashBytes = Convert.FromBase64String(savedPwdHash); //хэш из бд
						byte[] salt = new byte[16];
						Array.Copy(hashBytes, 0, salt, 0, 16); //получаем соль

						var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000); 
						byte[] hashedCurrent = pbkdf2.GetBytes(20); //получаем хэш введенного пароля

						isPwdCorrect = CompareHash(hashBytes, hashedCurrent);

					}
				}
				//если пароль верный, передаем во view значения ФИО и получаем роль пользователя
				if (isPwdCorrect)
				{
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
				}
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
