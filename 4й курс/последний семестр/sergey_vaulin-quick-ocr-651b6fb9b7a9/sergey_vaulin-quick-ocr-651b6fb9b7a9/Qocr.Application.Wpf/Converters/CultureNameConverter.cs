using System;
using System.Globalization;
using System.Windows.Data;

namespace Qocr.Application.Wpf.Converters
{
    /// <summary>
    /// Сигнатура культуры в название.
    /// </summary>
    public class CultureNameConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            var stringValue = value.ToString();
            if (string.Equals(stringValue, "en-EN", StringComparison.OrdinalIgnoreCase))
            {
                // TODO Переделать бы, но пока не хочется перестраивать словари
                stringValue = "en";
            }

            var cultureInfo = CultureInfo.GetCultureInfo(stringValue);

            return cultureInfo.EnglishName;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}