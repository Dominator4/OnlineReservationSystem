using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservationManagementService.Migrations
{
    public partial class customerIdEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Usunięcie klucza obcego
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations");

            // Usunięcie klucza głównego
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            // Jeśli istnieją inne indeksy zależne od 'Id', również należy je usunąć
            // migrationBuilder.DropIndex(
            //     name: "IX_IndexName",
            //     table: "TableName");

            // Teraz możesz usunąć kolumnę
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Customers");

            // Ponowne dodanie kolumny, jeśli planujesz ją ponownie utworzyć
            // (tutaj bez IDENTITY, jeśli to konieczne)
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Customers",
                nullable: false);

            // Ponowne dodanie klucza głównego
            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            // Ponowne dodanie klucza obcego
            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Customers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
