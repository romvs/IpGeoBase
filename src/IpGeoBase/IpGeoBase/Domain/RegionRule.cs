using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Правило выбора географического региона
    /// </summary>
    public class RegionRule : RuleBase
    {

        /// <summary>
        /// Идентификатор региона
        /// </summary>
        [Required]
        public int RegionId { get; set; }

        /// <summary>
        /// Навигационное свойство для региона
        /// </summary>
        public virtual Region Region { get; set; }

        public RegionRule()
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
            if (range.Location != null)
            {
                Location location = range.Location;

                if (location.RegionId == RegionId)
                {
                    return Kind;
                }
            }

            return RuleKind.Maybe;
        }

    }
}
