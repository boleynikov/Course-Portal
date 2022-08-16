using Data.Repository;
using Domain;
using Domain.CourseMaterials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// App Db Context
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Set of users
        /// </summary>
        public DbSet<User> Users { get; set; }

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

            BuildUsers(modelBuilder);
            BuildCourses(modelBuilder);
            BuildMaterials(modelBuilder);
        }

        /// <summary>
        /// Called while configuring application
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=CoursePortal; Trusted_Connection=true;");
        }


        private void BuildUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Property(a => a.Id).ValueGeneratedNever();

            JsonConverter[] converters = { new MaterialConverter() };
            modelBuilder?.Entity<User>(action =>
            {
                action.Property(u => u.UserMaterials)
                      .HasConversion(
                        value => JsonConvert.SerializeObject(value),
                        value => JsonConvert.DeserializeObject<List<Material>>(value, new JsonSerializerSettings() { Converters = converters }));

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

            JsonConverter[] converters = { new MaterialConverter() };
            modelBuilder?.Entity<Course>(action =>
            {
                action.Property(u => u.CourseMaterials)
                      .HasConversion(
                        value => JsonConvert.SerializeObject(value),
                        value => JsonConvert.DeserializeObject<List<Material>>(value, new JsonSerializerSettings() { Converters = converters }));

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
        }
    }
}
