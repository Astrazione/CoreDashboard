using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreDashboard.Migrations
{
    /// <inheritdoc />
    public partial class ViewsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR REPLACE VIEW direction_db_presence AS
SELECT dbrec.presence, sd.study_direction_id, sd.study_direction_name, dbres.uploaded_db_id FROM uploaded_db_record dbrec
JOIN uploaded_db_result dbres ON dbrec.uploaded_db_result_id = dbres.uploaded_db_result_id
JOIN study_direction sd ON dbres.study_direction_id = sd.study_direction_id;

CREATE OR REPLACE VIEW group_db_presence AS
SELECT presence, study_group_name, uploaded_db_id FROM uploaded_db_record dbrec
JOIN uploaded_db_result dbres ON dbrec.uploaded_db_result_id = dbres.uploaded_db_result_id
JOIN study_group sg ON dbres.study_group_id = sg.study_group_id;

CREATE OR REPLACE VIEW student_db_presence AS
SELECT presence, student_id, study_group_name, study_direction_name, uploaded_db_id FROM uploaded_db_record dbrec
JOIN uploaded_db_result dbres ON dbrec.uploaded_db_result_id = dbres.uploaded_db_result_id
JOIN study_group sg ON dbres.study_group_id = sg.study_group_id
JOIN study_direction sd ON dbres.study_direction_id = sd.study_direction_id;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
DROP VIEW direction_db_presence;

DROP VIEW group_db_presence;

DROP VIEW student_db_presence;
            ");
		}
    }
}
