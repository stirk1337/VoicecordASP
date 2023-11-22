﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Voicecord.Data;

#nullable disable

namespace Voicecord.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Voicecord.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserGroupId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VoiceChatId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserGroupId");

                    b.HasIndex("VoiceChatId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Voicecord.Models.Candidate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CandidateString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SdpMLine")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SdpMLineIndex")
                        .HasColumnType("int");

                    b.Property<string>("UsernameFragment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VoiceChatId")
                        .HasColumnType("int");

                    b.Property<int?>("VoiceChatId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VoiceChatId");

                    b.HasIndex("VoiceChatId1");

                    b.ToTable("Candidate");
                });

            modelBuilder.Entity("Voicecord.Models.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("UserGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserGroupId");

                    b.ToTable("Chat");
                });

            modelBuilder.Entity("Voicecord.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ChatId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("TextMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Voicecord.Models.OffersAnswers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Offer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VoiceChatId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VoiceChatId");

                    b.ToTable("OffersAnswers");
                });

            modelBuilder.Entity("Voicecord.Models.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LinkImageGroup")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Voicecord.Models.VoiceChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("UserGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserGroupId");

                    b.ToTable("VoiceChat");
                });

            modelBuilder.Entity("Voicecord.Models.ApplicationUser", b =>
                {
                    b.HasOne("Voicecord.Models.UserGroup", null)
                        .WithMany("Users")
                        .HasForeignKey("UserGroupId");

                    b.HasOne("Voicecord.Models.VoiceChat", null)
                        .WithMany("Users")
                        .HasForeignKey("VoiceChatId");
                });

            modelBuilder.Entity("Voicecord.Models.Candidate", b =>
                {
                    b.HasOne("Voicecord.Models.VoiceChat", null)
                        .WithMany("AnswerCandidates")
                        .HasForeignKey("VoiceChatId");

                    b.HasOne("Voicecord.Models.VoiceChat", null)
                        .WithMany("OfferCandidates")
                        .HasForeignKey("VoiceChatId1");
                });

            modelBuilder.Entity("Voicecord.Models.Chat", b =>
                {
                    b.HasOne("Voicecord.Models.UserGroup", null)
                        .WithMany("Chats")
                        .HasForeignKey("UserGroupId");
                });

            modelBuilder.Entity("Voicecord.Models.Message", b =>
                {
                    b.HasOne("Voicecord.Models.Chat", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatId");

                    b.HasOne("Voicecord.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Voicecord.Models.OffersAnswers", b =>
                {
                    b.HasOne("Voicecord.Models.VoiceChat", null)
                        .WithMany("OffersAns")
                        .HasForeignKey("VoiceChatId");
                });

            modelBuilder.Entity("Voicecord.Models.VoiceChat", b =>
                {
                    b.HasOne("Voicecord.Models.UserGroup", null)
                        .WithMany("Voices")
                        .HasForeignKey("UserGroupId");
                });

            modelBuilder.Entity("Voicecord.Models.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Voicecord.Models.UserGroup", b =>
                {
                    b.Navigation("Chats");

                    b.Navigation("Users");

                    b.Navigation("Voices");
                });

            modelBuilder.Entity("Voicecord.Models.VoiceChat", b =>
                {
                    b.Navigation("AnswerCandidates");

                    b.Navigation("OfferCandidates");

                    b.Navigation("OffersAns");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
