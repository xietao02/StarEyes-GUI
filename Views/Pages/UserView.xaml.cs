using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.Windows.Controls;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// UserPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserView : UserControl {

        public UserViewModel UserViewModel { get; set; } = new();
        public UserView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += UserViewModel.CalPageItemWidth;
        }
        
    }
}
