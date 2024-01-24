﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notifications.Context;

#nullable disable

namespace Notifications.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    [Migration("20240124094508_initNotification")]
    partial class initNotification
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Notifications.Entities.NotificationSeverity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("NotificationSeverities");
                });

            modelBuilder.Entity("Notifications.Entities.NotificationTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EnContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnTitle")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NotificationCode")
                        .HasColumnType("int");

                    b.Property<int>("NotificationSeverityId")
                        .HasColumnType("int");

                    b.Property<int>("NotificationTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotificationSeverityId");

                    b.HasIndex("NotificationTypeId");

                    b.ToTable("NotificationTemplates");
                });

            modelBuilder.Entity("Notifications.Entities.NotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EnName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("NotificationSeverityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotificationSeverityId");

                    b.ToTable("NotificationTypes");
                });

            modelBuilder.Entity("Notifications.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SecondName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ThirdName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Notifications.Entities.NotificationTemplate", b =>
                {
                    b.HasOne("Notifications.Entities.NotificationSeverity", "NotificationSeverity")
                        .WithMany("NotificationTemplates")
                        .HasForeignKey("NotificationSeverityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Notifications.Entities.NotificationType", "NotificationType")
                        .WithMany("NotificationTemplates")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NotificationSeverity");

                    b.Navigation("NotificationType");
                });

            modelBuilder.Entity("Notifications.Entities.NotificationType", b =>
                {
                    b.HasOne("Notifications.Entities.NotificationSeverity", null)
                        .WithMany("NotificationTypes")
                        .HasForeignKey("NotificationSeverityId");
                });

            modelBuilder.Entity("Notifications.Entities.NotificationSeverity", b =>
                {
                    b.Navigation("NotificationTemplates");

                    b.Navigation("NotificationTypes");
                });

            modelBuilder.Entity("Notifications.Entities.NotificationType", b =>
                {
                    b.Navigation("NotificationTemplates");
                });
#pragma warning restore 612, 618
        }
    }
}
