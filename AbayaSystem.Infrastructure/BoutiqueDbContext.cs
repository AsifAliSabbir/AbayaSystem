using AbayaSystem.Core;
using AbayaSystem.Core.AbayaSystem.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AbayaSystem.Infrastructure
{
    public class BoutiqueDbContext : DbContext
    {
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<FabricShop> FabricShops => Set<FabricShop>();
        public DbSet<SheilaShop> SheilaShops => Set<SheilaShop>();
        public DbSet<SheilaTran> SheilaTrans => Set<SheilaTran>();
        public DbSet<Fabric> Fabrics => Set<Fabric>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Worker> Workers => Set<Worker>();
        public DbSet<ExternalWorker> ExternalWorkers { get; set; }
        public DbSet<ExternalVendorJob> ExternalVendorJobs { get; set; }

        public DbSet<StatusLog> StatusLogs { get; set; }

        public BoutiqueDbContext(DbContextOptions<BoutiqueDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 👤 Configure Customer Entity Properties & Precision
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.CustomerName).HasMaxLength(150).IsRequired();
                entity.Property(c => c.CustomerPhone).HasMaxLength(30).IsRequired();

                entity.Property(c => c.LengthAbayaFront).HasColumnType("decimal(18,2)");
                entity.Property(c => c.LengthAbayaBack).HasColumnType("decimal(18,2)");
                entity.Property(c => c.LengthSleeve).HasColumnType("decimal(18,2)");
                entity.Property(c => c.WidthArmHole).HasColumnType("decimal(18,2)");
                entity.Property(c => c.WidthSleeveOpening).HasColumnType("decimal(18,2)");
                entity.Property(c => c.WidthShoulder).HasColumnType("decimal(18,2)");
                entity.Property(c => c.WidthBody).HasColumnType("decimal(18,2)");
                entity.Property(c => c.WidthBottom).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<SheilaShop>(entity =>
            {
                entity.ToTable("SheilaShops");
                entity.HasKey(s => s.SheilaShopID);
                entity.Property(s => s.SheilaShopName).HasMaxLength(150).IsRequired();
                entity.HasOne(s => s.Branch)
                    .WithMany()
                    .HasForeignKey(s => s.BranchID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SheilaTran>(entity =>
            {
                entity.ToTable("SheilaTrans");
                entity.HasKey(t => t.SheilaTranID);
                entity.Property(t => t.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(t => t.PurchaseAmount).HasColumnType("decimal(18,2)");
                entity.Property(t => t.PaymentAmount).HasColumnType("decimal(18,2)");
                entity.HasOne(t => t.SheilaShop)
                    .WithMany(s => s.Transactions)
                    .HasForeignKey(t => t.SheilaShopID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 🔑 Configure Composite Primary Key for Order & Customer FK
            modelBuilder.Entity<Order>()
                .HasKey(o => new { o.BranchId, o.OrderId });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(o => o.DepositPaid).HasColumnType("decimal(18,2)");
                entity.Property(o => o.BalanceDue).HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.Customer)
                      .WithMany()
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 🔗 Link OrderItems to Composite Key Parent Order
            modelBuilder.Entity<OrderItem>()
                .HasKey(i => new { i.BranchId, i.OrderId, i.OrderItemId });

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.OrderItemId)
                .ValueGeneratedNever();

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Order)
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

            modelBuilder.Entity<OrderItem>().Property(i => i.HandEmbRequired).HasDefaultValue(false);
            modelBuilder.Entity<OrderItem>().Property(i => i.rawFabricEmb).HasDefaultValue(false);
            modelBuilder.Entity<OrderItem>().Property(i => i.BuyFabricForExternal).HasDefaultValue(false);

            modelBuilder.Entity<ExternalVendorJob>()
                .HasOne(j => j.OrderItem)
                .WithMany(i => i.ExternalVendorJobs)
                .HasForeignKey(j => new { j.BranchId, j.OrderId, j.OrderItemId })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExternalVendorJob>()
                .HasOne(j => j.ExternalWorker)
                .WithMany()
                .HasForeignKey(j => j.ExternalWorkerId)
                .OnDelete(DeleteBehavior.Restrict);

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
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync();
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }

            if (!await context.Branches.AnyAsync())
            {
                var branches = new[]
                {
                    new Branch { BranchName = "Bahja", IsWorkshop = false },
                    new Branch { BranchName = "Black View", IsWorkshop = false },
                    new Branch { BranchName = "Hurain", IsWorkshop = false },
                    new Branch { BranchName = "Workshop", IsWorkshop = true }
                };
                context.Branches.AddRange(branches);
                await context.SaveChangesAsync();
            }

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
