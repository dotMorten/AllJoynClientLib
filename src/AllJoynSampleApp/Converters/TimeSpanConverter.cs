using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AllJoynSampleApp.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is TimeSpan)
            {
                var ts = (TimeSpan)value;
                if (ts.Hours > 0)
                    return ts.ToString("h\\:mm\\:ss");
                else
                    return ts.ToString("m\\:ss");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
