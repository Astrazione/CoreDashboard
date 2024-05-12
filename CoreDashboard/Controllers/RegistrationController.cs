using CoreDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CoreDashboard.Controllers
{
	public class RegistrationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		string connStr = "Host=localhost; Database=core_dashboard; Username=postgres; Password=1234";
		NpgsqlConnection conn;
		NpgsqlCommand cmd;

		public string GetHash(byte[] salt, string password)
		{
			//10000 итераций
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

			//string->byte array
			byte[] hash = pbkdf2.GetBytes(20);

			//хранение хэшированного пароля+соли
			//20(хэш)+16(соль)=36
			byte[] hashBytes = new byte[36];

			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);

			string passwordHash = Convert.ToBase64String(hashBytes);

			return passwordHash;
		}

		[HttpPost]
		public IActionResult Register(string[] fullName, string role, string login, string password)
		{
			string result = "";

			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			foreach (string name in fullName)
			{
				result += name + " ";
			}

			int typeId = 2;
			switch (role)
			{
				case "Администратор":
					{
						typeId = 1;
						break;
					}
				case "Куратор направления":
					{
						typeId = 2;
						break;
					}
					/*case "Преподаватель": { break; }
					case "Студент": { break; }*/
			}

			try
			{
				conn = new NpgsqlConnection(connStr);
				if (conn.State == System.Data.ConnectionState.Closed)
				{
					conn.Open();
				}

				string pwdHash = GetHash(salt, password);

				cmd = conn.CreateCommand();
				/*cmd.CommandText = "SELECT COUNT(*) FROM public.\"user\" WHERE user_email = @login";
				cmd.Parameters.AddWithValue("@login", login);
				int count = (int)cmd.ExecuteScalar();*/

				/*if (count > 0)
				{
					// Если запись уже существует, выводим уведомление пользователю
					ViewBag.ErrorMessage = "Пользователь с таким логином уже существует.";
					TempData["Error"] = "Пользователь с таким логином уже существует.";
					*//*return View();*//*
				}
				else
				{*/
					cmd.CommandText = "INSERT INTO public.\"user\"(user_name, user_email, user_password, user_type_id, salt) VALUES (@fullName, @login, @password, @typeId, @salt)";
					cmd.Parameters.AddWithValue("@fullName", result);
					cmd.Parameters.AddWithValue("@login", login);
					cmd.Parameters.AddWithValue("@password", pwdHash);
					cmd.Parameters.AddWithValue("@typeId", typeId);
					cmd.Parameters.AddWithValue("@salt", salt);
					cmd.ExecuteNonQuery();

				if (role == "Администратор") TempData["Role"] = "Администратор";
				else TempData["Role"] = "Куратор";
				TempData["UserName"] = result;
				

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
	}
}
