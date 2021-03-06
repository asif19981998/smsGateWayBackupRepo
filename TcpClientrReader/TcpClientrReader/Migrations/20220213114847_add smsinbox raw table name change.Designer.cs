// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TcpClientrReader;

namespace TcpClientrReader.Migrations
{
    [DbContext(typeof(SMSDbContext))]
    [Migration("20220213114847_add smsinbox raw table name change")]
    partial class addsmsinboxrawtablenamechange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("TcpClientrReader.SmsInbox", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GsmSpan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Recvtime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SIndex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SmsId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Smsc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Total")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("smsInboxes");
                });

            modelBuilder.Entity("TcpClientrReader.SmsInbox_Raw", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GsmSpan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Recvtime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SIndex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SmsId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Smsc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Total")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("smsInboxRaw");
                });
#pragma warning restore 612, 618
        }
    }
}
