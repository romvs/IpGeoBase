using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Правило выбора географической локации
    /// </summary>
    public class LocationRule : RuleBase
    {

        /// <summary>
        /// Идентификатор локации
        /// </summary>
        [Required]
        public int LocationId { get; set; }

        /// <summary>
        /// Навигационное свойство для локации
        /// </summary>
        public virtual Location Location { get; set; }

        public LocationRule()
            : base()
        {
        }

        /// <summary>
        /// Процедура проверки включения указанного диапазона ip-адресов в правило выбора цели
        /// </summary>
        /// <param name="range">Диапазон ip-адресов</param>
        /// <returns></returns>
        public override RuleKind Proceed(Range range)
        {
            if (range.LocationId.HasValue && (range.LocationId.Value == LocationId))
            {
                return Kind;
            }

            return RuleKind.Maybe;
        }
    }
}
