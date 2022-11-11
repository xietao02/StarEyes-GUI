using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.Windows.Controls;
using static StarEyes_GUI.ViewModels.Pages.AboutViewModel;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// AboutPage.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView : UserControl {

        public AboutViewModel AboutViewModel { get; set; } = new();
        public AboutView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += AboutViewModel.CalPageItemWidth;
            this.bt.Command = AboutViewModel.cmd;
            info a = new();
            a.name = "xiaoming";
            a.value = "boy";
            this.bt.CommandParameter = a;
        }
    }
}
