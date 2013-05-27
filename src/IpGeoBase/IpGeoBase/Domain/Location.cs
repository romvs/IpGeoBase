using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Географическая локация
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Идентификатор локации в БД
        /// </summary>
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }

        /// <summary>
        /// Идентификатор региона
        /// </summary>
        [Required]
        public int RegionId { get; set; }

        /// <summary>
        /// Название локации
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Широта центра локации
        /// </summary>
        [Required]
        public Decimal Latitude { get; set; }

        /// <summary>
        /// Долгота центра локации
        /// </summary>
        [Required]
        public Decimal Longitude { get; set; }

        /// <summary>
        /// Навигационное свойство для округа
        /// </summary>
        public virtual Region Region { get; set; }

        /// <summary>
        /// Навигационное свойство для диапазонов ip-адресов
        /// </summary>
        public virtual ICollection<Range> Ranges { get; set; }
    
    }
}
