using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// EventPage.xaml 的交互逻辑
    /// </summary>
    public partial class EventPage : UserControl {
        public EventPage(DashboardViewModel _DashboardViewModel) {
            this.DashboardViewModel = _DashboardViewModel;
            InitializeComponent();
            CameraItem cameraItem1 = new(DashboardViewModel);
            Panel.Children.Add(cameraItem1);
            CameraItem cameraItem2 = new(DashboardViewModel);
            Panel.Children.Add(cameraItem2);
        }

        private DashboardViewModel DashboardViewModel;
    }
}
