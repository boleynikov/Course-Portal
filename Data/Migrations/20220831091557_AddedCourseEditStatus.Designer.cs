﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220831091557_AddedCourseEditStatus")]
    partial class AddedCourseEditStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CourseMaterial", b =>
                {
                    b.Property<int>("CourseMaterialsId")
                        .HasColumnType("int");

                    b.Property<int>("CoursesId")
                        .HasColumnType("int");

                    b.HasKey("CourseMaterialsId", "CoursesId");

                    b.HasIndex("CoursesId");

                    b.ToTable("CourseMaterial");
                });

            modelBuilder.Entity("Domain.Course", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("CourseSkills")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Domain.CourseMaterials.Material", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Materials");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Material");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserCourses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserSkills")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.CourseMaterials.ArticleMaterial", b =>
                {
                    b.HasBaseType("Domain.CourseMaterials.Material");

                    b.Property<DateTime>("DateOfPublication")
                        .HasColumnType("datetime2");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ArticleMaterial");
                });

            modelBuilder.Entity("Domain.CourseMaterials.PublicationMaterial", b =>
                {
                    b.HasBaseType("Domain.CourseMaterials.Material");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Format")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PageCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("YearOfPublication")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("PublicationMaterial");
                });

            modelBuilder.Entity("Domain.CourseMaterials.VideoMaterial", b =>
                {
                    b.HasBaseType("Domain.CourseMaterials.Material");

                    b.Property<float>("Duration")
                        .HasColumnType("real");

                    b.Property<int>("Quality")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("VideoMaterial");
                });

            modelBuilder.Entity("CourseMaterial", b =>
                {
                    b.HasOne("Domain.CourseMaterials.Material", null)
                        .WithMany()
                        .HasForeignKey("CourseMaterialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.CourseMaterials.Material", b =>
                {
                    b.HasOne("Domain.User", "User")
                        .WithMany("UserMaterials")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Navigation("UserMaterials");
                });
#pragma warning restore 612, 618
        }
    }
}
