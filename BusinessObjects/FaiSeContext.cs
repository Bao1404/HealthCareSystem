using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects;

public partial class FaiSeContext : DbContext
{
    public FaiSeContext()
    {
    }

    public FaiSeContext(DbContextOptions<FaiSeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-7BB0UNTH\\TUANNT;Database=FAiSE;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Blogs_CategoryId");

            entity.HasIndex(e => e.UserId, "IX_Blogs_UserId");

            entity.Property(e => e.Avatar)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.Ncontent).HasColumnType("ntext");
            entity.Property(e => e.Summary).HasColumnType("ntext");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blogs_Categories");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blogs_Users");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.ParentCategoryId, "IX_Categories_ParentCategoryId");

            entity.Property(e => e.CategoryName).HasMaxLength(200);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_Categories_Categories");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Mail, "IX_Users").IsUnique();

            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.Mail)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
