using StarEyes_GUI.ViewModels;
using System.Windows.Controls;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// AboutPage.xaml 的交互逻辑
    /// </summary>
    public partial class AboutPage : UserControl {
        public AboutPage(DashboardViewModel _DashboardViewModel) {
            this.DashboardViewModel = _DashboardViewModel;
            InitializeComponent();
        }

        private DashboardViewModel DashboardViewModel;
    }
}
