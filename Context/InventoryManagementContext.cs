using Inventory_Management.Model;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Inventory_Management.Context
{
    public partial class InventoryManagementContext : DbContext
    {
        public InventoryManagementContext()
        {
        }

        public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Allocation> Allocations { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ReceiptRecord> ReceiptRecords { get; set; }
        public virtual DbSet<ReceivedRecord> ReceivedRecords { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");

            modelBuilder.Entity<Allocation>(entity =>
            {
                entity.HasKey(e => new { e.ReceiptId, e.ReceivedId })
                    .HasName("Allocation_pkey");

                entity.ToTable("Allocation");

                entity.Property(e => e.AllocatedQuantity).HasColumnName("Allocated_Quantity");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.Allocations)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReceiptId_fkey");

                entity.HasOne(d => d.Received)
                    .WithMany(p => p.Allocations)
                    .HasForeignKey(d => d.ReceivedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReceivedId_fkey");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Cost).HasPrecision(18, 2);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<ReceiptRecord>(entity =>
            {
                entity.ToTable("Receipt_Record");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<ReceivedRecord>(entity =>
            {
                entity.ToTable("Received_Record");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Cost).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.ToTable("Record");

                entity.Property(e => e.RecordId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("ProductId");

                entity.HasOne(d => d.RecordNavigation)
                    .WithOne(p => p.Record)
                    .HasForeignKey<Record>(d => d.RecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Receipt_RecordId_fkey");

                entity.HasOne(d => d.Record1)
                    .WithOne(p => p.Record)
                    .HasForeignKey<Record>(d => d.RecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Received_RecordId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
