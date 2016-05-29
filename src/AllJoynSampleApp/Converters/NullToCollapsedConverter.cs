using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AllJoynSampleApp.Converters
{
    public class NullToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                    return Visibility.Collapsed;
            }
            else if(value is System.Collections.IEnumerable)
            {
                var hasItems = (value as System.Collections.IEnumerable).GetEnumerator().MoveNext();
                if(!hasItems)
                    return Visibility.Collapsed;
            }
            else if(value == null)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class NotNullToCollapsedConverter : IValueConverter
    {
        NullToCollapsedConverter proxy = new NullToCollapsedConverter();
        public NotNullToCollapsedConverter()
        {
            
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = proxy.Convert(value, targetType, parameter, language);
            if (v is Visibility && ((Visibility)v) == Visibility.Visible)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
