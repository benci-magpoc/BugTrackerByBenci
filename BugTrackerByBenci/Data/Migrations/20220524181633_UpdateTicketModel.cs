using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrackerByBenci.Data.Migrations
{
    public partial class UpdateTicketModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterUserId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "SubmitterUserId",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterUserId",
                table: "Tickets",
                column: "SubmitterUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterUserId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "SubmitterUserId",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterUserId",
                table: "Tickets",
                column: "SubmitterUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
