using Microsoft.EntityFrameworkCore;
using SpendLess.Shared;

namespace SpendLess.Server.Models
{
    public partial class SpendLessContext : DbContext
    {
        public SpendLessContext()
        {
        }

        public SpendLessContext(DbContextOptions<SpendLessContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=SpendLess;Trusted_Connection=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Category)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Comment)
                    .HasMaxLength(200)
                    .IsFixedLength();

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50);

                entity.Property(e => e.InitialBalance);

                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(128)
                    .IsFixedLength();

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(64)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
