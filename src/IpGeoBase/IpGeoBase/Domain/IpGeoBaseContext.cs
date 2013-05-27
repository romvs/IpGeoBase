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

            modelBuilder.Entity<Region>()
                        .HasMany<Location>(region => region.Locations)
                        .WithRequired(location => location.Region)
                        .HasForeignKey(location => location.RegionId);

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

        }
    }
}
