using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Webworks.Models;

namespace Webworks.Context;

public partial class WebworksContext : DbContext
{
    public WebworksContext()
    {
    }

    public WebworksContext(DbContextOptions<WebworksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Webworks;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .HasComment("Id of category")
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasComment("Category name of post");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.PostId)
                .HasComment("")
                .HasColumnName("PostID");
            entity.Property(e => e.CategoryId)
                .HasComment("Post category id")
                .HasColumnName("CategoryID");
            entity.Property(e => e.Content).HasComment("Content of the post.");
            entity.Property(e => e.PublicationDate).HasComment("Date of publication of post");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasComment("Title of the post.");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Category");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
