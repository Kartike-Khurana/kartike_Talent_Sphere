using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSphere.Migrations
{
    /// <inheritdoc />
    public partial class FixTrainingEnumDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix rows with empty-string enum values created by migration defaultValue: ""
            migrationBuilder.Sql("UPDATE Trainings SET TrainingType = 'Other' WHERE TrainingType = ''");
            migrationBuilder.Sql("UPDATE Trainings SET DeliveryMode = 'Online' WHERE DeliveryMode = ''");
            // Fix rows with default DateTime min-value dates
            migrationBuilder.Sql("UPDATE Trainings SET StartDate = GETUTCDATE() WHERE StartDate = '0001-01-01 00:00:00.0000000'");
            migrationBuilder.Sql("UPDATE Trainings SET EndDate = DATEADD(day, 1, GETUTCDATE()) WHERE EndDate = '0001-01-01 00:00:00.0000000'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
