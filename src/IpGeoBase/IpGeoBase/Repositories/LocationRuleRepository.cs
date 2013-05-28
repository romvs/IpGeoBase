using System;
using System.Data.Entity;
using System.Linq;
using IpGeoBase.Domain;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным правил выбора географической локации
    /// </summary>
    public class LocationRuleRepository : RepositoryBase<LocationRule>
    {

        public LocationRuleRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public LocationRuleRepository()
            : base()
        {
        }

        public override DbSet<LocationRule> GetDbSet()
        {
            return DataContext.LocationRules;
        }

        /// <summary>
        /// Сохраняет новую сущность в базе данных
        /// </summary>
        /// <remarks>
        /// Перегружаем метод для генерации идентификатора правила выбора географической локации, если он не задан
        /// </remarks>
        /// <param name="entity">Сохраняемая сущность</param>
        public override void Create(LocationRule entity)
        {
            if (entity.Id.Equals(Guid.Empty))
            {
                entity.Id = Guid.NewGuid();
            }

            base.Create(entity);
        }

        /// <summary>
        /// Находит правило доступа к данным для указаной цели и географической локации
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="location">Географическая локация</param>
        /// <returns></returns>
        public LocationRule FindByTargetAndLocation(Target target, Location location)
        {
            return GetDbSet().FirstOrDefault(rule => (rule.TargetId == target.Id)
                                                  && (rule.LocationId == location.Id));
        }
    }
}
