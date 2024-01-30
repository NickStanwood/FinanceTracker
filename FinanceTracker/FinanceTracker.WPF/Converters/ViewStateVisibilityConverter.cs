using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FinanceTracker.WPF
{
    internal class ViewStateVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || value is not MainWindowViewState)
                return Visibility.Collapsed;

            if(parameter == null || parameter is not MainWindowViewState)
                return Visibility.Collapsed;

            MainWindowViewState state = (MainWindowViewState)value;
            MainWindowViewState visible = (MainWindowViewState)parameter;
            if (state != visible)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
