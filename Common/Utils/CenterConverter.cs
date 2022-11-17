using System;
using System.Windows.Data;

namespace StarEyes_GUI.Common.Utils {
    /// <summary>
    /// 页面渐变样式转换器
    /// </summary>
    public class CenterConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (double)value / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
}
