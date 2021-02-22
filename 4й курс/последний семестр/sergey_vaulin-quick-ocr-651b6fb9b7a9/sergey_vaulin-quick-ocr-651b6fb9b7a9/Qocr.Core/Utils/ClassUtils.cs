namespace Qocr.Core.Utils
{
    /// <summary>
    /// Набор методов для работы с классами.
    /// </summary>
    public static class ClassUtils
    {
        /// <summary>
        /// Заполнить значения свойств класса с шагом нумерации 1.
        /// </summary>
        /// <param name="instance">Ссылка на экземпляр.</param>
        /// <param name="prefix">Префикс свойства.</param>
        /// <param name="values">Список задаваемых значений.</param>
        public static void SetIndexValues(object instance, string prefix, params object[] values)
        {
            var type = instance.GetType();
            for (var i = 0; i < values.Length; i++)
            {
                type.GetProperty(prefix + i).SetValue(instance, values[i], null);
            }
        }
    }
}