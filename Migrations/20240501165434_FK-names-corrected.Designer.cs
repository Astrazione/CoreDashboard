﻿// <auto-generated />
using System;
using CoreDashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoreDashboard.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240501165434_FK-names-corrected")]
    partial class FKnamescorrected
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CoreDashboard.Models.Discipline", b =>
                {
                    b.Property<int>("DisciplineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("discipline_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DisciplineId"));

                    b.Property<string>("DisciplineName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discipline_name");

                    b.HasKey("DisciplineId");

                    b.ToTable("discipline");
                });

            modelBuilder.Entity("CoreDashboard.Models.PairTheme", b =>
                {
                    b.Property<int>("PairThemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("pair_theme_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PairThemeId"));

                    b.Property<string>("PairThemeName")
                        .HasColumnType("text")
                        .HasColumnName("pair_theme_name");

                    b.HasKey("PairThemeId");

                    b.ToTable("pair_theme");
                });

            modelBuilder.Entity("CoreDashboard.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("student_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StudentId"));

                    b.Property<string>("StudentEmail")
                        .HasColumnType("text")
                        .HasColumnName("student_email");

                    b.Property<string>("StudentName")
                        .HasColumnType("text")
                        .HasColumnName("student_name");

                    b.HasKey("StudentId");

                    b.ToTable("student");
                });

            modelBuilder.Entity("CoreDashboard.Models.StudyGroup", b =>
                {
                    b.Property<int>("StudyGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("study_group_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StudyGroupId"));

                    b.Property<string>("StudyGroupName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("study_group_name");

                    b.Property<int>("TeacherId")
                        .HasColumnType("integer")
                        .HasColumnName("teacher_id");

                    b.HasKey("StudyGroupId");

                    b.HasIndex("TeacherId");

                    b.ToTable("study_group");
                });

            modelBuilder.Entity("CoreDashboard.Models.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("teacher_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TeacherId"));

                    b.Property<string>("TeacherName")
                        .HasColumnType("text")
                        .HasColumnName("teacher_name");

                    b.HasKey("TeacherId");

                    b.ToTable("teacher");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDb", b =>
                {
                    b.Property<int>("UploadedDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("uploaded_db_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UploadedDbId"));

                    b.Property<int>("DisciplineId")
                        .HasColumnType("integer")
                        .HasColumnName("discipline_id");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("upload_date");

                    b.Property<string>("UploadedDbName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("uploaded_db_name");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("UploadedDbId");

                    b.HasIndex("DisciplineId");

                    b.HasIndex("UserId");

                    b.ToTable("uploaded_db");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDbRecord", b =>
                {
                    b.Property<int>("UploadedDbRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("uploaded_db_record_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UploadedDbRecordId"));

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hash");

                    b.Property<bool>("IsControlPoint")
                        .HasColumnType("boolean")
                        .HasColumnName("is_control_point");

                    b.Property<int>("PairThemeId")
                        .HasColumnType("integer")
                        .HasColumnName("pair_theme_id");

                    b.Property<bool>("Presence")
                        .HasColumnType("boolean")
                        .HasColumnName("presence");

                    b.Property<int?>("StudentId")
                        .HasColumnType("integer");

                    b.Property<decimal>("ThemeScore")
                        .HasColumnType("numeric")
                        .HasColumnName("theme_score");

                    b.Property<int>("UploadedDbResultId")
                        .HasColumnType("integer")
                        .HasColumnName("uploaded_db_result_id");

                    b.HasKey("UploadedDbRecordId");

                    b.HasIndex("PairThemeId");

                    b.HasIndex("StudentId");

                    b.HasIndex("UploadedDbResultId");

                    b.ToTable("uploaded_db_record");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDbResult", b =>
                {
                    b.Property<int>("UploadedDbResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("uploaded_db_result_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UploadedDbResultId"));

                    b.Property<string>("Rating")
                        .HasColumnType("text")
                        .HasColumnName("rating");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer")
                        .HasColumnName("student_id");

                    b.Property<int>("StudyGroupId")
                        .HasColumnType("integer")
                        .HasColumnName("study_group_id");

                    b.Property<decimal>("TotalScore")
                        .HasColumnType("numeric")
                        .HasColumnName("total_score");

                    b.Property<int>("UploadedDbId")
                        .HasColumnType("integer")
                        .HasColumnName("uploaded_db_id");

                    b.HasKey("UploadedDbResultId");

                    b.HasIndex("StudentId");

                    b.HasIndex("StudyGroupId");

                    b.HasIndex("UploadedDbId");

                    b.ToTable("uploaded_db_result");
                });

            modelBuilder.Entity("CoreDashboard.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_email");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_password");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("user_type_id");

                    b.HasKey("UserId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("user");
                });

            modelBuilder.Entity("CoreDashboard.Models.UserType", b =>
                {
                    b.Property<int>("UserTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_type_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserTypeId"));

                    b.Property<string>("UserTypeName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_type_name");

                    b.HasKey("UserTypeId");

                    b.ToTable("user_type");
                });

            modelBuilder.Entity("CoreDashboard.Models.StudyGroup", b =>
                {
                    b.HasOne("CoreDashboard.Models.Teacher", "Teacher")
                        .WithMany("StudyGroups")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDb", b =>
                {
                    b.HasOne("CoreDashboard.Models.Discipline", "Discipline")
                        .WithMany("UploadedDbs")
                        .HasForeignKey("DisciplineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreDashboard.Models.User", "User")
                        .WithMany("UploadedDbs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discipline");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDbRecord", b =>
                {
                    b.HasOne("CoreDashboard.Models.PairTheme", "PairTheme")
                        .WithMany("UploadedDbRecords")
                        .HasForeignKey("PairThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreDashboard.Models.Student", null)
                        .WithMany("UploadedDbRecords")
                        .HasForeignKey("StudentId");

                    b.HasOne("CoreDashboard.Models.UploadedDbResult", "UploadedDbResult")
                        .WithMany("UploadedDbRecords")
                        .HasForeignKey("UploadedDbResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PairTheme");

                    b.Navigation("UploadedDbResult");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDbResult", b =>
                {
                    b.HasOne("CoreDashboard.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreDashboard.Models.StudyGroup", "StudyGroup")
                        .WithMany("UploadedDbResults")
                        .HasForeignKey("StudyGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreDashboard.Models.UploadedDb", "UploadedDb")
                        .WithMany("UploadedDbResults")
                        .HasForeignKey("UploadedDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("StudyGroup");

                    b.Navigation("UploadedDb");
                });

            modelBuilder.Entity("CoreDashboard.Models.User", b =>
                {
                    b.HasOne("CoreDashboard.Models.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("CoreDashboard.Models.Discipline", b =>
                {
                    b.Navigation("UploadedDbs");
                });

            modelBuilder.Entity("CoreDashboard.Models.PairTheme", b =>
                {
                    b.Navigation("UploadedDbRecords");
                });

            modelBuilder.Entity("CoreDashboard.Models.Student", b =>
                {
                    b.Navigation("UploadedDbRecords");
                });

            modelBuilder.Entity("CoreDashboard.Models.StudyGroup", b =>
                {
                    b.Navigation("UploadedDbResults");
                });

            modelBuilder.Entity("CoreDashboard.Models.Teacher", b =>
                {
                    b.Navigation("StudyGroups");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDb", b =>
                {
                    b.Navigation("UploadedDbResults");
                });

            modelBuilder.Entity("CoreDashboard.Models.UploadedDbResult", b =>
                {
                    b.Navigation("UploadedDbRecords");
                });

            modelBuilder.Entity("CoreDashboard.Models.User", b =>
                {
                    b.Navigation("UploadedDbs");
                });

            modelBuilder.Entity("CoreDashboard.Models.UserType", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
