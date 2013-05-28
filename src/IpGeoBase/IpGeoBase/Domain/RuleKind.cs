using System;

namespace IpGeoBase.Domain
{

    /// <summary>
    /// Перечисление видов правил для выбора геоцели
    /// </summary>
    public enum RuleKind
    {
        /// <summary>
        /// Нейтрально
        /// </summary>
        Maybe,
        /// <summary>
        /// Правило включения
        /// </summary>
        Include,
        /// <summary>
        /// Правило ичключения
        /// </summary>
        Exclude
    }

}