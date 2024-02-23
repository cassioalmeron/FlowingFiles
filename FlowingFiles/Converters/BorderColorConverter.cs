using System.Globalization;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace FlowingFiles.Converters
{
    internal class BorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (FileStatusEnum)value;

            if (status == FileStatusEnum.Filled)
                return Brushes.Green;
            if (status == FileStatusEnum.EmptyRequired)
                return Brushes.Red;
            return Brushes.Orange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}