﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Backend.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.WebApi.Infrastructure.Persistence.Migrations.Sensor
{
    [DbContext(typeof(SensorServiceContext))]
    partial class SensorServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "hstore");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.WebApi.Domain.Models.AutomatedAction", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ProductID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("Backend.WebApi.Domain.Models.Product", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<float>("RegularWeight")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Backend.WebApi.Domain.Models.Sensor", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid?>("ProductID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("Sensors", (string)null);
                });

            modelBuilder.Entity("Backend.WebApi.Domain.Models.CallWebServiceAction", b =>
                {
                    b.HasBaseType("Backend.WebApi.Domain.Models.AutomatedAction");

                    b.Property<Dictionary<string, string>>("Parameters")
                        .IsRequired()
                        .HasColumnType("hstore");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("CallWebServiceActions");
                });

            modelBuilder.Entity("Backend.WebApi.Domain.Models.PushNotificationAction", b =>
                {
                    b.HasBaseType("Backend.WebApi.Domain.Models.AutomatedAction");

                    b.Property<string>("CustomText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("PushNotificationActions");
                });

            modelBuilder.Entity("Backend.WebApi.Domain.Models.AutomatedAction", b =>
                {
                    b.HasOne("Backend.WebApi.Domain.Models.Product", null)
                        .WithMany("AutomatedActions")
                        .HasForeignKey("ProductID");
                });

            modelBuilder.Entity("Backend.WebApi.Domain.Models.Sensor", b =>
                {
                    b.HasOne("Backend.WebApi.Domain.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Backend.WebApi.Domain.Models.Product", b =>
                {
                    b.Navigation("AutomatedActions");
                });
#pragma warning restore 612, 618
        }
    }
}
