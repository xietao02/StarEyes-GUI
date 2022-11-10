using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.Windows.Controls;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// OverviewPage.xaml 的交互逻辑
    /// </summary>
    public partial class OverviewView : UserControl {

        public OverviewViewModel OverviewViewModel { get; set; } = new();
        public OverviewView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += OverviewViewModel.CalPageItemWidth;
        }
        
    }
}
