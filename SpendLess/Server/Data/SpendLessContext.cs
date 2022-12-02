﻿using Microsoft.EntityFrameworkCore;
using SpendLess.Shared;

namespace SpendLess.Server.Models
{
    public partial class SpendLessContext : DbContext
    {
        public SpendLessContext()
        {
        }

        public SpendLessContext(DbContextOptions<SpendLessContext> options) : base(options)
        {
        }

        public DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; } = null!;
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
            //modelBuilder.Entity<Transactions>(entity =>
            //{
            //    entity.property(e => e.id)
            //        .valuegeneratedonadd()
            //        .hascolumnname("id");

            //    entity.property(e => e.category)
            //        .hasmaxlength(20)
            //        .isfixedlength();

            //    entity.property(e => e.comment)
            //        .hasmaxlength(200)
            //        .isfixedlength();

            //    entity.property(e => e.period)
            //        .hasmaxlength(10)
            //        .isfixedlength();

            //    entity.property(e => e.transactiondate).hascolumntype("datetime");
            //});

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
