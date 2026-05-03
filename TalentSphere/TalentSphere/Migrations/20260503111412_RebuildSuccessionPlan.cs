using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentSphere.Migrations
{
    /// <inheritdoc />
    public partial class RebuildSuccessionPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "SuccessionPlans");

            migrationBuilder.DropColumn(
                name: "Timeline",
                table: "SuccessionPlans");

            migrationBuilder.AddColumn<int>(
                name: "SuccessorID",
                table: "SuccessionPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SuccessionPlans_SuccessorID",
                table: "SuccessionPlans",
                column: "SuccessorID");

            migrationBuilder.AddForeignKey(
                name: "FK_SuccessionPlans_Employees_SuccessorID",
                table: "SuccessionPlans",
                column: "SuccessorID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuccessionPlans_Employees_SuccessorID",
                table: "SuccessionPlans");

            migrationBuilder.DropIndex(
                name: "IX_SuccessionPlans_SuccessorID",
                table: "SuccessionPlans");

            migrationBuilder.DropColumn(
                name: "SuccessorID",
                table: "SuccessionPlans");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "SuccessionPlans",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Timeline",
                table: "SuccessionPlans",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
