using System;
using System.Data.Entity;
using System.Linq;
using IpGeoBase.Domain;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным правил выбора страны
    /// </summary>
    public class CountryRuleRepository : RepositoryBase<CountryRule>
    {

        public CountryRuleRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public CountryRuleRepository()
            : base()
        {
        }

        public override DbSet<CountryRule> GetDbSet()
        {
            return DataContext.CountryRules;
        }

        /// <summary>
        /// Сохраняет новую сущность в базе данных
        /// </summary>
        /// <remarks>
        /// Перегружаем метод для генерации идентификатора правила выбора страны, если он не задан
        /// </remarks>
        /// <param name="entity">Сохраняемая сущность</param>
        public override void Create(CountryRule entity)
        {
            if (entity.Id.Equals(Guid.Empty))
            {
                entity.Id = Guid.NewGuid();
            }

            base.Create(entity);
        }

        /// <summary>
        /// Находит правило доступа к данным для указаной цели и коду страны
        /// </summary>
        /// <param name="target">Цель геолокации</param>
        /// <param name="country">Двухсимвольный код страны</param>
        /// <returns></returns>
        public CountryRule FindByTargetAndCountry(Target target, string country)
        {
            return GetDbSet().FirstOrDefault(rule => (rule.TargetId == target.Id)
                                                  && (rule.Country == country));
        }
    }
}
