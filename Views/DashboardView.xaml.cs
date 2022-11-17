using System;
using System.Windows;
using System.Windows.Controls;
using StarEyes_GUI.ViewModels;
using static StarEyes_GUI.UserControls.Sidebar;
using Window = System.Windows.Window;

namespace StarEyes_GUI.Views {
    /// <summary>
    /// DashboardView.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardView : Window {
        public DashboardViewModel DashboardViewModel { get; set; } = new();

        /// <summary>
        /// 初始化 DashboardViewModel
        /// </summary>
        public DashboardView() {
            InitializeComponent();
            DataContext = this;
            Switch += SwitchPageHandler;

            // 初始化 Header
            theGrid.Children.Add(DashboardViewModel.Header);
            Grid.SetColumnSpan(DashboardViewModel.Header, 2);

            // 初始化 Sidebar
            theGrid.Children.Add(DashboardViewModel.Sidebar);
            Grid.SetRow(DashboardViewModel.Sidebar, 1);

            // 初始化 _pages
            theGrid.Children.Add(DashboardViewModel.PagesPresenter);
            Grid.SetRow(DashboardViewModel.PagesPresenter, 1);
            Grid.SetColumn(DashboardViewModel.PagesPresenter, 1);

            Closed += DashboardView_Closed;
        }

        private void DashboardView_Closed(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        #region 切换页面

        // 为路由事件添加 CLR 事件包装器, XAML 编辑器将使用此包装器来生成自动提示
        public event SwitchEventHandler Switch {
            add { AddHandler(SwitchEvent, value); }
            remove { RemoveHandler(SwitchEvent, value); }
        }

        // SwitchEvent 路由事件处理器 - SwitchPageHandler
        private void SwitchPageHandler(object sender, SwitchEventArgs args) {
            FrameworkElement element = sender as FrameworkElement;
            if(args.ItemIndex == -1) {
                DashboardViewModel.DestructDashboardVM();
                DashboardViewModel.LoginView = new();
                DashboardViewModel.LoginView.Show();
                this.Close();
            }
            else if(args.ItemIndex == -2) {
                DashboardViewModel.Sidebar.SwitchItem(2);
                DashboardViewModel.PagesPresenter.ToPage(2);
            }
            else if (args.ItemIndex == -4) {
                DashboardViewModel.Sidebar.SwitchItem(4);
                DashboardViewModel.PagesPresenter.ToPage(4);
            }
            else DashboardViewModel.PagesPresenter.ToPage(args.ItemIndex);
            if (element == theGrid) {
                args.Handled = true;
            } 
        }
        #endregion

    }
}
