
using Microsoft.EntityFrameworkCore;
using backend.Models;  // Adjust namespace if needed
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<OptimizationResult> OptimizationResults { get; set; }
    }
}
