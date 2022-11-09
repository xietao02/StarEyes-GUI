using StarEyes_GUI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// CameraItem.xaml 的交互逻辑
    /// </summary>
    public partial class CameraItem : UserControl {
        public CameraItem(DashboardViewModel _DashboardViewModel) {
            this.DashboardViewModel = _DashboardViewModel;
            InitializeComponent();
        }

        private DashboardViewModel DashboardViewModel;
    }


}
