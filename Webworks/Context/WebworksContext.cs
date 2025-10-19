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

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<BlogPostContent> BlogPostContents { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Webworks;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.ToTable("blog_post");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasComment("Post category id")
                .HasColumnName("category_id");
            entity.Property(e => e.PublicationDate)
                .HasComment("Date of publication of post")
                .HasColumnType("datetime")
                .HasColumnName("publication_date");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .HasComment("Slug for the post.")
                .HasColumnName("slug");

            entity.HasOne(d => d.Category).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_blog_post_category");
        });

        modelBuilder.Entity<BlogPostContent>(entity =>
        {
            entity.ToTable("blog_post_content");

            entity.HasIndex(e => new { e.BlogPostId, e.LanguageId }, "IX_blog_post_content").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlogPostId).HasColumnName("blog_post_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Excerpt)
                .HasMaxLength(300)
                .HasColumnName("excerpt");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.BlogPost).WithMany(p => p.BlogPostContents)
                .HasForeignKey(d => d.BlogPostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_blog_post_content_blog_post");

            entity.HasOne(d => d.Language).WithMany(p => p.BlogPostContents)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_blog_post_content_language");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");

            entity.Property(e => e.Id)
                .HasComment("Id of category")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasComment("Category name")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("language");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Direction)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("LTR")
                .HasComment("Direction of language, 'LTR' or 'RTL'")
                .HasColumnName("direction");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasComment("language, e.g. 'en'")
                .HasColumnName("language_code");
            entity.Property(e => e.LocaleCode)
                .HasMaxLength(10)
                .HasComment("language + region,  e.g. 'en-US'")
                .HasColumnName("locale_code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
