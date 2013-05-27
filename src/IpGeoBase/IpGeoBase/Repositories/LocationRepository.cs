using System;
using System.Data.Entity;
using System.Linq;
using IpGeoBase.Domain;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным географических локаций
    /// </summary>
    public class LocationRepository : RepositoryBase<Location>
    {
        public LocationRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public LocationRepository()
            : base()
        {
        }


        public override DbSet<Location> GetDbSet()
        {
            return DataContext.Locations;
        }

        /// <summary>
        /// Находит географическую локацию с указанным идентификатором
        /// </summary>
        /// <param name="locationId">Идентификатор локации</param>
        /// <returns></returns>
        public Location FindById(Int32 locationId)
        {
            return GetDbSet().FirstOrDefault(loc => loc.Id == locationId);
        }

        /// <summary>
        /// Находит географическую локацию с указанным названием и регионом
        /// </summary>
        /// <param name="name">Наименование локации</param>
        /// <param name="region">Географический регион</param>
        /// <returns></returns>
        public Location FindbyNameAndRegion(string name, Region region)
        {
            return GetDbSet().FirstOrDefault(loc => (loc.Name == name)
                                                 && (loc.RegionId == region.Id));
        }

    }
}
