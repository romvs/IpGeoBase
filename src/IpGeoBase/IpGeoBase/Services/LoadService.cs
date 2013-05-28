using System;
using IpGeoBase.Domain;
using IpGeoBase.Repositories;

namespace IpGeoBase.Services
{
    /// <summary>
    /// Сервис загрузки данных
    /// </summary>
    public class LoadService
    {
        /// <summary>
        /// Контекст данных
        /// </summary>
        public IpGeoBaseContext DataContext { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным географических округов
        /// </summary>
        public AreaRepository AreaRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным географических регионов
        /// </summary>
        public RegionRepository RegionRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным географических локаций
        /// </summary>
        public LocationRepository LocationRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным диапазонов ip-адресов
        /// </summary>
        public RangeRepository RangeRepository { get; set; }

        public LoadService(IpGeoBaseContext dataContext)
        {
            DataContext = dataContext;
            AreaRepository = new AreaRepository(dataContext);
            RegionRepository = new RegionRepository(dataContext);
            LocationRepository = new LocationRepository(dataContext);
            RangeRepository = new RangeRepository(dataContext);
        }

        public LoadService()
            : this(new IpGeoBaseContext())
        {
        }

        /// <summary>
        /// Находит географический округ с указанным названием
        /// или создает новый
        /// </summary>
        /// <param name="name">Название округа</param>
        /// <returns></returns>
        public Area GetOrCreateArea(string name)
        {
            Area area = AreaRepository.FindByName(name);

            if (area == null)
            {
                area = new Area()
                {
                    Name = name
                };

                AreaRepository.Create(area);
            }

            return area;
        }

        /// <summary>
        /// Находит географический регион с указанным названием
        /// или создает новый
        /// </summary>
        /// <param name="name">Название региона</param>
        /// <param name="area">Географический округ, в котором располагается регион</param>
        /// <returns></returns>
        public Region GetOrCreateRegion(string name, Area area)
        {
            Region region = RegionRepository.FindByNameAndArea(name, area);

            if (region == null)
            {
                region = new Region()
                {
                    Name = name,
                    AreaId = area.Id
                };

                RegionRepository.Create(region);
            }

            return region;
        }

        /// <summary>
        /// Обновляет или создает географическую локацию с указанными данными
        /// </summary>
        /// <param name="id">Идентификатор локации</param>
        /// <param name="name">Название локации</param>
        /// <param name="latitude">Герграфическая широта центра локации</param>
        /// <param name="longitude">Герграфическая долгота центра локации</param>
        /// <param name="region">Географический регион, в котором располагается локация</param>
        /// <returns></returns>
        public Location UpdateOrCreateLocation(Int32 id, string name, Decimal latitude,
                                            Decimal longitude, Region region)
        {
            Location location = LocationRepository.FindById(id);

            if (location == null)
            {
                location = new Location()
                {
                    Id = id,
                    Name = name,
                    Latitude = latitude,
                    Longitude = longitude,
                    RegionId = region.Id
                };

                LocationRepository.Create(location);
            }
            else if ((location.Name != name) || (location.Latitude != latitude) ||
                     (location.Longitude != latitude) || (location.RegionId != region.Id))
            {
                location.Name = name;
                location.Latitude = latitude;
                location.Longitude = longitude;
                location.RegionId = region.Id;

                LocationRepository.Save(location);
            }

            return location;
        }

        /// <summary>
        /// Находит географическую локацию с указанным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор локации</param>
        /// <returns></returns>
        public Location GetLocation(int id)
        {
            return LocationRepository.FindById(id);
        }

        /// <summary>
        /// Обновляет или создает диапазон ip-адресов с указанными данными
        /// </summary>
        /// <param name="start">Начало диапазона ip-адресов</param>
        /// <param name="end">Конец диапазона ip-адресов</param>
        /// <param name="description">Интервал ip-адресов в понятном виде</param>
        /// <param name="country">Двухсимвольный код страны</param>
        /// <param name="location">Географическая локация</param>
        /// <returns></returns>
        public Range UpdateOrCreateRange(long start, long end, string description, string country, Location location)
        {
            Range range = RangeRepository.FindByStartAndEnd(start, end);

            if (range == null)
            {
                range = new Range()
                {
                    Start = start,
                    End = end,
                    Description = description,
                    Country = country,
                    Location = location
                };

                RangeRepository.Create(range);
            }
            else if ((range.Description != description) || (range.Country != country) || (range.Location != location))
            {
                range.Description = description;
                range.Country = country;
                range.Location = location;

                RangeRepository.Save(range);
            }

            return range;
        }
    }
}
