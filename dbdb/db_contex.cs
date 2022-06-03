using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbdb
{
    public partial class db_contex : DbContext
    {
        public virtual DbSet<Hostel> Hostels { get; set; } = null;
        public virtual DbSet<FirstName> FirstName { get; set; } = null;
        public virtual DbSet<SecondName> SecondName { get; set; } = null;
        public db_contex() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder oB)
        {
            if (!oB.IsConfigured)
            {
                oB.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;DataBase = dbdb_BD.mdf;Integrated Security=True;Connect Timeout=30");
            }
        }
        protected override void OnModelCreating(ModelBuilder mB) 
        {
            mB.Entity<Hostel>(entity =>
          {
              entity.HasKey(e => e.Id);

              entity.ToTable("Hostel");

              entity.Property(e => e.Id)
              .HasColumnName("Id")
              .HasMaxLength(5);

              entity.Property(e => e.RoomNumber)
             .HasColumnName("RoomNumber")
             .HasMaxLength(5);

              entity.Property(e => e.RoomType)
              .HasColumnName("RoomType")
              .HasMaxLength(50);

              entity.Property(e => e.DateRoomOccupied)
             .HasColumnName("DateRoomOccupied")
             .HasMaxLength(50);

              entity.Property(e => e.DateRoomFree)
             .HasColumnName("DateRoomFree")
             .HasMaxLength(50);

              entity.Property(e => e.FirstName)
              .HasColumnName("FirstName")
              .HasMaxLength(50);

              entity.Property(e => e.SecondName)
              .HasColumnName("SecondName")
              .HasMaxLength(50);

          });

            mB.Entity<FirstName>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.ToTable("FirstName");

                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasMaxLength(5);

                entity.Property(e => e.First)
                .HasColumnName("First")
                .HasMaxLength(50);
            });

            mB.Entity<SecondName>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.ToTable("SecondName");

                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasMaxLength(5);

                entity.Property(e => e.Second)
                .HasColumnName("Second")
                .HasMaxLength(50);
            });

            OnModelCreatingPartial(mB);
        }

        partial void OnModelCreatingPartial(ModelBuilder mB);
    }
}
