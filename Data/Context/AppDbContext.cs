using Data.Repository;
using Domain;
using Domain.CourseMaterials;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Data.Context
{
    /// <summary>
    /// App Db Context
    /// </summary>
    public class AppDbContext : IdentityDbContext
    {
        /// <summary>
        /// Set of users
        /// </summary>
        public new DbSet<User> Users { get; set; }

        /// <summary>
        /// Set of Courses
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Set of all materials
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// Set of article materials
        /// </summary>
        public DbSet<ArticleMaterial> ArticleMaterials { get; set; }

        /// <summary>
        /// Set of publication materials
        /// </summary>
        public DbSet<PublicationMaterial> PublicationMaterials { get; set; }

        /// <summary>
        /// Set of video materials
        /// </summary>
        public DbSet<VideoMaterial> VideoMaterials { get; set; }

        /// <summary>
        /// On model creating method
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            BuildUsers(modelBuilder);
            BuildCourses(modelBuilder);
            BuildMaterials(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        private void BuildUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Property(a => a.Id).ValueGeneratedNever();

            modelBuilder?.Entity<User>(action =>
            {

                action.Property(u => u.UserCourses)
                      .HasConversion(
                        value => JsonConvert.SerializeObject(value),
                        value => JsonConvert.DeserializeObject<Dictionary<int, CourseProgress>>(value));

                action.Property(u => u.UserSkills)
                      .HasConversion(
                        value => JsonConvert.SerializeObject(value),
                        value => JsonConvert.DeserializeObject<List<Skill>>(value));
            });
        }

        private void BuildCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
            .Property(a => a.Id).ValueGeneratedNever();

            modelBuilder?.Entity<Course>(action =>
            {
                action.Property(u => u.CourseSkills)
                      .HasConversion(
                        value => JsonConvert.SerializeObject(value),
                        value => JsonConvert.DeserializeObject<List<Skill>>(value));
            });
        }

        private void BuildMaterials(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
            .Property(a => a.Id).ValueGeneratedNever();

            modelBuilder.Entity<Material>()
                .HasOne(m => m.User).WithMany(u => u.UserMaterials);
        }
    }
}
