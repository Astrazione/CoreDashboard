using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

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
                name: "pair_theme",
                columns: table => new
                {
                    pair_theme_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pair_theme_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pair_theme", x => x.pair_theme_id);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_name = table.Column<string>(type: "text", nullable: true),
                    student_email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.student_id);
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
                name: "study_group",
                columns: table => new
                {
                    study_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    study_group_name = table.Column<string>(type: "text", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_study_group", x => x.study_group_id);
                    table.ForeignKey(
                        name: "FK_study_group_teacher_TeacherId",
                        column: x => x.TeacherId,
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
                    UserTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_user_type_UserTypeId",
                        column: x => x.UserTypeId,
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
                    upload_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DisciplineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_db", x => x.uploaded_db_id);
                    table.ForeignKey(
                        name: "FK_uploaded_db_discipline_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "discipline",
                        principalColumn: "discipline_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_user_UserId",
                        column: x => x.UserId,
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
                    UploadedDbId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    StudyGroupId = table.Column<int>(type: "integer", nullable: false),
                    total_score = table.Column<decimal>(type: "numeric", nullable: false),
                    rating = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_db_result", x => x.uploaded_db_result_id);
                    table.ForeignKey(
                        name: "FK_uploaded_db_result_student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_result_study_group_StudyGroupId",
                        column: x => x.StudyGroupId,
                        principalTable: "study_group",
                        principalColumn: "study_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_result_uploaded_db_UploadedDbId",
                        column: x => x.UploadedDbId,
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
                    theme_score = table.Column<decimal>(type: "numeric", nullable: false),
                    is_control_point = table.Column<bool>(type: "boolean", nullable: false),
                    presence = table.Column<bool>(type: "boolean", nullable: false),
                    UploadedDbResultId = table.Column<int>(type: "integer", nullable: false),
                    PairThemeId = table.Column<int>(type: "integer", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_db_record", x => x.uploaded_db_record_id);
                    table.ForeignKey(
                        name: "FK_uploaded_db_record_pair_theme_PairThemeId",
                        column: x => x.PairThemeId,
                        principalTable: "pair_theme",
                        principalColumn: "pair_theme_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploaded_db_record_student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "student",
                        principalColumn: "student_id");
                    table.ForeignKey(
                        name: "FK_uploaded_db_record_uploaded_db_result_UploadedDbResultId",
                        column: x => x.UploadedDbResultId,
                        principalTable: "uploaded_db_result",
                        principalColumn: "uploaded_db_result_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_study_group_TeacherId",
                table: "study_group",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_DisciplineId",
                table: "uploaded_db",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_UserId",
                table: "uploaded_db",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_record_PairThemeId",
                table: "uploaded_db_record",
                column: "PairThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_record_StudentId",
                table: "uploaded_db_record",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_record_UploadedDbResultId",
                table: "uploaded_db_record",
                column: "UploadedDbResultId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_StudentId",
                table: "uploaded_db_result",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_StudyGroupId",
                table: "uploaded_db_result",
                column: "StudyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_UploadedDbId",
                table: "uploaded_db_result",
                column: "UploadedDbId");

            migrationBuilder.CreateIndex(
                name: "IX_user_UserTypeId",
                table: "user",
                column: "UserTypeId");
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
                name: "student");

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
