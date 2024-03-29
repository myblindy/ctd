﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.DBModel;

namespace Services.Migrations
{
    [DbContext(typeof(JobsContext))]
    [Migration("20190815020525_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0-preview7.19362.6");

            modelBuilder.Entity("Services.DBModel.Job", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("JobId");

                    b.ToTable("Jobs");

                    b.HasData(
                        new
                        {
                            JobId = 1,
                            Name = "Job 1"
                        },
                        new
                        {
                            JobId = 2,
                            Name = "Job 2"
                        },
                        new
                        {
                            JobId = 3,
                            Name = "Job 3"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
