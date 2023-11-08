﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UlfenDk.EnergyAssistant.Database;

#nullable disable

namespace UlfenDk.EnergyAssistant.Migrations
{
    [DbContext(typeof(EnergyAssistantContext))]
    partial class EnergyAssistantContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.CarnotSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApiKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CarnotSettings");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            ApiKey = "",
                            IsEnabled = false,
                            User = ""
                        });
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.DailyInterval", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("MonthPeriodId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("Start")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MonthPeriodId");

                    b.ToTable("DailyIntervals");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.ElOverblikSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApiToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ElectricHeatingMeteringPointId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MeteringPointId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ElOverblikSettings");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            ApiToken = "",
                            ElectricHeatingMeteringPointId = "",
                            IsEnabled = false,
                            MeteringPointId = ""
                        });
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.EnergiDataServiceSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EnergiDataServiceSettings");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IsEnabled = true
                        });
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.EnergyUsageRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("FeePeriodId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Hour")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeePeriodId");

                    b.HasIndex("Hour");

                    b.ToTable("Usages");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.Fee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<long?>("DailyIntervalId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("MonthPeriodId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("MonthPeriodId1")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("MonthPeriodId2")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("YearPeriodId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DailyIntervalId");

                    b.HasIndex("MonthPeriodId");

                    b.HasIndex("MonthPeriodId1");

                    b.HasIndex("MonthPeriodId2");

                    b.HasIndex("YearPeriodId");

                    b.ToTable("Fees");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.FeePerUnit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AmountPerUnit")
                        .HasColumnType("TEXT");

                    b.Property<int>("Application")
                        .HasColumnType("INTEGER");

                    b.Property<long>("FeePerUnitId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("HourlyFeePeriodId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HourlyFeePeriodId");

                    b.ToTable("FeePerUnit");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.FeePeriod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("End")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasElectricHeating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("Start")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FeePeriod");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.FixedFee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<long>("FeePeriodId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeePeriodId");

                    b.ToTable("FixedFee");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.GeneralSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasElectricHeating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GeneralSettings");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            HasElectricHeating = false,
                            Region = "dk2"
                        });
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.HomeAssistantSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("HomeAssistantSettings");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IsEnabled = false,
                            Token = "",
                            Url = "http://supervisor/core"
                        });
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.HourlyFeePeriod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("FeePeriodId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("Start")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeePeriodId");

                    b.ToTable("HourlyFeePeriod");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.MonthPeriod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("From")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("To")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MonthPeriods");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.NordPoolSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("NordPoolSettings");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            IsEnabled = false
                        });
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.SpotPriceRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("FeePeriodId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Hour")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPrediction")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("RawPrice")
                        .HasColumnType("TEXT");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeePeriodId");

                    b.HasIndex("Hour");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.YearPeriod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Vat")
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("YearPeriods");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.DailyInterval", b =>
                {
                    b.HasOne("UlfenDk.EnergyAssistant.Database.MonthPeriod", null)
                        .WithMany("Daily")
                        .HasForeignKey("MonthPeriodId");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.EnergyUsageRecord", b =>
                {
                    b.HasOne("UlfenDk.EnergyAssistant.Database.FeePeriod", "FeePeriod")
                        .WithMany()
                        .HasForeignKey("FeePeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FeePeriod");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.Fee", b =>
                {
                    b.HasOne("UlfenDk.EnergyAssistant.Database.DailyInterval", null)
                        .WithMany("UnitFees")
                        .HasForeignKey("DailyIntervalId");

                    b.HasOne("UlfenDk.EnergyAssistant.Database.MonthPeriod", null)
                        .WithMany("MonthlyCosts")
                        .HasForeignKey("MonthPeriodId");

                    b.HasOne("UlfenDk.EnergyAssistant.Database.MonthPeriod", null)
                        .WithMany("ReducedFeesPerUnit")
                        .HasForeignKey("MonthPeriodId1");

                    b.HasOne("UlfenDk.EnergyAssistant.Database.MonthPeriod", null)
                        .WithMany("RegularFeesPerUnit")
                        .HasForeignKey("MonthPeriodId2");

                    b.HasOne("UlfenDk.EnergyAssistant.Database.YearPeriod", null)
                        .WithMany("YearlyCosts")
                        .HasForeignKey("YearPeriodId");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.FeePerUnit", b =>
                {
                    b.HasOne("UlfenDk.EnergyAssistant.Database.HourlyFeePeriod", null)
                        .WithMany("Fees")
                        .HasForeignKey("HourlyFeePeriodId");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.FixedFee", b =>
                {
                    b.HasOne("UlfenDk.EnergyAssistant.Database.FeePeriod", null)
                        .WithMany("MonthlyFees")
                        .HasForeignKey("FeePeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.HourlyFeePeriod", b =>
                {
                    b.HasOne("UlfenDk.EnergyAssistant.Database.FeePeriod", null)
                        .WithMany("HourlyFees")
                        .HasForeignKey("FeePeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.SpotPriceRecord", b =>
                {
                    b.HasOne("UlfenDk.EnergyAssistant.Database.FeePeriod", "FeePeriod")
                        .WithMany()
                        .HasForeignKey("FeePeriodId");

                    b.Navigation("FeePeriod");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.DailyInterval", b =>
                {
                    b.Navigation("UnitFees");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.FeePeriod", b =>
                {
                    b.Navigation("HourlyFees");

                    b.Navigation("MonthlyFees");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.HourlyFeePeriod", b =>
                {
                    b.Navigation("Fees");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.MonthPeriod", b =>
                {
                    b.Navigation("Daily");

                    b.Navigation("MonthlyCosts");

                    b.Navigation("ReducedFeesPerUnit");

                    b.Navigation("RegularFeesPerUnit");
                });

            modelBuilder.Entity("UlfenDk.EnergyAssistant.Database.YearPeriod", b =>
                {
                    b.Navigation("YearlyCosts");
                });
#pragma warning restore 612, 618
        }
    }
}
