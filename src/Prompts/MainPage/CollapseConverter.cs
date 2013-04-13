using System;
using System.Globalization;
using System.Windows.Data;

namespace Prompts.MainPage
{
    public class CollapseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (bool) value;

            return flag ? "Show" : "Hide";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
