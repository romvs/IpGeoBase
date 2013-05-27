﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpGeoBase.Domain;
using System.Data.Entity;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным географических округов
    /// </summary>
    public class AreaRepository : RepositoryBase<Area>
    {
        public AreaRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public AreaRepository()
            : base()
        {
        }

        public override DbSet<Area> GetDbSet()
        {
            return DataContext.Areas;
        }

        /// <summary>
        /// Находит географический округ с указанным названием
        /// </summary>
        /// <param name="name">Наименование округа</param>
        /// <returns></returns>
        public Area FindByName(string name)
        {
            return GetDbSet().FirstOrDefault(a => a.Name == name);
        }

    }
}
