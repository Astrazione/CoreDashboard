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
        public string name { get; set; }
        public int course { get; set; }
        public int grade { get; set; }

        public string hash { get; set; }

        public Student(string name, int course, int grade)
        {
            this.name = name;
            this.course = course;
            this.grade = grade;
        }

        public string ComputeHash()
        {
            SHA256 sha256 = SHA256.Create();
            string data = $"{name} {course} {grade}";
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
            
            /*modelBuilder.Entity<Student>()
                .HasKey(students => students.name);*/
        }
    }

    internal class Program
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
            string name = "Женя";
            int course = 2;
            int grade = 5;
            string hash2 = ComputeHash(name, course, grade);
            
            //Проверка на существование подобного хэша
            if (!hashSet.Contains(hash2))
            {
                Student st = new Student(name, course, grade);
                st.hash = hash2;
                context.students.Add(st);
            }
            else
            {
                Console.WriteLine("Такой хэш уже есть");
                //continue;
            }
            
            context.SaveChanges();
        }

        public static string ComputeHash(string name, int course, int grade)
        {
            SHA256 sha256 = SHA256.Create();
            string data = $"{name} {course} {grade}";
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2"));
            }
            string hash = builder.ToString();
            return hash;
        }


        //Для будущих проверок на уникальность. Заполняем его имеющимися hash
        public static void fillHashSet(List<Student> allSt)
        {
            foreach(Student student in allSt) 
            {
                hashSet.Add(student.hash);
            }
        }
    }
}
