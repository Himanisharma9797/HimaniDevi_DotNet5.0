﻿// <auto-generated />
using System;
using Employee.Service.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Employee.Service.DAL.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    [Migration("20220712054750_MOdelchange")]
    partial class MOdelchange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Employee.Service.DAL.Account.UserInfo", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("Employee.Service.DAL.Task.TaskDetails", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CompletedHours")
                        .HasColumnType("float");

                    b.Property<DateTime>("EndDuration")
                        .HasColumnType("datetime2");

                    b.Property<double>("EstimatedHours")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDuration")
                        .HasColumnType("datetime2");

                    b.Property<string>("TitleOfTask")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TaskId");

                    b.HasIndex("UserId");

                    b.ToTable("taskDetails");
                });

            modelBuilder.Entity("Employee.Service.DAL.Task.TaskDetails", b =>
                {
                    b.HasOne("Employee.Service.DAL.Account.UserInfo", "userInfo")
                        .WithMany("taskDetails")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("userInfo");
                });

            modelBuilder.Entity("Employee.Service.DAL.Account.UserInfo", b =>
                {
                    b.Navigation("taskDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
