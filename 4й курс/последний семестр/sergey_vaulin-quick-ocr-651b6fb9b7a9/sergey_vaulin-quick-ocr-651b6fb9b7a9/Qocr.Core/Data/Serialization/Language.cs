using System.Collections.Generic;

namespace Qocr.Core.Data.Serialization
{
    /// <summary>
    /// Язык.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="Language"/>.
        /// </summary>
        public Language()
        {
            Chars = new List<Symbol>();
            FontFamilyNames = new List<string>();
        }

        /// <summary>
        /// Символы.
        /// </summary>
        public List<Symbol> Chars { get; set; }

        /// <summary>
        /// Список использованных шрифтов.
        /// </summary>
        public List<string> FontFamilyNames { get; set; }

        /// <summary>
        /// Название локализации.
        /// </summary>
        /// <remarks>RU-ru, EN-en, и т.д.</remarks>
        public string LocalizationName { get; set; }
    }
}