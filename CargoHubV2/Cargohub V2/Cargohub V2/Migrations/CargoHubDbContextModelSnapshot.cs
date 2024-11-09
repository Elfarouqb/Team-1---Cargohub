﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Cargohub_V2.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cargohub_V2.Migrations
{
    [DbContext(typeof(CargoHubDbContext))]
    partial class CargoHubDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Cargohub_V2.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("text");

                    b.Property<string>("ContactName")
                        .HasColumnType("text");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Province")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ItemId")
                        .HasColumnType("text");

                    b.Property<string>("ItemReference")
                        .HasColumnType("text");

                    b.Property<List<int>>("Locations")
                        .HasColumnType("integer[]");

                    b.Property<int>("TotalAllocated")
                        .HasColumnType("integer");

                    b.Property<int>("TotalAvailable")
                        .HasColumnType("integer");

                    b.Property<int>("TotalOnHand")
                        .HasColumnType("integer");

                    b.Property<int>("TotalOrdered")
                        .HasColumnType("integer");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.Property<int>("totalExpected")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("CommodityCode")
                        .HasColumnType("text");

                    b.Property<string>("Created_at")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("ItemGroupId")
                        .HasColumnType("integer");

                    b.Property<int?>("ItemLineId")
                        .HasColumnType("integer");

                    b.Property<int?>("ItemTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("ModelNumber")
                        .HasColumnType("text");

                    b.Property<int>("PackOrderQuantity")
                        .HasColumnType("integer");

                    b.Property<string>("Shortescription")
                        .HasColumnType("text");

                    b.Property<string>("SupplierCode")
                        .HasColumnType("text");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("integer");

                    b.Property<string>("SupplierPartNumber")
                        .HasColumnType("text");

                    b.Property<int>("Supplier_id")
                        .HasColumnType("integer");

                    b.Property<string>("UId")
                        .HasColumnType("text");

                    b.Property<int>("UnitOrderQuantity")
                        .HasColumnType("integer");

                    b.Property<int>("UnitPurchaseQuantity")
                        .HasColumnType("integer");

                    b.Property<string>("UpcCode")
                        .HasColumnType("text");

                    b.Property<string>("Updated_at")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ItemGroupId");

                    b.HasIndex("ItemLineId");

                    b.HasIndex("ItemTypeId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Item_Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Items_Groups");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Item_Line", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Created_at")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Updated_at")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Items_Lines");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Item_Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Created_at")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Updated_at")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Items_Types");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BillTo")
                        .HasColumnType("text");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("OrderDate")
                        .HasColumnType("text");

                    b.Property<string>("Order_status")
                        .HasColumnType("text");

                    b.Property<string>("PickingNotes")
                        .HasColumnType("text");

                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<string>("Reference_extra")
                        .HasColumnType("text");

                    b.Property<string>("RequestDate")
                        .HasColumnType("text");

                    b.Property<string>("ShipTo")
                        .HasColumnType("text");

                    b.Property<int>("ShipmentId")
                        .HasColumnType("integer");

                    b.Property<string>("ShippingNotes")
                        .HasColumnType("text");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("double precision");

                    b.Property<double>("TotalDiscount")
                        .HasColumnType("double precision");

                    b.Property<double>("TotalSurcharge")
                        .HasColumnType("double precision");

                    b.Property<double>("TotalTax")
                        .HasColumnType("double precision");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Shipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CarrierCode")
                        .HasColumnType("text");

                    b.Property<string>("CarrierDescription")
                        .HasColumnType("text");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("OrderDate")
                        .HasColumnType("text");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<string>("PaymentType")
                        .HasColumnType("text");

                    b.Property<string>("RequestDate")
                        .HasColumnType("text");

                    b.Property<string>("ServiceCode")
                        .HasColumnType("text");

                    b.Property<string>("ShipmentDate")
                        .HasColumnType("text");

                    b.Property<string>("ShipmentStatus")
                        .HasColumnType("text");

                    b.Property<string>("ShipmentType")
                        .HasColumnType("text");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.Property<int>("TotalPackageCount")
                        .HasColumnType("integer");

                    b.Property<double>("TotalPackageWeight")
                        .HasColumnType("double precision");

                    b.Property<string>("TransferMode")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ItemId")
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("StockType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.HasKey("Id");

                    b.ToTable("Stocks", (string)null);

                    b.HasDiscriminator<string>("StockType").HasValue("Stock");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Cargohub_V2.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("AddressExtra")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("ContactName")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Province")
                        .HasColumnType("text");

                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<int>("TransferFrom")
                        .HasColumnType("integer");

                    b.Property<string>("TransferStatus")
                        .HasColumnType("text");

                    b.Property<int>("TransferTo")
                        .HasColumnType("integer");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Province")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedAt")
                        .HasColumnType("text");

                    b.Property<string>("Zip")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("Cargohub_V2.Models.OrderStock", b =>
                {
                    b.HasBaseType("Cargohub_V2.Models.Stock");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.HasIndex("OrderId");

                    b.HasDiscriminator().HasValue("Order");
                });

            modelBuilder.Entity("Cargohub_V2.Models.ShipmentStock", b =>
                {
                    b.HasBaseType("Cargohub_V2.Models.Stock");

                    b.Property<int>("ShipmentId")
                        .HasColumnType("integer");

                    b.HasIndex("ShipmentId");

                    b.HasDiscriminator().HasValue("Shipment");
                });

            modelBuilder.Entity("Cargohub_V2.Models.TransferStock", b =>
                {
                    b.HasBaseType("Cargohub_V2.Models.Stock");

                    b.Property<int>("TransferId")
                        .HasColumnType("integer");

                    b.HasIndex("TransferId");

                    b.HasDiscriminator().HasValue("Transfer");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Item", b =>
                {
                    b.HasOne("Cargohub_V2.Models.Item_Group", "ItemGroup")
                        .WithMany()
                        .HasForeignKey("ItemGroupId");

                    b.HasOne("Cargohub_V2.Models.Item_Line", "ItemLine")
                        .WithMany()
                        .HasForeignKey("ItemLineId");

                    b.HasOne("Cargohub_V2.Models.Item_Type", "ItemType")
                        .WithMany()
                        .HasForeignKey("ItemTypeId");

                    b.HasOne("Cargohub_V2.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId");

                    b.Navigation("ItemGroup");

                    b.Navigation("ItemLine");

                    b.Navigation("ItemType");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Warehouse", b =>
                {
                    b.OwnsOne("Cargohub_V2.Models.Contact", "Contact", b1 =>
                        {
                            b1.Property<int>("WarehouseId")
                                .HasColumnType("integer");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("WarehouseId");

                            b1.ToTable("Warehouses");

                            b1.WithOwner()
                                .HasForeignKey("WarehouseId");
                        });

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("Cargohub_V2.Models.OrderStock", b =>
                {
                    b.HasOne("Cargohub_V2.Models.Order", "Order")
                        .WithMany("Stocks")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Cargohub_V2.Models.ShipmentStock", b =>
                {
                    b.HasOne("Cargohub_V2.Models.Shipment", "Shipment")
                        .WithMany("Stocks")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("Cargohub_V2.Models.TransferStock", b =>
                {
                    b.HasOne("Cargohub_V2.Models.Transfer", "Transfer")
                        .WithMany("Stocks")
                        .HasForeignKey("TransferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transfer");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Order", b =>
                {
                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Shipment", b =>
                {
                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("Cargohub_V2.Models.Transfer", b =>
                {
                    b.Navigation("Stocks");
                });
#pragma warning restore 612, 618
        }
    }
}
