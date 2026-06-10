using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MyAdminExplorer.Infrastructure
{
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance { get; } = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var header = value as string;
            if (header == null)
            {
                return null;
            }

            var imageName = header.Contains(@"\") ? "diskdrive.png" : "folder.png";
            return new BitmapImage(new Uri("pack://application:,,,/Assets/Images/" + imageName, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
