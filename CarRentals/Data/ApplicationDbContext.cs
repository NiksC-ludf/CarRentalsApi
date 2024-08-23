namespace CarRentals.Data
{
    using Microsoft.EntityFrameworkCore;
    using CarRentals.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarOffer> CarOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarOffer>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }

        public override int SaveChanges()
        {
            UpdateLastModified();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateLastModified();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateLastModified()
        {
            var entries = ChangeTracker.Entries<CarOffer>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }
}
