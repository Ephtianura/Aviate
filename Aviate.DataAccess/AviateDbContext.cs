using Aviate.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace Aviate.DataAccess
{
    public class AviateDbContext : DbContext
    {
        public AviateDbContext(DbContextOptions<AviateDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AviateDbContext).Assembly);
        }
    }

}