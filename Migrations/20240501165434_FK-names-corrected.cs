using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreDashboard.Migrations
{
    /// <inheritdoc />
    public partial class FKnamescorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_study_group_teacher_TeacherId",
                table: "study_group");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_discipline_DisciplineId",
                table: "uploaded_db");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_user_UserId",
                table: "uploaded_db");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_record_pair_theme_PairThemeId",
                table: "uploaded_db_record");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_UploadedDbResultId",
                table: "uploaded_db_record");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_student_StudentId",
                table: "uploaded_db_result");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_study_group_StudyGroupId",
                table: "uploaded_db_result");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_UploadedDbId",
                table: "uploaded_db_result");

            migrationBuilder.DropForeignKey(
                name: "FK_user_user_type_UserTypeId",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                table: "user",
                newName: "user_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_UserTypeId",
                table: "user",
                newName: "IX_user_user_type_id");

            migrationBuilder.RenameColumn(
                name: "UploadedDbId",
                table: "uploaded_db_result",
                newName: "uploaded_db_id");

            migrationBuilder.RenameColumn(
                name: "StudyGroupId",
                table: "uploaded_db_result",
                newName: "study_group_id");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "uploaded_db_result",
                newName: "student_id");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_result_UploadedDbId",
                table: "uploaded_db_result",
                newName: "IX_uploaded_db_result_uploaded_db_id");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_result_StudyGroupId",
                table: "uploaded_db_result",
                newName: "IX_uploaded_db_result_study_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_result_StudentId",
                table: "uploaded_db_result",
                newName: "IX_uploaded_db_result_student_id");

            migrationBuilder.RenameColumn(
                name: "UploadedDbResultId",
                table: "uploaded_db_record",
                newName: "uploaded_db_result_id");

            migrationBuilder.RenameColumn(
                name: "PairThemeId",
                table: "uploaded_db_record",
                newName: "pair_theme_id");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_record_UploadedDbResultId",
                table: "uploaded_db_record",
                newName: "IX_uploaded_db_record_uploaded_db_result_id");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_record_PairThemeId",
                table: "uploaded_db_record",
                newName: "IX_uploaded_db_record_pair_theme_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "uploaded_db",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "DisciplineId",
                table: "uploaded_db",
                newName: "discipline_id");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_UserId",
                table: "uploaded_db",
                newName: "IX_uploaded_db_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_DisciplineId",
                table: "uploaded_db",
                newName: "IX_uploaded_db_discipline_id");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "study_group",
                newName: "teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_study_group_TeacherId",
                table: "study_group",
                newName: "IX_study_group_teacher_id");

            migrationBuilder.AddForeignKey(
                name: "FK_study_group_teacher_teacher_id",
                table: "study_group",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_discipline_discipline_id",
                table: "uploaded_db",
                column: "discipline_id",
                principalTable: "discipline",
                principalColumn: "discipline_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_user_user_id",
                table: "uploaded_db",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_record_pair_theme_pair_theme_id",
                table: "uploaded_db_record",
                column: "pair_theme_id",
                principalTable: "pair_theme",
                principalColumn: "pair_theme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_uploaded_db_result_id",
                table: "uploaded_db_record",
                column: "uploaded_db_result_id",
                principalTable: "uploaded_db_result",
                principalColumn: "uploaded_db_result_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_student_student_id",
                table: "uploaded_db_result",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_study_group_study_group_id",
                table: "uploaded_db_result",
                column: "study_group_id",
                principalTable: "study_group",
                principalColumn: "study_group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_uploaded_db_id",
                table: "uploaded_db_result",
                column: "uploaded_db_id",
                principalTable: "uploaded_db",
                principalColumn: "uploaded_db_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_type_user_type_id",
                table: "user",
                column: "user_type_id",
                principalTable: "user_type",
                principalColumn: "user_type_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_study_group_teacher_teacher_id",
                table: "study_group");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_discipline_discipline_id",
                table: "uploaded_db");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_user_user_id",
                table: "uploaded_db");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_record_pair_theme_pair_theme_id",
                table: "uploaded_db_record");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_uploaded_db_result_id",
                table: "uploaded_db_record");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_student_student_id",
                table: "uploaded_db_result");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_study_group_study_group_id",
                table: "uploaded_db_result");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_uploaded_db_id",
                table: "uploaded_db_result");

            migrationBuilder.DropForeignKey(
                name: "FK_user_user_type_user_type_id",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "user_type_id",
                table: "user",
                newName: "UserTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_user_user_type_id",
                table: "user",
                newName: "IX_user_UserTypeId");

            migrationBuilder.RenameColumn(
                name: "uploaded_db_id",
                table: "uploaded_db_result",
                newName: "UploadedDbId");

            migrationBuilder.RenameColumn(
                name: "study_group_id",
                table: "uploaded_db_result",
                newName: "StudyGroupId");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "uploaded_db_result",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_result_uploaded_db_id",
                table: "uploaded_db_result",
                newName: "IX_uploaded_db_result_UploadedDbId");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_result_study_group_id",
                table: "uploaded_db_result",
                newName: "IX_uploaded_db_result_StudyGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_result_student_id",
                table: "uploaded_db_result",
                newName: "IX_uploaded_db_result_StudentId");

            migrationBuilder.RenameColumn(
                name: "uploaded_db_result_id",
                table: "uploaded_db_record",
                newName: "UploadedDbResultId");

            migrationBuilder.RenameColumn(
                name: "pair_theme_id",
                table: "uploaded_db_record",
                newName: "PairThemeId");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_record_uploaded_db_result_id",
                table: "uploaded_db_record",
                newName: "IX_uploaded_db_record_UploadedDbResultId");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_record_pair_theme_id",
                table: "uploaded_db_record",
                newName: "IX_uploaded_db_record_PairThemeId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "uploaded_db",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "discipline_id",
                table: "uploaded_db",
                newName: "DisciplineId");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_user_id",
                table: "uploaded_db",
                newName: "IX_uploaded_db_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_uploaded_db_discipline_id",
                table: "uploaded_db",
                newName: "IX_uploaded_db_DisciplineId");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "study_group",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_study_group_teacher_id",
                table: "study_group",
                newName: "IX_study_group_TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_study_group_teacher_TeacherId",
                table: "study_group",
                column: "TeacherId",
                principalTable: "teacher",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_discipline_DisciplineId",
                table: "uploaded_db",
                column: "DisciplineId",
                principalTable: "discipline",
                principalColumn: "discipline_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_user_UserId",
                table: "uploaded_db",
                column: "UserId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_record_pair_theme_PairThemeId",
                table: "uploaded_db_record",
                column: "PairThemeId",
                principalTable: "pair_theme",
                principalColumn: "pair_theme_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_UploadedDbResultId",
                table: "uploaded_db_record",
                column: "UploadedDbResultId",
                principalTable: "uploaded_db_result",
                principalColumn: "uploaded_db_result_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_student_StudentId",
                table: "uploaded_db_result",
                column: "StudentId",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_study_group_StudyGroupId",
                table: "uploaded_db_result",
                column: "StudyGroupId",
                principalTable: "study_group",
                principalColumn: "study_group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_UploadedDbId",
                table: "uploaded_db_result",
                column: "UploadedDbId",
                principalTable: "uploaded_db",
                principalColumn: "uploaded_db_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_type_UserTypeId",
                table: "user",
                column: "UserTypeId",
                principalTable: "user_type",
                principalColumn: "user_type_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
