using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace K19GameLibrary
{
    /// <summary>
    /// Конвертер для преобразования bool в Visibility
    /// Используется для показа/скрытия элементов при привязке данных
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Преобразование bool в Visibility
        /// true -> Visible, false -> Collapsed
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool boolValue && boolValue) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Обратное преобразование Visibility в bool
        /// Visible -> true, Collapsed -> false
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }
}