using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoreDashboard.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "discipline",
                columns: table => new
                {
                    discipline_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    discipline_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discipline", x => x.discipline_id);
                });

            migrationBuilder.CreateTable(
                name: "pair_type",
                columns: table => new
                {
                    pair_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pair_type_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pair_type", x => x.pair_type_id);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    student_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_name = table.Column<string>(type: "text", nullable: true),
                    student_email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.student_id);
                });

            migrationBuilder.CreateTable(
                name: "study_direction",
                columns: table => new
                {
                    study_direction_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    study_direction_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_study_direction", x => x.study_direction_id);
                });

            migrationBuilder.CreateTable(
                name: "teacher",
                columns: table => new
                {
                    teacher_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher", x => x.teacher_id);
                });

            migrationBuilder.CreateTable(
                name: "user_type",
                columns: table => new
                {
                    user_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_type_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_type", x => x.user_type_id);
                });

            migrationBuilder.CreateTable(
                name: "pair_theme",
                columns: table => new
                {
                    pair_theme_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pair_theme_name = table.Column<string>(type: "text", nullable: true),
                    pair_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pair_theme", x => x.pair_theme_id);
                    table.ForeignKey(
                        name: "FK_pair_theme_pair_type_pair_type_id",
                        column: x => x.pair_type_id,
                        principalTable: "pair_type",
                        principalColumn: "pair_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "study_group",
                columns: table => new
                {
                    study_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    study_group_name = table.Column<string>(type: "text", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_study_group", x => x.study_group_id);
                    table.ForeignKey(
                        name: "FK_study_group_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "teacher_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    user_email = table.Column<string>(type: "text", nullable: false),
                    user_password = table.Column<string>(type: "text", nullable: false),
                    user_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_user_type_user_type_id",
                        column: x => x.user_type_id,
                        principalTable: "user_type",
                        principalColumn: "user_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_db",
                columns: table => new
                {
                    uploaded_db_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uploaded_db_name = table.Column<string>(type: "text", nullable: false),
                    upload_date = table.Column<DateTime>(type: "Date", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    discipline_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_db", x => x.uploaded_db_id);
                    table.ForeignKey(
                        name: "FK_uploaded_db_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalTable: "discipline",
                        principalColumn: "discipline_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_db_result",
                columns: table => new
                {
                    uploaded_db_result_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uploaded_db_id = table.Column<int>(type: "integer", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    study_direction_id = table.Column<int>(type: "integer", nullable: false),
                    study_group_id = table.Column<int>(type: "integer", nullable: false),
                    total_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    rating = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_db_result", x => x.uploaded_db_result_id);
                    table.ForeignKey(
                        name: "FK_uploaded_db_result_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_result_study_direction_study_direction_id",
                        column: x => x.study_direction_id,
                        principalTable: "study_direction",
                        principalColumn: "study_direction_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_result_study_group_study_group_id",
                        column: x => x.study_group_id,
                        principalTable: "study_group",
                        principalColumn: "study_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_result_uploaded_db_uploaded_db_id",
                        column: x => x.uploaded_db_id,
                        principalTable: "uploaded_db",
                        principalColumn: "uploaded_db_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_db_record",
                columns: table => new
                {
                    uploaded_db_record_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theme_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    is_control_point = table.Column<bool>(type: "boolean", nullable: false),
                    presence = table.Column<bool>(type: "boolean", nullable: true),
                    uploaded_db_result_id = table.Column<int>(type: "integer", nullable: false),
                    pair_theme_id = table.Column<int>(type: "integer", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_db_record", x => x.uploaded_db_record_id);
                    table.ForeignKey(
                        name: "FK_uploaded_db_record_pair_theme_pair_theme_id",
                        column: x => x.pair_theme_id,
                        principalTable: "pair_theme",
                        principalColumn: "pair_theme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_record_uploaded_db_result_uploaded_db_result_id",
                        column: x => x.uploaded_db_result_id,
                        principalTable: "uploaded_db_result",
                        principalColumn: "uploaded_db_result_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "pair_type",
                columns: new[] { "pair_type_id", "pair_type_name" },
                values: new object[,]
                {
                    { 1, "Лекция" },
                    { 2, "Практика" }
                });

            migrationBuilder.InsertData(
                table: "user_type",
                columns: new[] { "user_type_id", "user_type_name" },
                values: new object[,]
                {
                    { 1, "Администратор" },
                    { 2, "Куратор" }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "user_id", "user_email", "user_name", "user_password", "user_type_id" },
                values: new object[] { 1, "admin@ya.ru", "admin", "password", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_pair_theme_pair_type_id",
                table: "pair_theme",
                column: "pair_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_study_group_teacher_id",
                table: "study_group",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_discipline_id",
                table: "uploaded_db",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_user_id",
                table: "uploaded_db",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_record_pair_theme_id",
                table: "uploaded_db_record",
                column: "pair_theme_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_record_uploaded_db_result_id",
                table: "uploaded_db_record",
                column: "uploaded_db_result_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_student_id",
                table: "uploaded_db_result",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_study_direction_id",
                table: "uploaded_db_result",
                column: "study_direction_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_study_group_id",
                table: "uploaded_db_result",
                column: "study_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_uploaded_db_id",
                table: "uploaded_db_result",
                column: "uploaded_db_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_user_type_id",
                table: "user",
                column: "user_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uploaded_db_record");

            migrationBuilder.DropTable(
                name: "pair_theme");

            migrationBuilder.DropTable(
                name: "uploaded_db_result");

            migrationBuilder.DropTable(
                name: "pair_type");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "study_direction");

            migrationBuilder.DropTable(
                name: "study_group");

            migrationBuilder.DropTable(
                name: "uploaded_db");

            migrationBuilder.DropTable(
                name: "teacher");

            migrationBuilder.DropTable(
                name: "discipline");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "user_type");
        }
    }
}
