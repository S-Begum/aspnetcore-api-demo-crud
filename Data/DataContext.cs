using System;
using System.Collections.Generic;
using Demo2CoreAPICrud.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo2CoreAPICrud.Data;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK_Location");

            entity.ToTable("Locations", "Attendance");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Building).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.Company).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(200);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Log__5E5499A8F0277B5F");

            entity.ToTable("Log", "Attendance");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.LogDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_Log_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC2727C7BC9A");

            entity.ToTable("Users", "Attendance");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
