using System;
using System.Data.Entity;
using System.Linq;
using IpGeoBase.Domain;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным правил выбора географического округа
    /// </summary>
    public class AreaRuleRepository : RepositoryBase<AreaRule>
    {

        public AreaRuleRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public AreaRuleRepository()
            : base()
        {
        }

        public override DbSet<AreaRule> GetDbSet()
        {
            return DataContext.AreaRules;
        }

        /// <summary>
        /// Сохраняет новую сущность в базе данных
        /// </summary>
        /// <remarks>
        /// Перегружаем метод для генерации идентификатора правила выбора географического округа, если он не задан
        /// </remarks>
        /// <param name="entity">Сохраняемая сущность</param>
        public override void Create(AreaRule entity)
        {
            if (entity.Id.Equals(Guid.Empty))
            {
                entity.Id = Guid.NewGuid();
            }

            base.Create(entity);
        }

        /// <summary>
        /// Находит правило доступа к данным для указаной цели и географического округа
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="area">Географический округ</param>
        /// <returns></returns>
        public AreaRule FindByTargetAndArea(Target target, Area area)
        {
            return GetDbSet().FirstOrDefault(rule => (rule.TargetId == target.Id)
                                                  && (rule.AreaId == area.Id));
        }
    }
}
