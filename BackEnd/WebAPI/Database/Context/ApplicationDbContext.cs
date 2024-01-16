using EmployeeManagement.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            
        }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Lottery> Lotterys { get; set; }
        public virtual DbSet<Agency> Agencys { get; set; }
        public virtual DbSet<LotteryPrice> LotteryPrices { get; set; }
    }
}
