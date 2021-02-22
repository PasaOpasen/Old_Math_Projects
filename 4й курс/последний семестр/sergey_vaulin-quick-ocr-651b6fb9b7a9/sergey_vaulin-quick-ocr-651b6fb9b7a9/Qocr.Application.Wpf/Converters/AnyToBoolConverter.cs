using System;
using System.Globalization;
using System.Windows.Data;

namespace Qocr.Application.Wpf.Converters
{
    /// <summary>
    /// Любое не default(value) даёт true.
    /// </summary>
    public class AnyToBoolConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            var type = value.GetType();
            if (type.IsValueType)
            {
                return value != Activator.CreateInstance(type);
            }

            return true;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}