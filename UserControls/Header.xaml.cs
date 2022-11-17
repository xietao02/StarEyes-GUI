using System.Windows;
using System.Windows.Controls;
using StarEyes_GUI.UserControls.UCViewModels;
using static StarEyes_GUI.UserControls.Sidebar;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// Header.xaml 的交互逻辑
    /// </summary>
    public partial class Header : UserControl {

        public HeaderViewModel HeaderViewModel { get; set; } = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        public Header() {
            InitializeComponent();
            DataContext = this;
        }
        
        #region 激发路由事件
        /// <summary>
        /// 通知按钮被按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notif_Click(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = -2;
            this.RaiseEvent(args);
        }

        /// <summary>
        /// 用户按钮被按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void User_Click(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = -4;
            this.RaiseEvent(args);
        }

        /// <summary>
        /// 退出登录按钮被按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = -1;
            this.RaiseEvent(args);
        }
        #endregion
        
    }
}
