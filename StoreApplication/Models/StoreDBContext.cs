using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StoreApplication.Models
{
    public partial class StoreDBContext : DbContext
    {
        public StoreDBContext()
        {
        }

        public StoreDBContext(DbContextOptions<StoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<UserCart> UserCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.ProductImage)
                    .HasColumnName("productImage");

                entity.Property(e => e.ProductName)
                    .HasColumnName("productName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserCart>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.CartId });

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.CartId)
                    .HasColumnName("cartId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.UserCart)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserCart__produc__7C4F7684");
            });
        }
    }
}
