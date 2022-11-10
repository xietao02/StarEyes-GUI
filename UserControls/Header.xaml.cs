using StarEyes_GUI.UserControls.UCViewModels;
using System.Windows;
using System.Windows.Controls;
using static StarEyes_GUI.UserControls.SideBar;

namespace StarEyes_GUI.UserControls
{
    /// <summary>
    /// Header.xaml 的交互逻辑
    /// </summary>
    public partial class Header : UserControl {

        public HeaderViewModel HeaderViewModel { get; set; } = new();

        public Header() {
            InitializeComponent();
            DataContext = this;
        }
        
        #region 激发路由事件
        private void Notif_Click(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = 2;
            this.RaiseEvent(args);
        }

        private void User_Click(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = 4;
            this.RaiseEvent(args);
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = -1;
            this.RaiseEvent(args);
        }
        #endregion
        
    }
}
