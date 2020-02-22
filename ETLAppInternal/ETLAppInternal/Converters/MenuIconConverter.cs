using System;
using System.Globalization;
using ETLAppInternal.Enumerations;
using Xamarin.Forms;

namespace ETLAppInternal.Converters
{
    public class MenuIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (MenuItemType)value;

            switch (type)
            {
                //case MenuItemType.Jobs:
                //    return "ic_home.png";
                //case MenuItemType.Materials:
                //    return "ic_contact.png";
                //case MenuItemType.Samples:
                //    return "ic_pies.png";
                //case MenuItemType.Logout:
                //    return "ic_cart.png";
                //case MenuItemType.Logout:
                //    return "ic_logout.png";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Not needed here
            throw new NotImplementedException();
        }
    }
}
