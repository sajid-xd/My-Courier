using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace mycourier.Models;

public partial class MycourierContext : DbContext
{
    public MycourierContext()
    {
    }

    public MycourierContext(DbContextOptions<MycourierContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Weight> Weights { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__deliveri__3213E83FD71FFB10");

            entity.ToTable("deliveries");

            entity.HasIndex(e => e.TrackingId, "UQ__deliveri__7AC3E9AFC4B24895").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FromAddress)
                .HasColumnType("text")
                .HasColumnName("from_address");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.ReceiverName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("receiver_name");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.SenderName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sender_name");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.ToAddress)
                .HasColumnType("text")
                .HasColumnName("to_address");
            entity.Property(e => e.TrackingId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("tracking_id");
            entity.Property(e => e.WeightId).HasColumnName("weight_id");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__location__3213E83FDE4DBBFC");

            entity.ToTable("locations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fees)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("fees");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__services__3213E83F7B3FFC5D");

            entity.ToTable("services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fees)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("fees");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F804CEC18");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC5720A2F1A8E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.UserType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("user_type");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Weight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__weight__3213E83FE87EA7D7");

            entity.ToTable("weight");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fees)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("fees");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
