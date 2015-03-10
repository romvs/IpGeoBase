using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using IpGeoBase.Domain;
using System.Net;

namespace IpGeoBase.Repositories
{
    /// <summary>
    /// Репозиторий доступа к данным диапазонов ip-адресов
    /// </summary>
    public class RangeRepository : RepositoryBase<Range>
    {
        public RangeRepository(IpGeoBaseContext dataContext)
            : base(dataContext)
        {
        }

        public RangeRepository()
            : base()
        {
        }


        public override DbSet<Range> GetDbSet()
        {
            return DataContext.Ranges;
        }

        /// <summary>
        /// Находит диапазон ip-адресов с указанными границами
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Range FindByStartAndEnd(long start, long end)
        {
            return GetDbSet().FirstOrDefault(range => (range.Start == start)
                                                   && (range.End == end));
        }

        /// <summary>
        /// Находит диапазон ip-адресов, в который попадает указанный адрес
        /// </summary>
        /// <param name="userHostAddress">Адрес для поиска диапазона</param>
        /// <returns></returns>
        public Range FindForIp(string userHostAddress)
        {
            byte[] bytes = IPAddress.Parse(userHostAddress).GetAddressBytes();
            long addr = (long)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | (bytes[3]));//IPAddress.HostToNetworkOrder(BitConverter.ToUInt32(, 0));
            return GetDbSet().FirstOrDefault(range => (range.Start <= addr)
                                                   && (range.End >= addr));
        }
    }
}
