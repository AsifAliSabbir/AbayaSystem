using Microsoft.EntityFrameworkCore;
using AbayaSystem.Core;

namespace AbayaSystem.Infrastructure
{
    public class BoutiqueDbContext : DbContext
    {
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<FabricShop> FabricShops => Set<FabricShop>();
        public DbSet<Fabric> Fabrics => Set<Fabric>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Worker> Workers => Set<Worker>();
        public DbSet<ExternalWorker> ExternalWorkers { get; set; }

        public BoutiqueDbContext(DbContextOptions<BoutiqueDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔑 Configure Composite Primary Key for Order
            modelBuilder.Entity<Order>()
                .HasKey(o => new { o.BranchId, o.OrderId });

            // 💰 Column Specifications & Decimal Precision for Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(o => o.DepositPaid).HasColumnType("decimal(18,2)");
                entity.Property(o => o.BalanceDue).HasColumnType("decimal(18,2)");
                entity.Property(o => o.CustomerPhone).HasMaxLength(30);
            });

            // 🔗 Link OrderItems to Composite Key Parent Order
            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Order) // Specified 'i => i.Order' here
                .WithMany(o => o.Items)
                .HasForeignKey(i => new { i.BranchId, i.OrderId })
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign Keys for Workers & Catalogs
            modelBuilder.Entity<Worker>()
                .HasOne(w => w.Branch)
                .WithMany()
                .HasForeignKey(w => w.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.FabricShop)
                .WithMany()
                .HasForeignKey(i => i.FabricShopId)
                .IsRequired(false);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Fabric)
                .WithMany()
                .HasForeignKey(i => i.FabricId)
                .IsRequired(false);

            // Seed External Workers Data
            modelBuilder.Entity<ExternalWorker>().HasData(
                new ExternalWorker { ExternalWorkerId = 1, Name = "Rubel", Phone = "+971500000001", SupportedType = ExternalWorkerType.FullExternal },
                new ExternalWorker { ExternalWorkerId = 2, Name = "Saiful", Phone = "+971500000002", SupportedType = ExternalWorkerType.FullExternal },
                new ExternalWorker { ExternalWorkerId = 3, Name = "Alim Emb", Phone = "+971500000003", SupportedType = ExternalWorkerType.Hybrid },
                new ExternalWorker { ExternalWorkerId = 4, Name = "Computer Emb1", Phone = "+971500000003", SupportedType = ExternalWorkerType.Hybrid },
                new ExternalWorker { ExternalWorkerId = 5, Name = "Computer Emb2", Phone = "+971500000003", SupportedType = ExternalWorkerType.Hybrid }
            );
        }

        public static async Task SeedDatabaseAsync(BoutiqueDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            // 1. Seed Branches
            if (!await context.Branches.AnyAsync())
            {
                var branches = new[]
                {
                    new Branch { BranchName = "Bahja", IsWorkshop = false }, // ID 1
                    new Branch { BranchName = "Black View", IsWorkshop = false },  // ID 2
                    new Branch { BranchName = "Hurain", IsWorkshop = false },  // ID 3
                    new Branch { BranchName = "Workshop", IsWorkshop = true }      // ID 4
                };
                context.Branches.AddRange(branches);
                await context.SaveChangesAsync();
            }

            // 2. Seed Fabric Shops
            if (!await context.FabricShops.AnyAsync())
            {
                var shops = new[]
                {
                    new FabricShop { FabricShopName = "Sharqiya" },
                    new FabricShop { FabricShopName = "Najmat Al Madina" },
                    new FabricShop { FabricShopName = "ATN" }
                };
                context.FabricShops.AddRange(shops);
                await context.SaveChangesAsync();
            }

            // 3. Seed Fabrics
            if (!await context.Fabrics.AnyAsync())
            {
                var fabrics = new[]
                {
                    new Fabric { FabricName = "Nida" },
                    new Fabric { FabricName = "Crepe" },
                    new Fabric { FabricName = "Fua" },
                    new Fabric { FabricName = "Bella linen" },
                    new Fabric { FabricName = "Angelica" },
                    new Fabric { FabricName = "Victoria" },
                    new Fabric { FabricName = "Barbie Indonesian" },
                };
                context.Fabrics.AddRange(fabrics);
                await context.SaveChangesAsync();
            }

            // 4. Seed Workers with Branch Assignments
            if (!await context.Workers.AnyAsync())
            {
                var mainBranchId = (await context.Branches.FirstAsync(b => !b.IsWorkshop)).BranchId;
                var workshopBranchId = (await context.Branches.FirstAsync(b => b.IsWorkshop)).BranchId;

                var workers = new[]
                {
                    new Worker { Name = "Asif", Username = "asif", PasswordHash = "1111", AssignedRoles = WorkerRole.Admin, BranchId = mainBranchId },
                    new Worker { Name = "Nazrul", Username = "nazrul", PasswordHash = "2222", AssignedRoles = WorkerRole.Salesman, BranchId = mainBranchId },
                    new Worker { Name = "Forhad", Username = "forhad", PasswordHash = "1234", AssignedRoles = WorkerRole.CuttingMaster, BranchId = workshopBranchId },
                    new Worker { Name = "Razib", Username = "razib", PasswordHash = "1235", AssignedRoles = WorkerRole.Tailor, BranchId = workshopBranchId },
                    new Worker { Name = "Mamun", Username = "mamun", PasswordHash = "1236", AssignedRoles = WorkerRole.Tailor, BranchId = workshopBranchId },
                    new Worker { Name = "Jaojan", Username = "jaojan", PasswordHash = "1237", AssignedRoles = WorkerRole.HandEmbroiderer, BranchId = workshopBranchId }
                };

                context.Workers.AddRange(workers);
                await context.SaveChangesAsync();
            }
        }
    }
}