using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FinanceTracker.Models;

namespace FinanceTracker.WPF
{
    internal class CategoryIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            Guid categoryId = (Guid)value;
            var task = Task.Run(async () =>
            {
                CategoryModel cm = await SQLiteContext.GetCategory(categoryId);
                return cm.Name;
            });


            return task.Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
