using GMap.NET;
using StarEyes_GUI.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StarEyes_GUI.UserControls.Items {
    /// <summary>
    /// mapItem1.xaml 的交互逻辑
    /// </summary>
    public partial class mapItem1 : UserControl {
        private AMapControl MainMap;
        private double Lon, Lat;
        public mapItem1(AMapControl mainMap, string name, double lon, double lat) {
            InitializeComponent();
            MainMap = mainMap;
            Lon = lon;
            Lat = lat;
            bt.Content = name + ": 正常";
        }

        private void ChangePos(object sender, RoutedEventArgs e) {
            MainMap.Zoom = 17;
            MainMap.Position = new PointLatLng(Lat, Lon);
        }
    }
}
