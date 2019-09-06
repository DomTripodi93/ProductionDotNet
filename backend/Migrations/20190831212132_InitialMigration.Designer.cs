﻿// <auto-generated />
using System;
using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190831212132_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("BackEnd.Models.ChangeLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChangeType");

                    b.Property<string>("ChangedId");

                    b.Property<string>("ChangedModel");

                    b.Property<string>("OldValues");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("ChangeLogs");
                });

            modelBuilder.Entity("BackEnd.Models.Hourly", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CounterQuantity");

                    b.Property<string>("Job");

                    b.Property<string>("Machine");

                    b.Property<string>("Quantity");

                    b.Property<DateTime>("Time");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Hourlys");
                });

            modelBuilder.Entity("BackEnd.Models.Mach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CurrentJob");

                    b.Property<string>("Machine");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("BackEnd.Models.Part", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bars");

                    b.Property<string>("CutOff");

                    b.Property<string>("CycleTime");

                    b.Property<string>("HeatLot");

                    b.Property<string>("Job");

                    b.Property<string>("Machine");

                    b.Property<string>("MainFacing");

                    b.Property<string>("Oal");

                    b.Property<string>("OrderQuantity");

                    b.Property<string>("PartNumber");

                    b.Property<string>("PossibleQuantity");

                    b.Property<string>("RemainingQuantity");

                    b.Property<string>("SubFacing");

                    b.Property<string>("WeightLength");

                    b.Property<string>("WeightQuantity");

                    b.Property<string>("WeightRecieved");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("BackEnd.Models.Production", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<bool>("InQuestion");

                    b.Property<string>("Job");

                    b.Property<string>("Machine");

                    b.Property<string>("PartNumber");

                    b.Property<string>("Quantity");

                    b.Property<string>("Shift");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Production");
                });

            modelBuilder.Entity("BackEnd.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsNew");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("BackEnd.Models.StartTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int?>("MachId");

                    b.Property<string>("Machine");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("MachId");

                    b.HasIndex("userId");

                    b.ToTable("StartTimes");
                });

            modelBuilder.Entity("BackEnd.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackEnd.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("BackEnd.Models.ChangeLog", b =>
                {
                    b.HasOne("BackEnd.Models.User", "User")
                        .WithMany("ChangeLog")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Models.Hourly", b =>
                {
                    b.HasOne("BackEnd.Models.User", "User")
                        .WithMany("Hourly")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Models.Mach", b =>
                {
                    b.HasOne("BackEnd.Models.User", "User")
                        .WithMany("Machine")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Models.Part", b =>
                {
                    b.HasOne("BackEnd.Models.User", "User")
                        .WithMany("Part")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Models.Production", b =>
                {
                    b.HasOne("BackEnd.Models.User", "User")
                        .WithMany("Production")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Models.Settings", b =>
                {
                    b.HasOne("BackEnd.Models.User", "User")
                        .WithOne("Settings")
                        .HasForeignKey("BackEnd.Models.Settings", "userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BackEnd.Models.StartTime", b =>
                {
                    b.HasOne("BackEnd.Models.Mach")
                        .WithMany("StartTimes")
                        .HasForeignKey("MachId");

                    b.HasOne("BackEnd.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}