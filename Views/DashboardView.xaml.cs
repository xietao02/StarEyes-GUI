using StarEyes_GUI.Models;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StarEyes_GUI.Views {
    /// <summary>
    /// DashboardView.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardView : Window {
        public DashboardViewModel DashboardViewModel { get; set; } = new();
        PageTransition Pages;
        int curIndex, lastIndex;
        LoginView loginView;

        public DashboardView() {
            InitializeComponent();
            DataContext = this;
            curIndex = 0;
            SideBarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
            Pages = new PageTransition(DashboardViewModel);
            this.theGrid.Children.Add(Pages);
            Grid.SetRow(Pages, 1);
            Grid.SetColumn(Pages, 1);
        }
        
        // SwitchEvent 路由事件处理器
        private void SwitchHandler(object sender, SwitchEventArgs args) {
            FrameworkElement element = sender as FrameworkElement;
            SwitchPages(args.ItemIndex);
            if (element == this.Sidebar) {
                args.Handled = true;
            }
        }

        private void Notif_Click(object sender, RoutedEventArgs e) {
            SwitchPages(2);
        }

        private void User_Click(object sender, RoutedEventArgs e) {
            SwitchPages(4);
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            loginView = new();
            loginView.Show();
            this.Close();
        }

        #region 切换页面
        private void SwitchPages(int newIndex) {
            if(curIndex != newIndex) {
                lastIndex = curIndex;
                curIndex = newIndex;
                Pages.SwitchPage(curIndex);
                switch (curIndex) {
                    case 0:
                        SideBarItem1.IconSrc = "/Assets/icons/overview-active.png";
                        SideBarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 1:
                        SideBarItem2.IconSrc = "/Assets/icons/camera-active.png";
                        SideBarItem2.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 2:
                        SideBarItem3.IconSrc = "/Assets/icons/event-active.png";
                        SideBarItem3.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 3:
                        SideBarItem4.IconSrc = "/Assets/icons/cpu-active.png";
                        SideBarItem4.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 4:
                        SideBarItem5.IconSrc = "/Assets/icons/user-active.png";
                        SideBarItem5.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 5:
                        SideBarItem6.IconSrc = "/Assets/icons/more-active.png";
                        SideBarItem6.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    default:
                        break;
                }
                switch (lastIndex) {
                    case 0:
                        SideBarItem1.IconSrc = "/Assets/icons/overview.png";
                        SideBarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 1:
                        SideBarItem2.IconSrc = "/Assets/icons/camera.png";
                        SideBarItem2.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 2:
                        SideBarItem3.IconSrc = "/Assets/icons/event.png";
                        SideBarItem3.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 3:
                        SideBarItem4.IconSrc = "/Assets/icons/cpu.png";
                        SideBarItem4.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 4:
                        SideBarItem5.IconSrc = "/Assets/icons/user.png";
                        SideBarItem5.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 5:
                        SideBarItem6.IconSrc = "/Assets/icons/more.png";
                        SideBarItem6.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        
    }
}
