using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_uploaded_db_result_id",
                table: "uploaded_db_record");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_uploaded_db_id",
                table: "uploaded_db_result");

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_uploaded_db_result_id",
                table: "uploaded_db_record",
                column: "uploaded_db_result_id",
                principalTable: "uploaded_db_result",
                principalColumn: "uploaded_db_result_id");

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_uploaded_db_id",
                table: "uploaded_db_result",
                column: "uploaded_db_id",
                principalTable: "uploaded_db",
                principalColumn: "uploaded_db_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_uploaded_db_result_id",
                table: "uploaded_db_record");

            migrationBuilder.DropForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_uploaded_db_id",
                table: "uploaded_db_result");

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_record_uploaded_db_result_uploaded_db_result_id",
                table: "uploaded_db_record",
                column: "uploaded_db_result_id",
                principalTable: "uploaded_db_result",
                principalColumn: "uploaded_db_result_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploaded_db_result_uploaded_db_uploaded_db_id",
                table: "uploaded_db_result",
                column: "uploaded_db_id",
                principalTable: "uploaded_db",
                principalColumn: "uploaded_db_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
