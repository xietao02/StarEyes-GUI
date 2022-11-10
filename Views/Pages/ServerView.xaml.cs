using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.Windows.Controls;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// ServerPage.xaml 的交互逻辑
    /// </summary>
    public partial class ServerView : UserControl {

        public ServerViewModel ServerViewModel { get; set; } = new();
        public ServerView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += ServerViewModel.CalPageItemWidth;
        }
        
    }
}
