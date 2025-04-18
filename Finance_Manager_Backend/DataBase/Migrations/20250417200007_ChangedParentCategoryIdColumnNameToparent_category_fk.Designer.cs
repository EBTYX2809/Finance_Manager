﻿// <auto-generated />
using System;
using Finance_Manager_Backend.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Finance_Manager_Backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250417200007_ChangedParentCategoryIdColumnNameToparent_category_fk")]
    partial class ChangedParentCategoryIdColumnNameToparent_category_fk
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ColorForBackground")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)")
                        .HasColumnName("background_color");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("icon_path");

                    b.Property<bool>("IsIncome")
                        .HasColumnType("bit")
                        .HasColumnName("is_income");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int")
                        .HasColumnName("parent_category_fk");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.Saving", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("CurrentAmount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(10,2)")
                        .HasDefaultValue(0m)
                        .HasColumnName("current_amount");

                    b.Property<decimal>("Goal")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("goal");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_fk");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("savings", (string)null);
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_fk");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("date");

                    b.Property<int?>("InnerCategoryId")
                        .HasColumnType("int")
                        .HasColumnName("inner_category_fk");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("price");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_fk");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("InnerCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("balance");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("password_hash");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("salt");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.Category", b =>
                {
                    b.HasOne("Finance_Manager_Backend.BusinessLogic.Models.Category", "ParentCategory")
                        .WithMany("InnerCategories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.Saving", b =>
                {
                    b.HasOne("Finance_Manager_Backend.BusinessLogic.Models.User", "User")
                        .WithMany("Savings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.Transaction", b =>
                {
                    b.HasOne("Finance_Manager_Backend.BusinessLogic.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Finance_Manager_Backend.BusinessLogic.Models.Category", "InnerCategory")
                        .WithMany()
                        .HasForeignKey("InnerCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Finance_Manager_Backend.BusinessLogic.Models.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("InnerCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.Category", b =>
                {
                    b.Navigation("InnerCategories");
                });

            modelBuilder.Entity("Finance_Manager_Backend.BusinessLogic.Models.User", b =>
                {
                    b.Navigation("Savings");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
