﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ScreenshotMonitor.Data.Context;

#nullable disable

namespace ScreenshotMonitor.Data.Migrations
{
    [DbContext(typeof(SmDbContext))]
    [Migration("20250209150024_AddGetterSetterTest01")]
    partial class AddGetterSetterTest01
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Project", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AdminId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.ProjectEmployee", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("text");

                    b.Property<string>("ProjectId")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("TotalActiveTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectEmployees");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Screenshot", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CapturedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SessionId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Screenshots");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("ActiveDuration")
                        .HasColumnType("interval");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ProjectId")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.SessionBackgroundApp", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SessionId")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("TotalUsageTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("SessionBackgroundApps");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.SessionForegroundApp", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SessionId")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("TotalUsageTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("SessionForegroundApps");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Designation")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("TotalOnlineTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Project", b =>
                {
                    b.HasOne("ScreenshotMonitor.Data.Entities.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.ProjectEmployee", b =>
                {
                    b.HasOne("ScreenshotMonitor.Data.Entities.User", "Employee")
                        .WithMany("ProjectEmployees")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScreenshotMonitor.Data.Entities.Project", "Project")
                        .WithMany("ProjectEmployees")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Employee");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Screenshot", b =>
                {
                    b.HasOne("ScreenshotMonitor.Data.Entities.Session", "Session")
                        .WithMany("Screenshots")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Session");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Session", b =>
                {
                    b.HasOne("ScreenshotMonitor.Data.Entities.User", "Employee")
                        .WithMany("Sessions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScreenshotMonitor.Data.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Employee");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.SessionBackgroundApp", b =>
                {
                    b.HasOne("ScreenshotMonitor.Data.Entities.Session", "Session")
                        .WithMany("BackgroundApps")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Session");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.SessionForegroundApp", b =>
                {
                    b.HasOne("ScreenshotMonitor.Data.Entities.Session", "Session")
                        .WithMany("ForegroundApps")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Session");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Project", b =>
                {
                    b.Navigation("ProjectEmployees");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.Session", b =>
                {
                    b.Navigation("BackgroundApps");

                    b.Navigation("ForegroundApps");

                    b.Navigation("Screenshots");
                });

            modelBuilder.Entity("ScreenshotMonitor.Data.Entities.User", b =>
                {
                    b.Navigation("ProjectEmployees");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
