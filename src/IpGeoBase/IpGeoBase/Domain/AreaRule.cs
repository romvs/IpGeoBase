using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Правило выбора географического округа
    /// </summary>
    public class AreaRule : RuleBase
    {
        /// <summary>
        /// Идентификатор округа
        /// </summary>
        [Required]
        public int AreaId { get; set; }

        /// <summary>
        /// Навигационное свойство для округа
        /// </summary>
        public virtual Area Area { get; set; }

        public AreaRule()
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

                if (location.Region != null)
                {
                    Region region = location.Region;

                    if (region.AreaId == AreaId)
                    {
                        return Kind;
                    }
                }
            }

            return RuleKind.Maybe;            
        }
    }
}
