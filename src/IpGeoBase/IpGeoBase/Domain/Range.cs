using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Диапазон ip-адресов
    /// </summary>
    public class Range
    {
        /// <summary>
        /// Начало диапазона ip-адресов
        /// </summary>
        [Required, Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Start { get; set; }

        /// <summary>
        /// Конец диапазона ip-адресов
        /// </summary>
        [Required, Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long End { get; set; }

        /// <summary>
        /// Двухсимвольный код страны
        /// </summary>
        [Required, MaxLength(2)]
        public string Country { get; set; }

        /// <summary>
        /// Интервал ip-адресов в понятном виде
        /// </summary>
        [Required, MaxLength(33)]
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор локации
        /// </summary>
        public int? LocationId { get; set; }

        /// <summary>
        /// Навигационное свойство для локации
        /// </summary>
        public virtual Location Location { get; set; }

    }
}
