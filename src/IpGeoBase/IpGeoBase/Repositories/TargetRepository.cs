using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpGeoBase.Domain;
using System.Data.Entity;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным целей геолокации
    /// </summary>
    public class TargetRepository : RepositoryBase<Target>
    {
        public TargetRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public TargetRepository()
            : base()
        {
        }

        public override DbSet<Target> GetDbSet()
        {
            return DataContext.Targets;
        }

        /// <summary>
        /// Находит цель геолокации с указанным идентификатором
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Target FindById(Guid id)
        {
            return GetDbSet().FirstOrDefault(target => target.Id == id);
        }

        /// <summary>
        /// Сохраняет новую сущность в базе данных
        /// </summary>
        /// <remarks>
        /// Перегружаем метод для генерации идентификатора цели геолокации, если он не задан
        /// </remarks>
        /// <param name="entity">Сохраняемая сущность</param>
        public override void Create(Target entity)
        {
            if (entity.Id.Equals(Guid.Empty))
            {
                entity.Id = Guid.NewGuid();
            }

            base.Create(entity);
        }
    }
}
