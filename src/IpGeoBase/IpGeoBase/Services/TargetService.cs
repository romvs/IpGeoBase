using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpGeoBase.Domain;
using IpGeoBase.Repositories;

namespace IpGeoBase.Services
{
    /// <summary>
    /// Класс управления целями геолокации
    /// </summary>
    public class TargetService : IDisposable
    {
        /// <summary>
        /// Контекст данных
        /// </summary>
        public IpGeoBaseContext DataContext { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным целей геолокации
        /// </summary>
        public TargetRepository TargetRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным правил выбора страны
        /// </summary>
        public CountryRuleRepository CountryRuleRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным правил выбора географического округа
        /// </summary>
        public AreaRuleRepository AreaRuleRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным географических округов
        /// </summary>
        public AreaRepository AreaRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным правил выбора географического региона
        /// </summary>
        public RegionRuleRepository RegionRuleRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным географических регионов
        /// </summary>
        public RegionRepository RegionRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным правил выбора географической локации
        /// </summary>
        public LocationRuleRepository LocationRuleRepository { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным географических локаций
        /// </summary>
        public LocationRepository LocationRepository { get; set; }

        public TargetService(IpGeoBaseContext dataContext)
        {
            DataContext = dataContext;
            TargetRepository = new TargetRepository(dataContext);
            CountryRuleRepository = new CountryRuleRepository(dataContext);
            AreaRuleRepository = new AreaRuleRepository(dataContext);
            AreaRepository = new AreaRepository(dataContext);
            RegionRuleRepository = new RegionRuleRepository(dataContext);
            RegionRepository = new RegionRepository(dataContext);
            LocationRuleRepository = new LocationRuleRepository(dataContext);
            LocationRepository = new LocationRepository(dataContext);
        }

        public TargetService()
            : this(new IpGeoBaseContext())
        {
        }

        /// <summary>
        /// Добавляет к цели правило выбора страны
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="country">Двухсимвольный код страны</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddCountryToTarget(Target target, string country, RuleKind kind)
        {
            CountryRule rule = CountryRuleRepository.FindByTargetAndCountry(target, country);

            if (rule == null)
            {
                rule = new CountryRule()
                {
                    TargetId = target.Id,
                    Country = country,
                    Kind = kind
                };

                CountryRuleRepository.Create(rule);
            }
            else if (rule.Kind != kind)
            {
                rule.Kind = kind;
                CountryRuleRepository.Save(rule);
            }
        }

        /// <summary>
        /// Добавляет к цели правило выбора страны
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="country">Двухсимвольный код страны</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddCountryToTarget(Guid targetId, string country, RuleKind kind)
        {
            AddCountryToTarget(FindTarget(targetId), country, kind);
        }

        /// <summary>
        /// Удаляет из цели правило выбора страны
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="country">Двухсимвольный код страны</param>
        public void RemoveCountryFromTarget(Target target, string country)
        {
            CountryRule rule = CountryRuleRepository.FindByTargetAndCountry(target, country);

            if (rule != null)
            {
                CountryRuleRepository.Delete(rule);
            }
        }

        /// <summary>
        /// Удаляет из цели правило выбора страны
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="country">Двухсимвольный код страны</param>
        public void RemoveCountryFromTarget(Guid targetId, string country)
        {
            RemoveCountryFromTarget(FindTarget(targetId), country);
        }

        /// <summary>
        /// Добавляет к цели правило выбора географического округа
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="area">Географический округ</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddAreaToTarget(Target target, Area area, RuleKind kind)
        {
            AreaRule rule = AreaRuleRepository.FindByTargetAndArea(target, area);

            if (rule == null)
            {
                rule = new AreaRule()
                {
                    TargetId = target.Id,
                    AreaId = area.Id,
                    Kind = kind
                };

                AreaRuleRepository.Create(rule);
            }
            else if (rule.Kind != kind)
            {
                rule.Kind = kind;
                AreaRuleRepository.Save(rule);
            }
        }

        /// <summary>
        /// Добавляет к цели правило выбора географического округа
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="areaName">Название географического округа</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddAreaToTarget(Guid targetId, string areaName, RuleKind kind)
        {
            AddAreaToTarget(FindTarget(targetId), FindArea(areaName), kind);
        }

        /// <summary>
        /// Удаляет из цели правило выбора географического округа
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="area">Географический округ</param>
        public void RemoveAreaFromTarget(Target target, Area area)
        {
            AreaRule rule = AreaRuleRepository.FindByTargetAndArea(target, area);

            if (rule != null)
            {
                AreaRuleRepository.Delete(rule);
            }
        }

        /// <summary>
        /// Удаляет из цели правило выбора географического округа
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="areaName">Название географического округа</param>
        public void RemoveAreaFromTarget(Guid targetId, string areaName)
        {
            RemoveAreaFromTarget(FindTarget(targetId), FindArea(areaName));
        }

        /// <summary>
        /// Добавляет к цели правило выбора географического региона
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="region">Географический регион</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddRegionToTarget(Target target, Region region, RuleKind kind)
        {
            RegionRule rule = RegionRuleRepository.FindByTargetAndRegion(target, region);

            if (rule == null)
            {
                rule = new RegionRule()
                {
                    TargetId = target.Id,
                    RegionId = region.Id,
                    Kind = kind
                };

                RegionRuleRepository.Create(rule);
            }
            else if (rule.Kind != kind)
            {
                rule.Kind = kind;
                RegionRuleRepository.Save(rule);
            }
        }

        /// <summary>
        /// Добавляет к цели правило выбора географического региона
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="regionName">Название географического региона</param>
        /// <param name="areaName">Название географического округа</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddRegionToTarget(Guid targetId, string regionName, string areaName, RuleKind kind)
        {
            AddRegionToTarget(FindTarget(targetId), FindRegion(regionName, areaName), kind);
        }

        /// <summary>
        /// Удаляет из цели правило выбора географического региона
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="region">Географический регион</param>
        public void RemoveRegionFromTarget(Target target, Region region)
        {
            RegionRule rule = RegionRuleRepository.FindByTargetAndRegion(target, region);

            if (rule != null)
            {
                RegionRuleRepository.Delete(rule);
            }
        }

        /// <summary>
        /// Удаляет из цели правило выбора географического региона
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="regionName">Название географического региона</param>
        /// <param name="areaName">Название географического округа</param>
        public void RemoveRegionFromTarget(Guid targetId, string regionName, string areaName)
        {
            RemoveRegionFromTarget(FindTarget(targetId), FindRegion(regionName, areaName));
        }

        /// <summary>
        /// Добавляет к цели правило выбора географической локации
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="location">Географическая локация</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddLocationToTarget(Target target, Location location, RuleKind kind)
        {
            LocationRule rule = LocationRuleRepository.FindByTargetAndLocation(target, location);

            if (rule == null)
            {
                rule = new LocationRule()
                {
                    TargetId = target.Id,
                    LocationId = location.Id,
                    Kind = kind
                };

                LocationRuleRepository.Create(rule);
            }
            else if (rule.Kind != kind)
            {
                rule.Kind = kind;
                LocationRuleRepository.Save(rule);
            }
        }

        /// <summary>
        /// Добавляет к цели правило выбора географической локации
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="locationName">Название географической локации</param>
        /// <param name="regionName">Название географического региона</param>
        /// <param name="areaName">Название географического округа</param>
        /// <param name="kind">Вид правила включения</param>
        public void AddLocationToTarget(Guid targetId, string locationName, string regionName, string areaName, RuleKind kind)
        {
            AddLocationToTarget(FindTarget(targetId), FindLocation(locationName, regionName, areaName), kind);
        }

        /// <summary>
        /// Удаляет из цели правило выбора географической локации
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="location">Географическая локация</param>
        public void RemoveLocationFromTarget(Target target, Location location)
        {
            LocationRule rule = LocationRuleRepository.FindByTargetAndLocation(target, location);

            if (rule != null)
            {
                LocationRuleRepository.Delete(rule);
            }
        }

        /// <summary>
        /// Удаляет из цели правило выбора географической локации
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <param name="locationName">Название географической локации</param>
        /// <param name="regionName">Название географического региона</param>
        /// <param name="areaName">Название географического округа</param>
        public void RemoveLocationFromTarget(Guid targetId, string locationName, string regionName, string areaName)
        {
            RemoveLocationFromTarget(FindTarget(targetId), FindLocation(locationName, regionName, areaName));
        }

        /// <summary>
        /// Находит цель геолокации с указанным идентификатором
        /// </summary>
        /// <param name="targetId">Идентификатор цели геолокации</param>
        /// <returns></returns>
        private Target FindTarget(Guid targetId)
        {
            Target target = TargetRepository.FindById(targetId);

            if (target == null)
            {
                throw new ArgumentException("Не найдена цель геолокации с указанным идентификатором", "targetId");
            }

            return target;
        }

        /// <summary>
        /// Находит географический округ с указанным названием
        /// </summary>
        /// <param name="areaName">Название географического округа</param>
        /// <returns></returns>
        private Area FindArea(string areaName)
        {
            Area area = AreaRepository.FindByName(areaName);

            if (area == null)
            {
                throw new ArgumentException("Не найден географический округ с указанным названием", "areaName");
            }

            return area;
        }

        /// <summary>
        /// Находит географический регион с указанным названием
        /// </summary>
        /// <param name="regionName">Название географического региона</param>
        /// <param name="areaName">Название географического округа</param>
        /// <returns></returns>
        private Region FindRegion(string regionName, string areaName)
        {
            Region region = RegionRepository.FindByNameAndArea(regionName, FindArea(areaName));

            if (region == null)
            {
                throw new ArgumentException("Не найден географический регион с указанным названием", "regionName");
            }

            return region;
        }

        /// <summary>
        /// Находит географическую локацию с указанным названием
        /// </summary>
        /// <param name="locationName">Название географической локации</param>
        /// <param name="regionName">Название географического региона</param>
        /// <param name="areaName">Название географического округа</param>
        /// <returns></returns>
        private Location FindLocation(string locationName, string regionName, string areaName)
        {
            Location location = LocationRepository.FindbyNameAndRegion(locationName, FindRegion(regionName, areaName));

            if (location == null)
            {
                throw new ArgumentException("Не найдена географическая локация с указанным названием", "locationName");
            }

            return location;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TargetService()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && DataContext != null)
            {
                // Обнуляем контекст данных в используемых репозиториях
                TargetRepository.DataContext = null;
                CountryRuleRepository.DataContext = null;
                AreaRuleRepository.DataContext = null;
                AreaRepository.DataContext = null;
                RegionRuleRepository.DataContext = null;
                RegionRepository.DataContext = null;
                LocationRuleRepository.DataContext = null;
                LocationRepository.DataContext = null;

                // Удаляем контекст данных
                DataContext.Dispose();
                DataContext = null;
            }
        }
    }
}
