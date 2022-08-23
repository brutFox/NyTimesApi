using Microsoft.EntityFrameworkCore;
using NyTimesData.Entity;

namespace NyTimesData
{
    public class NyTimesDBContext : DbContext, INyTimesDBContext
    {
        public NyTimesDBContext(DbContextOptions<NyTimesDBContext> options) : base(options) 
        { 
        }

        public virtual DbSet<Multimedium> Multimedia { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Root> Roots { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(new System.Threading.CancellationToken());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Multimedium>().HasOne(m => m.result).WithMany(r => r.multimedia).HasForeignKey(m => m.ResultId).HasPrincipalKey(rr => rr.Id);
            modelBuilder.Entity<Result>().HasOne(r => r.root).WithMany(rt => rt.results).HasForeignKey(res => res.RootId).HasPrincipalKey(rr => rr.Id);
        }
    }
}
