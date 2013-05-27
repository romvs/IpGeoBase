using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Географический регион
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Идентификатор региона в БД
        /// </summary>
        [Required, Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// Идентификатор округа
        /// </summary>
        [Required]
        public int AreaId { get; set; }

        /// <summary>
        /// Название региона
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Навигационное свойство для округа
        /// </summary>
        public virtual Area Area { get; set; }

        /// <summary>
        /// Навигационное свойство для регионов
        /// </summary>
        public virtual ICollection<Location> Locations { get; set; }

    }
}
