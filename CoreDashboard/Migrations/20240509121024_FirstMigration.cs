using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoreDashboard.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "study_direction_id",
                table: "uploaded_db_result",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "study_direction",
                columns: table => new
                {
                    study_direction_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    study_direction_name = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_study_direction", x => x.study_direction_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_result_study_direction_id",
                table: "uploaded_db_result",
                column: "study_direction_id");

            /*migrationBuilder.CreateIndex(
                name: "IX_uploaded_db_record_StudentId",
                table: "uploaded_db_record",
                column: "StudentId");*/

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_study_direction_study_direction_id",
                table: "uploaded_db_result",
                column: "study_direction_id",
                principalTable: "study_direction",
                principalColumn: "study_direction_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_study_direction_study_direction_id",
                table: "uploaded_db_result");

            migrationBuilder.DropTable(
                name: "study_direction");

            migrationBuilder.DropIndex(
                name: "IX_uploaded_db_result_study_direction_id",
                table: "uploaded_db_result");

            migrationBuilder.DropIndex(
                name: "IX_uploaded_db_record_StudentId",
                table: "uploaded_db_record");

            migrationBuilder.DropColumn(
                name: "study_direction_id",
                table: "uploaded_db_result");
        }
    }
}
