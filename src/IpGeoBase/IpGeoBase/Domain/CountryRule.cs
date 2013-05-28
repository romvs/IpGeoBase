using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Правило выбора страны
    /// </summary>
    public class CountryRule : RuleBase
    {

        /// <summary>
        /// Двухсимвольный код страны
        /// </summary>
        [Required, MaxLength(2)]
        public string Country { get; set; }

        public CountryRule()
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
            if (range.Country == Country) return Kind;
            return RuleKind.Maybe;
        }

    }
}
