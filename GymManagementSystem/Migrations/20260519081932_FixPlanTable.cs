using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixPlanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Plan_Duration",
                table: "Plan");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Plan_Duration",
                table: "Plan",
                sql: "Duration BETWEEN 1 AND 365");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Plan_Duration",
                table: "Plan");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Plan_Duration",
                table: "Plan",
                sql: "Duration Between 1 and 365");
        }
    }
}
