using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IpGeoBase.Domain
{
    /// <summary>
    /// Базовый класс для правил выбора цели
    /// </summary>
    public abstract class RuleBase
    {

        /// <summary>
        /// Идентификатор правила
        /// </summary>
        [Required, Key, Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор цели геолокации
        /// </summary>
        [Required, Column(TypeName = "uniqueidentifier")]
        public Guid TargetId { get; set; }

        /// <summary>
        /// Поле хранения вида правила включения
        /// </summary>
        [Column("Kind"), DefaultValue((int)RuleKind.Include)]
        public int KindId
        {
            get { return (int)this.Kind; }
            set { this.Kind = (RuleKind)value; }
        }

        /// <summary>
        /// Вид правила включения
        /// </summary>
        [NotMapped]
        public RuleKind Kind { get; set; }

        /// <summary>
        /// Навигационное свойство для цели геолокации
        /// </summary>
        public virtual Target Target { get; set; }

        /// <summary>
        /// Процедура проверки включения указанного диапазона ip-адресов в правило выбора цели
        /// </summary>
        /// <param name="range">Диапазон ip-адресов</param>
        /// <returns></returns>
        public abstract RuleKind Proceed(Range range);

        public RuleBase()
        {
            Id = Guid.Empty;
        }

    }
}
