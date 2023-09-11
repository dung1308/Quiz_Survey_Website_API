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
    [Migration("20230911015513_Initial migration")]
    partial class Initialmigration
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
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ChoicesString")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("OnAnswersString")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid?>("QuestionBankId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QuestionName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionBankId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBank", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Category")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("CategoryListId")
                        .HasColumnType("int");

                    b.Property<bool>("EnableStatus")
                        .HasColumnType("bit");

                    b.Property<string>("EndDate")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Owner")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("QuestionBankInteractId")
                        .HasColumnType("int");

                    b.Property<string>("StartDate")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SurveyCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SurveyName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Timer")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryListId");

                    b.HasIndex("QuestionBankInteractId");

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

                    b.Property<double?>("ResultScores")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("QuestionBankInteracts");
                });

            modelBuilder.Entity("survey_quiz_app.Models.ResultShow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("OnAnswer")
                        .HasColumnType("float");

                    b.Property<int?>("QuestionBankInteractId")
                        .HasColumnType("int");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("int");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("QuestionBankInteractId")
                        .HasColumnType("int");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionBankInteractId")
                        .IsUnique()
                        .HasFilter("[QuestionBankInteractId] IS NOT NULL");

                    b.HasIndex("RoleId");

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

                    b.HasOne("survey_quiz_app.Models.QuestionBankInteract", null)
                        .WithMany("QuestionBanks")
                        .HasForeignKey("QuestionBankInteractId");

                    b.Navigation("CategoryList");
                });

            modelBuilder.Entity("survey_quiz_app.Models.ResultShow", b =>
                {
                    b.HasOne("survey_quiz_app.Models.QuestionBankInteract", null)
                        .WithMany("ResultShows")
                        .HasForeignKey("QuestionBankInteractId");

                    b.HasOne("survey_quiz_app.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("survey_quiz_app.Models.User", b =>
                {
                    b.HasOne("survey_quiz_app.Models.QuestionBankInteract", "QuestionBankInteract")
                        .WithOne("User")
                        .HasForeignKey("survey_quiz_app.Models.User", "QuestionBankInteractId");

                    b.HasOne("survey_quiz_app.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("QuestionBankInteract");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("survey_quiz_app.Models.CategoryList", b =>
                {
                    b.Navigation("QuestionBanks");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBank", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("survey_quiz_app.Models.QuestionBankInteract", b =>
                {
                    b.Navigation("QuestionBanks");

                    b.Navigation("ResultShows");

                    b.Navigation("User");
                });

            modelBuilder.Entity("survey_quiz_app.Models.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
