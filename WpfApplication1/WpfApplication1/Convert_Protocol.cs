using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfApplication1.Database;

namespace WpfApplication1
{
    class Convert_Protocol : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int port;
            int.TryParse(value.ToString(), out port);
            PortService ps = new PortService();
            return ps.GetServiceName(port);
            
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)

        {

            throw new Exception();

        }
    }
}
