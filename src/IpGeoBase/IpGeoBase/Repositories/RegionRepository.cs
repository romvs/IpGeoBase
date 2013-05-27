using System.Data.Entity;
using System.Linq;
using IpGeoBase.Domain;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным географических регионов
    /// </summary>
    public class RegionRepository : RepositoryBase<Region>
    {
        public RegionRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public RegionRepository()
            : base()
        {
        }

        public override DbSet<Region> GetDbSet()
        {
            return DataContext.Regions;
        }

        /// <summary>
        /// Находит географический регион с указанным названием и округом
        /// </summary>
        /// <param name="name">Наименование региона</param>
        /// <param name="area">Географический округ</param>
        /// <returns></returns>
        public Region FindByNameAndArea(string name, Area area)
        {
            return GetDbSet().FirstOrDefault(r => (r.Name == name)
                                               && (r.AreaId == area.Id));
        }

    }
}
