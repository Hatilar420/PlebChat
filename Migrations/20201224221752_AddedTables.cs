using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.Migrations
{
    public partial class AddedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMap_AspNetUsers_UserId",
                table: "GroupMap");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMap_MessageChannel_MessageChannelKey",
                table: "GroupMap");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_MessageChannel_MessageChannelKey",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageChannel_ServerChannel_ServerKey",
                table: "MessageChannel");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerChannelMap_AspNetUsers_UserId",
                table: "ServerChannelMap");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerChannelMap_ServerChannel_ServerChannelKey",
                table: "ServerChannelMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerChannelMap",
                table: "ServerChannelMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerChannel",
                table: "ServerChannel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageChannel",
                table: "MessageChannel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMap",
                table: "GroupMap");

            migrationBuilder.RenameTable(
                name: "ServerChannelMap",
                newName: "serverChannelMaps");

            migrationBuilder.RenameTable(
                name: "ServerChannel",
                newName: "ServerChannels");

            migrationBuilder.RenameTable(
                name: "MessageChannel",
                newName: "messageChannels");

            migrationBuilder.RenameTable(
                name: "GroupMap",
                newName: "GroupMaps");

            migrationBuilder.RenameIndex(
                name: "IX_ServerChannelMap_ServerChannelKey",
                table: "serverChannelMaps",
                newName: "IX_serverChannelMaps_ServerChannelKey");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChannel_ServerKey",
                table: "messageChannels",
                newName: "IX_messageChannels_ServerKey");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMap_MessageChannelKey",
                table: "GroupMaps",
                newName: "IX_GroupMaps_MessageChannelKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_serverChannelMaps",
                table: "serverChannelMaps",
                columns: new[] { "UserId", "ServerChannelKey", "Key" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerChannels",
                table: "ServerChannels",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_messageChannels",
                table: "messageChannels",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMaps",
                table: "GroupMaps",
                columns: new[] { "UserId", "MessageChannelKey", "Key" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMaps_AspNetUsers_UserId",
                table: "GroupMaps",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMaps_messageChannels_MessageChannelKey",
                table: "GroupMaps",
                column: "MessageChannelKey",
                principalTable: "messageChannels",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_messageChannels_MessageChannelKey",
                table: "Medias",
                column: "MessageChannelKey",
                principalTable: "messageChannels",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_messageChannels_ServerChannels_ServerKey",
                table: "messageChannels",
                column: "ServerKey",
                principalTable: "ServerChannels",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_serverChannelMaps_AspNetUsers_UserId",
                table: "serverChannelMaps",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_serverChannelMaps_ServerChannels_ServerChannelKey",
                table: "serverChannelMaps",
                column: "ServerChannelKey",
                principalTable: "ServerChannels",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMaps_AspNetUsers_UserId",
                table: "GroupMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMaps_messageChannels_MessageChannelKey",
                table: "GroupMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_messageChannels_MessageChannelKey",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_messageChannels_ServerChannels_ServerKey",
                table: "messageChannels");

            migrationBuilder.DropForeignKey(
                name: "FK_serverChannelMaps_AspNetUsers_UserId",
                table: "serverChannelMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_serverChannelMaps_ServerChannels_ServerChannelKey",
                table: "serverChannelMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerChannels",
                table: "ServerChannels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_serverChannelMaps",
                table: "serverChannelMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_messageChannels",
                table: "messageChannels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMaps",
                table: "GroupMaps");

            migrationBuilder.RenameTable(
                name: "ServerChannels",
                newName: "ServerChannel");

            migrationBuilder.RenameTable(
                name: "serverChannelMaps",
                newName: "ServerChannelMap");

            migrationBuilder.RenameTable(
                name: "messageChannels",
                newName: "MessageChannel");

            migrationBuilder.RenameTable(
                name: "GroupMaps",
                newName: "GroupMap");

            migrationBuilder.RenameIndex(
                name: "IX_serverChannelMaps_ServerChannelKey",
                table: "ServerChannelMap",
                newName: "IX_ServerChannelMap_ServerChannelKey");

            migrationBuilder.RenameIndex(
                name: "IX_messageChannels_ServerKey",
                table: "MessageChannel",
                newName: "IX_MessageChannel_ServerKey");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMaps_MessageChannelKey",
                table: "GroupMap",
                newName: "IX_GroupMap_MessageChannelKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerChannel",
                table: "ServerChannel",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerChannelMap",
                table: "ServerChannelMap",
                columns: new[] { "UserId", "ServerChannelKey", "Key" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageChannel",
                table: "MessageChannel",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMap",
                table: "GroupMap",
                columns: new[] { "UserId", "MessageChannelKey", "Key" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMap_AspNetUsers_UserId",
                table: "GroupMap",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMap_MessageChannel_MessageChannelKey",
                table: "GroupMap",
                column: "MessageChannelKey",
                principalTable: "MessageChannel",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_MessageChannel_MessageChannelKey",
                table: "Medias",
                column: "MessageChannelKey",
                principalTable: "MessageChannel",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChannel_ServerChannel_ServerKey",
                table: "MessageChannel",
                column: "ServerKey",
                principalTable: "ServerChannel",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerChannelMap_AspNetUsers_UserId",
                table: "ServerChannelMap",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerChannelMap_ServerChannel_ServerChannelKey",
                table: "ServerChannelMap",
                column: "ServerChannelKey",
                principalTable: "ServerChannel",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
