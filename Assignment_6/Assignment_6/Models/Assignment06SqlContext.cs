using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assignment_6.Models;

public partial class Assignment06SqlContext : DbContext
{
    public Assignment06SqlContext()
    {
    }

    public Assignment06SqlContext(DbContextOptions<Assignment06SqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:dolphin06.database.windows.net,1433;Initial Catalog=Assignment06Sql; User Id=dolphinadmin06; Password=Gryffindor#1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Author__70DAFC3491E4F1EA");

            entity.ToTable("Author");

            entity.Property(e => e.AuthorId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Book__3DE0C207CA07BAC7");

            entity.ToTable("Book");

            entity.Property(e => e.BookId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Book__CategoryId__628FA481");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Book__PublisherI__6383C8BA");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BookAutho__Autho__6754599E"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BookAutho__BookI__66603565"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId").HasName("PK__BookAuth__6AED6DC42A625A9F");
                        j.ToTable("BookAuthor");
                    });
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__BookCate__19093A0BE8841CD2");

            entity.ToTable("BookCategory");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__4C657FAB090E76CC");

            entity.ToTable("Publisher");

            entity.Property(e => e.PublisherId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.PublisherName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
