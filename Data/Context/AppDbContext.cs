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
        /// Value comparer for dictionary
        /// </summary>
        private static readonly ValueComparer _dictionaryComparer = new ValueComparer<Dictionary<int, CourseProgress>> ((dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2), dictionary => dictionary.Aggregate(0, (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode(), p.Value.GetHashCode()))));
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
            modelBuilder?.Entity<User>(action =>
            {
            action.Property(u => u.UserCourses)
                  .HasConversion(
                    value => JsonConvert.SerializeObject(value),
                    value => JsonConvert.DeserializeObject<Dictionary<int, CourseProgress>>(value))
                      .Metadata.SetValueComparer(_dictionaryComparer);
            });

            modelBuilder.Entity<User>()
            .Property(a => a.Id).ValueGeneratedNever();

            modelBuilder.Entity<Course>()
            .Property(a => a.Id).ValueGeneratedNever();

            modelBuilder.Entity<Material>()
            .Property(a => a.Id).ValueGeneratedNever();

        }

        /// <summary>
        /// Called while configuring application
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=CoursePortal; Trusted_Connection=true;");
        }
    }
}
