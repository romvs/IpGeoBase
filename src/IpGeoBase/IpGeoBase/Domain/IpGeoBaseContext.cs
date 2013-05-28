using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Контекст доступа к данным модели
    /// </summary>
    public class IpGeoBaseContext : DbContext
    {
        /// <summary>
        /// Географические округа
        /// </summary>
        public DbSet<Area> Areas { get; set; }

        /// <summary>
        /// Географические регионы
        /// </summary>
        public DbSet<Region> Regions { get; set; }

        /// <summary>
        /// Географические локации
        /// </summary>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Диапазоны ip-адресов
        /// </summary>
        public DbSet<Range> Ranges { get; set; }

        /// <summary>
        /// Цели геолокации
        /// </summary>
        public DbSet<Target> Targets { get; set; }

        /// <summary>
        /// Правила выбора страны
        /// </summary>
        public DbSet<CountryRule> CountryRules { get; set; }

        /// <summary>
        /// Правила выбора географического округа
        /// </summary>
        public DbSet<AreaRule> AreaRules { get; set; }

        /// <summary>
        /// Правила выбора географического региона
        /// </summary>
        public DbSet<RegionRule> RegionRules { get; set; }

        /// <summary>
        /// Правила выбора географической локации
        /// </summary>
        public DbSet<LocationRule> LocationRules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Устанавливаем стратегию инициализации контекста данных
            IDatabaseInitializer<IpGeoBaseContext> strategy = null;
            Database.SetInitializer<IpGeoBaseContext>(strategy);

            // Настраиваем конфигурацию навигации

            modelBuilder.Entity<Area>()
                        .HasMany<Region>(area => area.Regions)
                        .WithRequired(region => region.Area)
                        .HasForeignKey(region => region.AreaId);

            modelBuilder.Entity<Area>()
                        .HasMany<AreaRule>(area => area.Rules)
                        .WithRequired(rule => rule.Area)
                        .HasForeignKey(rule => rule.AreaId);

            modelBuilder.Entity<Region>()
                        .HasMany<Location>(region => region.Locations)
                        .WithRequired(location => location.Region)
                        .HasForeignKey(location => location.RegionId);

            modelBuilder.Entity<Region>()
                        .HasMany<RegionRule>(region => region.Rules)
                        .WithRequired(rule => rule.Region)
                        .HasForeignKey(rule => rule.RegionId);

            modelBuilder.Entity<Location>()
                        .Property(location => location.Latitude)
                        .HasPrecision(10, 6);

            modelBuilder.Entity<Location>()
                        .Property(location => location.Longitude)
                        .HasPrecision(10, 6);

            modelBuilder.Entity<Location>()
                        .HasMany<Range>(location => location.Ranges)
                        .WithOptional(range => range.Location)
                        .HasForeignKey(range => range.LocationId);

            modelBuilder.Entity<Location>()
                        .HasMany<LocationRule>(location => location.Rules)
                        .WithRequired(rule => rule.Location)
                        .HasForeignKey(rule => rule.LocationId);

            modelBuilder.Entity<Target>()
                        .HasMany<RuleBase>(target => target.Rules)
                        .WithRequired(rule => rule.Target)
                        .HasForeignKey(rule => rule.TargetId);

            modelBuilder.Entity<CountryRule>()
                        .Map<CountryRule>(m => m.Requires("RuleType").HasValue<int>(1));

            modelBuilder.Entity<AreaRule>()
                        .Map<AreaRule>(m => m.Requires("RuleType").HasValue<int>(2));

            modelBuilder.Entity<RegionRule>()
                        .Map<RegionRule>(m => m.Requires("RuleType").HasValue<int>(3));

            modelBuilder.Entity<LocationRule>()
                        .Map<LocationRule>(m => m.Requires("RuleType").HasValue<int>(4));

        }
    }
}
