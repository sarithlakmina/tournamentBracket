﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

#nullable disable

namespace TournamentBracket.BackEnd.V1.Persistence.Migrations
{
    [DbContext(typeof(TournamentBracketDbContext))]
    [Migration("20231017202229_Update_Tables_RefactoringNamePropertyToConsistent")]
    partial class Update_Tables_RefactoringNamePropertyToConsistent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.Match", b =>
                {
                    b.Property<Guid>("MatchID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AwayTeamID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HomeTeamID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsMatchCompleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("TournamentID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WinningTeamID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MatchID");

                    b.HasIndex("TournamentID");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.MatchCategory", b =>
                {
                    b.Property<Guid>("MatchCategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<Guid>("TournamentID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MatchCategoryID");

                    b.HasIndex("TournamentID");

                    b.ToTable("MatchCategories");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.MatchMatchCategoryMap", b =>
                {
                    b.Property<Guid>("MatchMatchCategoryMapID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchCategoryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TournamentID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MatchMatchCategoryMapID");

                    b.HasIndex("MatchCategoryID");

                    b.HasIndex("MatchID");

                    b.HasIndex("MatchMatchCategoryMapID");

                    b.HasIndex("TournamentID");

                    b.ToTable("MatchMatchCategoryMaps");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.Team", b =>
                {
                    b.Property<Guid>("TeamID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Seed")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamID");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.Tournament", b =>
                {
                    b.Property<Guid>("TournamentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<Guid?>("SecondPlace")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ThirdPlace")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Winner")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TournamentID");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.TournamentMatchMap", b =>
                {
                    b.Property<Guid>("TournamentMatchMapID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("MatchID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TournamentID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TournamentMatchMapID");

                    b.HasIndex("MatchID");

                    b.HasIndex("TournamentID");

                    b.HasIndex("TournamentMatchMapID");

                    b.ToTable("TournamentMatchMaps");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.TournamentTeamMap", b =>
                {
                    b.Property<Guid>("TournamentTeamMapID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("TeamID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TournamentID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TournamentTeamMapID");

                    b.HasIndex("TeamID");

                    b.HasIndex("TournamentID");

                    b.HasIndex("TournamentTeamMapID");

                    b.ToTable("TournamentTeamMaps");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.Match", b =>
                {
                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.MatchCategory", b =>
                {
                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.MatchMatchCategoryMap", b =>
                {
                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.MatchCategory", "MatchCategory")
                        .WithMany("MatchMatchCategoryMaps")
                        .HasForeignKey("MatchCategoryID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Match", "Match")
                        .WithMany("MatchMatchCategoryMaps")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Tournament", "Tournament")
                        .WithMany("MatchMatchCategoryMaps")
                        .HasForeignKey("TournamentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("MatchCategory");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.TournamentMatchMap", b =>
                {
                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Match", "Match")
                        .WithMany("TournamentMatchMaps")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Tournament", "Tournament")
                        .WithMany("TournamentMatchMaps")
                        .HasForeignKey("TournamentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.TournamentTeamMap", b =>
                {
                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Team", "Team")
                        .WithMany("TournamentTeamMaps")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TournamentBracket.BackEnd.V1.Common.Entity.Tournament", "Tournament")
                        .WithMany("TournamentTeamMaps")
                        .HasForeignKey("TournamentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.Match", b =>
                {
                    b.Navigation("MatchMatchCategoryMaps");

                    b.Navigation("TournamentMatchMaps");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.MatchCategory", b =>
                {
                    b.Navigation("MatchMatchCategoryMaps");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.Team", b =>
                {
                    b.Navigation("TournamentTeamMaps");
                });

            modelBuilder.Entity("TournamentBracket.BackEnd.V1.Common.Entity.Tournament", b =>
                {
                    b.Navigation("MatchMatchCategoryMaps");

                    b.Navigation("TournamentMatchMaps");

                    b.Navigation("TournamentTeamMaps");
                });
#pragma warning restore 612, 618
        }
    }
}
