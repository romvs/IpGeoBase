using System;
using System.Data.Entity;
using System.Linq;
using IpGeoBase.Domain;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным правил выбора географического региона
    /// </summary>
    public class RegionRuleRepository : RepositoryBase<RegionRule>
    {

        public RegionRuleRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public RegionRuleRepository()
            : base()
        {
        }

        public override DbSet<RegionRule> GetDbSet()
        {
            return DataContext.RegionRules;
        }

        /// <summary>
        /// Сохраняет новую сущность в базе данных
        /// </summary>
        /// <remarks>
        /// Перегружаем метод для генерации идентификатора правила выбора географического региона, если он не задан
        /// </remarks>
        /// <param name="entity">Сохраняемая сущность</param>
        public override void Create(RegionRule entity)
        {
            if (entity.Id.Equals(Guid.Empty))
            {
                entity.Id = Guid.NewGuid();
            }

            base.Create(entity);
        }

        /// <summary>
        /// Находит правило доступа к данным для указаной цели и географического региона
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="region">Географический регион</param>
        /// <returns></returns>
        public RegionRule FindByTargetAndRegion(Target target, Region region)
        {
            return GetDbSet().FirstOrDefault(rule => (rule.TargetId == target.Id)
                                                  && (rule.RegionId == region.Id));
        }
    }
}
