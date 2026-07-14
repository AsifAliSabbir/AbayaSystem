using Microsoft.EntityFrameworkCore;
using AbayaSystem.Core;

namespace AbayaSystem.Infrastructure
{
    public class BoutiqueDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Worker> Workers => Set<Worker>(); // ◄ New physical table

        public BoutiqueDbContext(DbContextOptions<BoutiqueDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🧾 Lock down OrderId as a raw manual string primary key
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<Order>().Property(o => o.OrderId).ValueGeneratedNever().IsRequired();

            // 🔗 Link items directly to the parent order
            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>()
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🧵 Link Order Items to the Workers table for role assignments
            modelBuilder.Entity<OrderItem>()
                .HasOne<Worker>()
                .WithMany()
                .HasForeignKey(i => i.CutByWorkerId)
                .IsRequired(false) // Can be null before the cutting master claims it
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne<Worker>()
                .WithMany()
                .HasForeignKey(i => i.StitchedByWorkerId)
                .IsRequired(false) // Can be null before the tailor claims it
                .OnDelete(DeleteBehavior.Restrict);
        }

        // ... (Keep your existing DbSets and OnModelCreating code here) ...

        /// <summary>
        /// Seeds a default master employee account if the database table is completely empty.
        /// </summary>
        public static async Task SeedDatabaseAsync(BoutiqueDbContext context)
        {
            // Ensure the database itself is created before seeding
            await context.Database.EnsureCreatedAsync();

            // Check if any employees exist in SQL Server
            if (!await context.Workers.AnyAsync())
            {
                var defaultAdmin = new Worker
                {
                    Name = "Asif",
                    Username = "asif",
                    PasswordHash = "1111", // Plain text string matching our dev auth scheme
                    AssignedRoles = WorkerRole.Admin
                };

                var a = new Worker
                {
                    Name = "Nazrul",
                    Username = "nazrul",
                    PasswordHash = "2222", // Plain text string matching our dev auth scheme
                    AssignedRoles = WorkerRole.Salesman
                };
                var b = new Worker
                {
                    Name = "Forhad",
                    Username = "forhad",
                    PasswordHash = "1234", // Plain text string matching our dev auth scheme
                    AssignedRoles = WorkerRole.CuttingMaster
                };

                var c = new Worker
                {
                    Name = "Razib",
                    Username = "razib",
                    PasswordHash = "1235", // Plain text string matching our dev auth scheme
                    AssignedRoles = WorkerRole.Tailor
                };

                var d = new Worker
                {
                    Name = "Mamun",
                    Username = "mamun",
                    PasswordHash = "1236", // Plain text string matching our dev auth scheme
                    AssignedRoles = WorkerRole.Tailor
                };

                var e = new Worker
                {
                    Name = "jaojan",
                    Username = "jaojan",
                    PasswordHash = "1237", // Plain text string matching our dev auth scheme
                    AssignedRoles = WorkerRole.HandEmbroiderer
                };
                Worker[] workers = new Worker[] { defaultAdmin, a, b, c, d, e };

                //context.Workers.Add(defaultAdmin);
                context.Workers.AddRange(workers);
                await context.SaveChangesAsync();
            }
        }
    }


}