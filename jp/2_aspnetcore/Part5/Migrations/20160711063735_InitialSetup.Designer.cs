using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AspNetCorehandson;
using AspNetCorehandson.Models;

namespace AspNetCorehandson.Migrations
{
    [DbContext(typeof(PubsEntities))]
    [Migration("20160711063735_InitialSetup")]
    partial class InitialSetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("AspNetCorehandson.Author", b =>
                {
                    b.Property<string>("AuthorId")
                        .HasColumnName("au_id")
                        .HasAnnotation("MaxLength", 11);

                    b.Property<string>("Address")
                        .HasColumnName("address")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("AuthorFirstName")
                        .IsRequired()
                        .HasColumnName("au_fname")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("AuthorLastName")
                        .IsRequired()
                        .HasColumnName("au_lname")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("City")
                        .HasColumnName("city")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<bool>("Contract")
                        .HasColumnName("contract");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnName("phone")
                        .HasAnnotation("MaxLength", 12);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("rowversion");

                    b.Property<string>("State")
                        .HasColumnName("state")
                        .HasAnnotation("MaxLength", 2);

                    b.Property<string>("Zip")
                        .HasColumnName("zip")
                        .HasAnnotation("MaxLength", 5);

                    b.HasKey("AuthorId");

                    b.ToTable("authors");
                });

            modelBuilder.Entity("AspNetCorehandson.Publisher", b =>
                {
                    b.Property<string>("PublisherId")
                        .HasColumnName("pub_id")
                        .HasAnnotation("MaxLength", 4);

                    b.Property<string>("City")
                        .HasColumnName("city")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("Country")
                        .HasColumnName("country")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("PublisherName")
                        .HasColumnName("pub_name")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("State")
                        .HasColumnName("state")
                        .HasAnnotation("MaxLength", 2);

                    b.HasKey("PublisherId");

                    b.ToTable("publishers");
                });

            modelBuilder.Entity("AspNetCorehandson.Sale", b =>
                {
                    b.Property<string>("StoreId")
                        .HasColumnName("stor_id")
                        .HasAnnotation("MaxLength", 4);

                    b.Property<string>("OrderNumber")
                        .HasColumnName("ord_num")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("TitleId")
                        .HasColumnName("title_id")
                        .HasAnnotation("MaxLength", 6);

                    b.Property<DateTime>("OrderDate")
                        .HasColumnName("ord_date");

                    b.Property<string>("PayTerms")
                        .IsRequired()
                        .HasColumnName("payterms")
                        .HasAnnotation("MaxLength", 12);

                    b.Property<int>("Quantity")
                        .HasColumnName("qty");

                    b.HasKey("StoreId", "OrderNumber", "TitleId");

                    b.HasIndex("StoreId");

                    b.HasIndex("TitleId");

                    b.ToTable("sales");
                });

            modelBuilder.Entity("AspNetCorehandson.Store", b =>
                {
                    b.Property<string>("StoreId")
                        .HasColumnName("stor_id")
                        .HasAnnotation("MaxLength", 4);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("stor_addr")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnName("city")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnName("state")
                        .HasAnnotation("MaxLength", 22);

                    b.Property<string>("StoreName")
                        .IsRequired()
                        .HasColumnName("stor_name")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnName("zip")
                        .HasAnnotation("MaxLength", 5);

                    b.HasKey("StoreId");

                    b.ToTable("stores");
                });

            modelBuilder.Entity("AspNetCorehandson.Title", b =>
                {
                    b.Property<string>("TitleId")
                        .HasColumnName("title_id")
                        .HasAnnotation("MaxLength", 6);

                    b.Property<decimal?>("Advance")
                        .HasColumnName("advance");

                    b.Property<string>("Notes")
                        .HasColumnName("notes")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<decimal?>("Price")
                        .HasColumnName("price");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnName("pubdate");

                    b.Property<string>("PublisherId")
                        .IsRequired()
                        .HasColumnName("pub_id")
                        .HasAnnotation("MaxLength", 4);

                    b.Property<int?>("Royalty")
                        .HasColumnName("royalty");

                    b.Property<string>("TitleName")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasAnnotation("MaxLength", 80);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnName("type")
                        .HasAnnotation("MaxLength", 12);

                    b.Property<int?>("YeatToDateSales")
                        .HasColumnName("ytd_sales");

                    b.HasKey("TitleId");

                    b.HasIndex("PublisherId");

                    b.ToTable("titles");
                });

            modelBuilder.Entity("AspNetCorehandson.TitleAuthor", b =>
                {
                    b.Property<string>("AuthorId")
                        .HasColumnName("au_id");

                    b.Property<string>("TitleId")
                        .HasColumnName("title_id");

                    b.Property<byte>("AuthorOrder")
                        .HasColumnName("au_ord");

                    b.Property<int>("RoyaltyPercentage")
                        .HasColumnName("royaltyper");

                    b.HasKey("AuthorId", "TitleId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("TitleId");

                    b.ToTable("titleauthor");
                });

            modelBuilder.Entity("AspNetCorehandson.Sale", b =>
                {
                    b.HasOne("AspNetCorehandson.Store", "Store")
                        .WithMany("Sales")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AspNetCorehandson.Title", "Title")
                        .WithMany("Sales")
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AspNetCorehandson.Title", b =>
                {
                    b.HasOne("AspNetCorehandson.Publisher", "Publisher")
                        .WithMany("Titles")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AspNetCorehandson.TitleAuthor", b =>
                {
                    b.HasOne("AspNetCorehandson.Author", "Author")
                        .WithMany("TitleAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AspNetCorehandson.Title", "Title")
                        .WithMany("TitleAuthors")
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
