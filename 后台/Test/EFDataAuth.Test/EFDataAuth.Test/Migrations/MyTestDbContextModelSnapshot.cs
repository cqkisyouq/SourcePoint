﻿// <auto-generated />
using EFDataAuth.Test.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace EFDataAuth.Test.Migrations
{
    [DbContext(typeof(MyTestDbContext))]
    partial class MyTestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EFDataAuth.Test.Domain.Data.Adress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Adress");
                });

            modelBuilder.Entity("EFDataAuth.Test.Domain.Data.Users", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Account");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<string>("Phone");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
