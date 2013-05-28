using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Цель геолокации
    /// </summary>
    public class Target
    {

        /// <summary>
        /// Идентификатор цели
        /// </summary>
        [Required, Key, Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование цели
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Навигационное свойство для правил включения
        /// </summary>
        public virtual ICollection<RuleBase> Rules { get; set; }

        public Target()
        {
            Id = Guid.Empty;
        }

        /// <summary>
        /// Проверяет соответствие диапазона ip-адресов данной цели
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public bool IsSatisfied(Range range)
        {
            bool satisfied = false;

            foreach (RuleBase rule in Rules)
            {
                switch (rule.Proceed(range))
                {
                    case RuleKind.Include:
                        satisfied = true;
                        break;
                    case RuleKind.Exclude:
                        return false;
                }
            }

            return satisfied;
        }
    }

}
