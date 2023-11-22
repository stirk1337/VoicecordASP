using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voicecord.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkImageGroup = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_Groups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoiceChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceChat_Groups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SdpMLineIndex = table.Column<int>(type: "int", nullable: false),
                    SdpMLine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsernameFragment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoiceChatId = table.Column<int>(type: "int", nullable: true),
                    VoiceChatId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidate_VoiceChat_VoiceChatId",
                        column: x => x.VoiceChatId,
                        principalTable: "VoiceChat",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Candidate_VoiceChat_VoiceChatId1",
                        column: x => x.VoiceChatId1,
                        principalTable: "VoiceChat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OffersAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Offer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoiceChatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffersAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffersAnswers_VoiceChat_VoiceChatId",
                        column: x => x.VoiceChatId,
                        principalTable: "VoiceChat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserGroupId = table.Column<int>(type: "int", nullable: true),
                    VoiceChatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Groups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_VoiceChat_VoiceChatId",
                        column: x => x.VoiceChatId,
                        principalTable: "VoiceChat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    TextMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_VoiceChatId",
                table: "Candidate",
                column: "VoiceChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_VoiceChatId1",
                table: "Candidate",
                column: "VoiceChatId1");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_UserGroupId",
                table: "Chat",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatId",
                table: "Message",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_OwnerId",
                table: "Message",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OffersAnswers_VoiceChatId",
                table: "OffersAnswers",
                column: "VoiceChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGroupId",
                table: "Users",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_VoiceChatId",
                table: "Users",
                column: "VoiceChatId");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceChat_UserGroupId",
                table: "VoiceChat",
                column: "UserGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "OffersAnswers");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VoiceChat");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
