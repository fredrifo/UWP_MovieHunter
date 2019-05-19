using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MovieHunter.DataAccessCore.Models
{
    public partial class fredrifoContext : DbContext
    {
        public fredrifoContext()
        {
        }

        public fredrifoContext(DbContextOptions<fredrifoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<ListItem> ListItem { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<TokenValidator> TokenValidator { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:donau.hiof.no,1433;Database=fredrifo;User Id=fredrifo;Password=asD8TMgJ;Trusted_Connection=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<List>(entity =>
            {
                entity.Property(e => e.ListName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.List)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_List_User1");
            });

            modelBuilder.Entity<ListItem>(entity =>
            {
                entity.HasOne(d => d.List)
                    .WithMany(p => p.ListItem)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListItem_List1");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.ListItem)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListItem_Movie1");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.CoverImage)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Summary)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Director)
                    .WithMany(p => p.MovieDirector)
                    .HasForeignKey(d => d.DirectorId)
                    .HasConstraintName("FK_Movie_Person1");

                entity.HasOne(d => d.StarNavigation)
                    .WithMany(p => p.MovieStarNavigation)
                    .HasForeignKey(d => d.Star)
                    .HasConstraintName("FK_Movie_Person2");

                entity.HasOne(d => d.Writer)
                    .WithMany(p => p.MovieWriter)
                    .HasForeignKey(d => d.WriterId)
                    .HasConstraintName("FK_Movie_Person");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TokenValidator>(entity =>
            {
                entity.HasKey(e => e.TokenId);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
