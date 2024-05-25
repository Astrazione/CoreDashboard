﻿using CoreDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreDashboard
{
	public class ApplicationContext : DbContext
	{
		protected readonly IConfiguration Configuration;

		public ApplicationContext(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(Configuration.GetConnectionString("CoreDashboardDatabase"));
		}

		public DbSet<Discipline> Disciplines { get; set; } = null!;
		public DbSet<PairTheme> PairThemes { get; set; } = null!;
		public DbSet<Student> Students { get; set; } = null!;
		public DbSet<StudyDirection> StudyDirections { get; set; } = null!;
		public DbSet<StudyGroup> StudyGroups { get; set; } = null!;
		public DbSet<Teacher> Teachers { get; set; } = null!;
		public DbSet<UploadedDb> UploadedDbs { get; set; } = null!;
		public DbSet<UploadedDbRecord> UploadedDbRecords { get; set; } = null!;
		public DbSet<UploadedDbResult> UploadedDbResults { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<UserType> UserTypes { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Добавление начальных данных
			modelBuilder.Entity<PairType>().HasData(
				new PairType() { PairTypeId = 1, PairTypeName = "Лекция" },
				new PairType() { PairTypeId = 2, PairTypeName = "Практика" }
			);

			modelBuilder.Entity<UserType>().HasData(
				new UserType() { UserTypeId = 1, UserTypeName = "Администратор" },
				new UserType() { UserTypeId = 2, UserTypeName = "Куратор" }
			);

			modelBuilder.Entity<User>().HasData(new User() { UserId = 1, UserName = "admin", UserEmail = "admin@ya.ru", UserPassword = "password", UserTypeId = 1 });
			modelBuilder.Entity<UploadedDb>().HasMany(db => db.UploadedDbResults).WithOne(dbr => dbr.UploadedDb).OnDelete(DeleteBehavior.ClientCascade);
			modelBuilder.Entity<UploadedDbResult>().HasMany(db => db.UploadedDbRecords).WithOne(dbr => dbr.UploadedDbResult).OnDelete(DeleteBehavior.ClientCascade);
		}
	}
}
