using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Actors_ActorId",
                table: "MoviesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesActors_Movies_MovieId",
                table: "MoviesActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            //migrationBuilder.DeleteData(
            //    table: "Users",
            //    keyColumn: "Id",
            //    keyValue: 1);

            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "Actors");

            migrationBuilder.RenameTable(
                name: "MoviesActors",
                newName: "MovieActors");

            migrationBuilder.RenameIndex(
                name: "IX_MoviesActors_ActorId",
                table: "MovieActors",
                newName: "IX_MovieActors_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors",
                columns: new[] { "MovieId", "ActorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Actors_ActorId",
                table: "MovieActors",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Movies_MovieId",
                table: "MovieActors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Actors_ActorId",
                table: "MovieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Movies_MovieId",
                table: "MovieActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieActors",
                table: "MovieActors");

            migrationBuilder.RenameTable(
                name: "MovieActors",
                newName: "MoviesActors");

            migrationBuilder.RenameIndex(
                name: "IX_MovieActors_ActorId",
                table: "MoviesActors",
                newName: "IX_MoviesActors_ActorId");

            migrationBuilder.AddColumn<int>(
                name: "BirthYear",
                table: "Actors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                columns: new[] { "MovieId", "ActorId" });

            //migrationBuilder.InsertData(
            //    table: "Users",
            //    columns: new[] { "Id", "Password", "Role", "Status", "Username" },
            //    columns: new[] { "Id", "Password", "Role", "Status", "Username" },
            //    valu  es: new object[] { 1, "admin123", 2, 1, "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Actors_ActorId",
                table: "MoviesActors",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesActors_Movies_MovieId",
                table: "MoviesActors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
