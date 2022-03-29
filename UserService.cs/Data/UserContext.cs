using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.cs.Models;

namespace UserService.cs.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> opts) : base(opts)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Purchases)
                .WithOne(p => p.User);

            modelBuilder.Entity<Purchase>()
                .Property(p => p.ProductId)
                .HasConversion(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<Guid>(v));
        }
    }
}
