using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MyAdminExplorer.Properties;

namespace MyAdminExplorer
{
    #region HeaderToImageConverter

    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if ((value as string).Contains(@"\"))
            {
                Uri uri = new Uri("/Images/diskdrive.png", UriKind.Relative);
                BitmapImage source = new BitmapImage();
                return source;
            }
            else
            {
                Uri uri = new Uri("/Images/folder.png", UriKind.Relative);
                BitmapImage source = new BitmapImage(uri);
                return source;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }

    #endregion // DoubleToIntegerConverter


}