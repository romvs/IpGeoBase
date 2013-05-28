using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Географический округ
    /// </summary>
    public class Area
    {
        /// <summary>
        /// Идентификатор округа в БД
        /// </summary>
        [Required, Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// Название округа
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Навигационное свойство для регионов
        /// </summary>
        public virtual ICollection<Region> Regions { get; set; }

        /// <summary>
        /// Навигационное свойство для правил выбора
        /// </summary>
        public virtual ICollection<AreaRule> Rules { get; set; }

    }
}
