using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class ConsoleDbContext : AppDbContext
    {
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
