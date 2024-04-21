using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;


namespace ConsoleApp2
{
    public class Student
    {
        public string email { get; set; }
        public string theme_name { get; set; }
        public string group_name { get; set; }
        public string professor { get; set; }

        public string hash { get; set; }

        public Student(string email, string theme_name, string group_name, string professor)
        {
            this.email = email;
            this.theme_name = theme_name;
            this.group_name = group_name;
            this.professor = professor;
        }

        public string ComputeHash()
        {
            SHA256 sha256 = SHA256.Create();
            string data = $"{email} {theme_name} {group_name} {professor}";
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2"));
            }
            hash = builder.ToString();
            return hash;
        }

    }

    public class StContext : DbContext
    {
        public DbSet<Student> students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Заменить на нужный
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Student;Username=postgres;Password=Win");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>()
                .HasKey(students => students.email);
        }
    }

    internal class HashSet
    {
        public static HashSet<string> hashSet = new HashSet<string>();
        static void Main()
        {
            var context = new StContext();
            //Путь также заменить
            string path = "Host=localhost;Port=5432;Database=Student;Username=postgres;Password=Win";

            //Создание нового столбца
            string tableName = "students";
            string colName = "hash";
            string addColumnQuery = $"ALTER TABLE {tableName} ADD COLUMN {colName} TEXT Default('default')";
            using (NpgsqlConnection connection = new NpgsqlConnection(path))
            {
                connection.Open();
                using NpgsqlCommand command = new NpgsqlCommand(addColumnQuery, connection);
                command.ExecuteNonQuery();

                Console.WriteLine("Новый столбец успешно добавлен.");
            }


            // Заполнение нового столбца данными с помощью имеющихся строк данных
            var allSt = context.students.ToList();
            foreach (var student in allSt)
            {
                var hash = student.ComputeHash();
                student.hash = hash;

                //Добавление в базу хэшей
                hashSet.Add(hash);
                Console.WriteLine(hash);
            }

            //Попытка добавить существующую строку данных  (поля для тестирования)
            string email = "stud0000277462";
            string email2 = "stud0000277662";
            string theme_name = "Строковые переменные в Python";
            string group_name = "ПиОА Л-08";
            string professor = "Аврискин Михаил Владимирович";

            string hash2 = ComputeHash(email, theme_name, group_name, professor);
            string hash3 = ComputeHash(email2, theme_name, group_name, professor);
            

             //Проверка на существование подобного хэша
            if (!hashSet.Contains(hash2))
            {
                Student st = new Student(email, theme_name, group_name, professor);
                st.hash = hash2;
                context.students.Add(st);
            }
            else
                Console.WriteLine("Такой хэш уже есть");
            

            if (!hashSet.Contains(hash3))
            {
                Student st = new Student(email2, theme_name, group_name, professor);
                st.hash = hash3;
                context.students.Add(st);
            }
            else
                Console.WriteLine("Такой хэш уже есть");
            
            context.SaveChanges();
        }

        public static string ComputeHash(string email, string theme_name, string group_name, string professor)
        {
            SHA256 sha256 = SHA256.Create();
            string data = $"{email} {theme_name} {group_name} {professor}";
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2"));
            }
            string hash = builder.ToString();
            return hash;
        }
    }
}