﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using survey_quiz_app.Data;

#nullable disable

namespace survey_quiz_app.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20231010064641_Add NightMode Fixed Table For User  migration")]
    partial class AddNightModeFixedTableForUsermigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("survey_quiz_app.Models.CategoryList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("CategoryList");
                });

            modelBuilder.Entity("survey_quiz_app.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AnswersString")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ChoicesString")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("QuestionBankId")
                        .HasColumnType("int");

                    b.Property<string>("QuestionName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionBankId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryListId")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateTimeNow")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EnableStatus")
                        .HasColumnType("bit");

                    b.Property<string>("EndDate")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Owner")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("StartDate")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Status")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("SurveyCode")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("SurveyName")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Timer")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryListId");

                    b.HasIndex("SurveyCode")
                        .IsUnique();

                    b.ToTable("QuestionBanks");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBankInteract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("QuestionBankId")
                        .HasColumnType("int");

                    b.Property<double?>("ResultScores")
                        .HasColumnType("float");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionBankId");

                    b.HasIndex("UserId");

                    b.ToTable("QuestionBankInteracts");
                });

            modelBuilder.Entity("survey_quiz_app.Models.ResultShow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("OnAnswersString")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("QuestionBankInteractId")
                        .HasColumnType("int");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("int");

                    b.Property<double?>("ResultScore")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("QuestionBankInteractId");

                    b.HasIndex("QuestionId");

                    b.ToTable("ResultShows");
                });

            modelBuilder.Entity("survey_quiz_app.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Permission")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("survey_quiz_app.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("survey_quiz_app.Models.Question", b =>
                {
                    b.HasOne("survey_quiz_app.Models.QuestionBank", "QuestionBank")
                        .WithMany("Questions")
                        .HasForeignKey("QuestionBankId");

                    b.Navigation("QuestionBank");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBank", b =>
                {
                    b.HasOne("survey_quiz_app.Models.CategoryList", "CategoryList")
                        .WithMany("QuestionBanks")
                        .HasForeignKey("CategoryListId");

                    b.Navigation("CategoryList");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBankInteract", b =>
                {
                    b.HasOne("survey_quiz_app.Models.QuestionBank", "QuestionBank")
                        .WithMany("QuestionBankInteract")
                        .HasForeignKey("QuestionBankId");

                    b.HasOne("survey_quiz_app.Models.User", "User")
                        .WithMany("QuestionBankInteracts")
                        .HasForeignKey("UserId");

                    b.Navigation("QuestionBank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("survey_quiz_app.Models.ResultShow", b =>
                {
                    b.HasOne("survey_quiz_app.Models.QuestionBankInteract", "QuestionBankInteract")
                        .WithMany("ResultShows")
                        .HasForeignKey("QuestionBankInteractId");

                    b.HasOne("survey_quiz_app.Models.Question", "Question")
                        .WithMany("ResultShows")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Question");

                    b.Navigation("QuestionBankInteract");
                });

            modelBuilder.Entity("survey_quiz_app.Models.User", b =>
                {
                    b.HasOne("survey_quiz_app.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("survey_quiz_app.Models.CategoryList", b =>
                {
                    b.Navigation("QuestionBanks");
                });

            modelBuilder.Entity("survey_quiz_app.Models.Question", b =>
                {
                    b.Navigation("ResultShows");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBank", b =>
                {
                    b.Navigation("QuestionBankInteract");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBankInteract", b =>
                {
                    b.Navigation("ResultShows");
                });

            modelBuilder.Entity("survey_quiz_app.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("survey_quiz_app.Models.User", b =>
                {
                    b.Navigation("QuestionBankInteracts");
                });
#pragma warning restore 612, 618
        }
    }
}
