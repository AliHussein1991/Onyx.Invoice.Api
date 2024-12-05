using Microsoft.EntityFrameworkCore;
using Onyx.Invoice.Core.Entities;

namespace Onyx.Invoice.Infrastructure.Contexts
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options) { }

        public DbSet<InvoiceGroup> InvoiceGroup { get; set; }
        public DbSet<Core.Entities.Invoice> Invoice { get; set; }
        public DbSet<Observation> Observation { get; set; }
        public DbSet<TravelAgentInfo> TravelAgent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InvoiceGroup>()
                .HasMany(ig => ig.Invoices)
                .WithOne(i => i.InvoiceGroup)
                .HasForeignKey(i => i.InvoiceGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Core.Entities.Invoice>()
                .HasMany(i => i.Observations)
                .WithOne(o => o.Invoice)
                .HasForeignKey(o => o.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
