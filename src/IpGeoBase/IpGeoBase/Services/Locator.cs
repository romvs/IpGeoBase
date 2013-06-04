using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpGeoBase.Domain;
using IpGeoBase.Repositories;

namespace IpGeoBase.Services
{
    /// <summary>
    /// Сервис геолокации
    /// </summary>
    public class Locator : IDisposable
    {
        /// <summary>
        /// Контекст данных
        /// </summary>
        public IpGeoBaseContext DataContext { get; set; }

        /// <summary>
        /// Репозиторий доступа к данным географических локаций
        /// </summary>
        public RangeRepository RangeRepository { get; set; }

        public TargetRepository TargetRepository { get; set; }

        public Locator(IpGeoBaseContext dataContext)
        {
            DataContext = dataContext;
            RangeRepository = new RangeRepository(dataContext);
            TargetRepository = new TargetRepository(dataContext);
        }

        public Locator()
            : this(new IpGeoBaseContext())
        {
        }

        /// <summary>
        /// Находит диапазон ip-адресов, в который попадает указанный адрес
        /// </summary>
        /// <param name="userHostAddress">Адрес для поиска диапазона</param>
        /// <returns></returns>
        public Range FindRange(string userHostAddress)
        {
            return RangeRepository.FindForIp(userHostAddress);
        }

        /// <summary>
        /// Находит цель геолокации, в которую попадает указанный диапазон ip-адресов
        /// </summary>
        /// <param name="range">Диапазон ip-адресов</param>
        /// <returns></returns>
        public Target Resolve(Range range)
        {
            if (range == null) return null;

            foreach (Target t in TargetRepository.FindAllWithRules())
            {
                if (t.IsSatisfied(range))
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Находит цель геолокации, в которую попадает указанный адрес
        /// </summary>
        /// <param name="userHostAddress">Адрес для поиска цели геолокации</param>
        /// <returns></returns>
        public Target FindTarget(string userHostAddress)
        {
            return Resolve(FindRange(userHostAddress));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Locator()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && DataContext != null)
            {
                // Обнуляем контекст данных в используемых репозиториях
                RangeRepository.DataContext = null;
                TargetRepository.DataContext = null;

                // Удаляем контекст данных
                DataContext.Dispose();
                DataContext = null;
            }
        }
    }
}
