using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WeatherTest.WebUI.Models
{
    public partial class WeatherTestContext : DbContext
    {
        #region Constants

        #endregion

        #region Constructors
        public WeatherTestContext(DbContextOptions<WeatherTestContext> options) : base(options)
        { }

        #endregion

        #region Events + Delegates

        #endregion

        #region Properties
        public virtual DbSet<TemperatureConversion> TemperatureConversion { get; set; }
        public virtual DbSet<TemperatureUom> TemperatureUom { get; set; }
        public virtual DbSet<WeatherService> WeatherService { get; set; }
        public virtual DbSet<WindSpeedConversion> WindSpeedConversion { get; set; }
        public virtual DbSet<WindSpeedUom> WindSpeedUom { get; set; }

        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TemperatureConversion>(entity =>
            {
                entity.HasIndex(e => new { e.FromTemperatureUomid, e.ToTemperatureUomid })
                    .HasName("UC_TemperatureUniqueConversions")
                    .IsUnique();

                entity.Property(e => e.FromTemperatureUomid).HasColumnName("FromTemperatureUOMId");

                entity.Property(e => e.ToTemperatureUomid).HasColumnName("ToTemperatureUOMId");

                entity.Property(e => e.Formula).HasColumnName("Formula");

                entity.HasOne(d => d.FromTemperatureUom)
                    .WithMany(p => p.TemperatureConversionFromTemperatureUom)
                    .HasForeignKey(d => d.FromTemperatureUomid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TemperatureConversion_FromTemperatureUOM");

                entity.HasOne(d => d.ToTemperatureUom)
                    .WithMany(p => p.TemperatureConversionToTemperatureUom)
                    .HasForeignKey(d => d.ToTemperatureUomid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_TemperatureConversion_ToTemperatureUOM");
            });

            modelBuilder.Entity<TemperatureUom>(entity =>
            {
                entity.ToTable("TemperatureUOM");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<WeatherService>(entity =>
            {
                entity.Property(e => e.ServiceAddress)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.TemperaturePath)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TemperatureUomid).HasColumnName("TemperatureUOMId");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.WindSpeedPath)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.WindSpeedUomid).HasColumnName("WindSpeedUOMId");

                entity.HasOne(d => d.TemperatureUom)
                    .WithMany(p => p.WeatherService)
                    .HasForeignKey(d => d.TemperatureUomid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_WeatherService_TemperatureUOM");

                entity.HasOne(d => d.WindSpeedUom)
                    .WithMany(p => p.WeatherService)
                    .HasForeignKey(d => d.WindSpeedUomid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_WeatherService_WindSpeedUOM");
            });

            modelBuilder.Entity<WindSpeedConversion>(entity =>
            {
                entity.HasIndex(e => new { e.FromWindSpeedUomid, e.ToWindSpeedUomid })
                    .HasName("UC_WindSpeedUniqueConversions")
                    .IsUnique();

                entity.Property(e => e.FromWindSpeedUomid).HasColumnName("FromWindSpeedUOMId");

                entity.Property(e => e.ToWindSpeedUomid).HasColumnName("ToWindSpeedUOMId");

                entity.Property(e => e.Formula).HasColumnName("Formula");

                entity.HasOne(d => d.FromWindSpeedUom)
                    .WithMany(p => p.WindSpeedConversionFromWindSpeedUom)
                    .HasForeignKey(d => d.FromWindSpeedUomid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_WindSpeedConversion_FromTemperatureUOM");

                entity.HasOne(d => d.ToWindSpeedUom)
                    .WithMany(p => p.WindSpeedConversionToWindSpeedUom)
                    .HasForeignKey(d => d.ToWindSpeedUomid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_WindSpeedConversion_ToTemperatureUOM");
            });

            modelBuilder.Entity<WindSpeedUom>(entity =>
            {
                entity.ToTable("WindSpeedUOM");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });
        }

        #endregion
        

    }
}