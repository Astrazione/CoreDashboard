using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoreDashboard.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationalRecords",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Student = table.Column<string>(type: "text", nullable: false),
                    DisciplineName = table.Column<string>(type: "text", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: false),
                    TopicName = table.Column<string>(type: "text", nullable: false),
                    TopicScore = table.Column<decimal>(type: "numeric", nullable: false),
                    Presence = table.Column<char>(type: "character(1)", nullable: false),
                    ControlPoint = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalScore = table.Column<decimal>(type: "numeric", nullable: false),
                    Rating = table.Column<string>(type: "text", nullable: false),
                    StudyDirection = table.Column<string>(type: "text", nullable: false),
                    Teacher = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalRecords", x => x.RecordId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationalRecords");
        }
    }
}
