using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Migrations
{
    public partial class AddedMessageGroupsAndServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageChannelKey",
                table: "Medias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServerChannel",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerChannel", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "MessageChannel",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerKey = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageChannel", x => x.Key);
                    table.ForeignKey(
                        name: "FK_MessageChannel_ServerChannel_ServerKey",
                        column: x => x.ServerKey,
                        principalTable: "ServerChannel",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServerChannelMap",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServerChannelKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerChannelMap", x => new { x.UserId, x.ServerChannelKey, x.Key });
                    table.ForeignKey(
                        name: "FK_ServerChannelMap_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerChannelMap_ServerChannel_ServerChannelKey",
                        column: x => x.ServerChannelKey,
                        principalTable: "ServerChannel",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMap",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageChannelKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMap", x => new { x.UserId, x.MessageChannelKey, x.Key });
                    table.ForeignKey(
                        name: "FK_GroupMap_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMap_MessageChannel_MessageChannelKey",
                        column: x => x.MessageChannelKey,
                        principalTable: "MessageChannel",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_MessageChannelKey",
                table: "Medias",
                column: "MessageChannelKey");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMap_MessageChannelKey",
                table: "GroupMap",
                column: "MessageChannelKey");

            migrationBuilder.CreateIndex(
                name: "IX_MessageChannel_ServerKey",
                table: "MessageChannel",
                column: "ServerKey");

            migrationBuilder.CreateIndex(
                name: "IX_ServerChannelMap_ServerChannelKey",
                table: "ServerChannelMap",
                column: "ServerChannelKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_MessageChannel_MessageChannelKey",
                table: "Medias",
                column: "MessageChannelKey",
                principalTable: "MessageChannel",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_MessageChannel_MessageChannelKey",
                table: "Medias");

            migrationBuilder.DropTable(
                name: "GroupMap");

            migrationBuilder.DropTable(
                name: "ServerChannelMap");

            migrationBuilder.DropTable(
                name: "MessageChannel");

            migrationBuilder.DropTable(
                name: "ServerChannel");

            migrationBuilder.DropIndex(
                name: "IX_Medias_MessageChannelKey",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "MessageChannelKey",
                table: "Medias");
        }
    }
}
