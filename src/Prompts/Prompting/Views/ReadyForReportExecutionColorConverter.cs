using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Prompts.Prompting.Views
{
    public class ReadyForReportExecutionColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == false ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
