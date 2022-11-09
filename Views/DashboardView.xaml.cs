using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using static StarEyes_GUI.UserControls.SideBar;

namespace StarEyes_GUI.Views {
    /// <summary>
    /// DashboardView.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardView : Window {
        public DashboardViewModel DashboardViewModel { get; set; } = new();
        PageTransition Pages;
        Header Header;
        LoginView? loginView;

        public DashboardView() {
            InitializeComponent();
            DataContext = this;
            Switch += SwitchPageHandler;

            // 初始化 Header
            Header = new Header(DashboardViewModel);
            theGrid.Children.Add(Header);
            Grid.SetColumnSpan(Header, 2);

            // 初始化 Sidebar
            SideBar sideBar = new SideBar();
            theGrid.Children.Add(sideBar);
            Grid.SetRow(sideBar, 1);

            // 初始化 Pages
            Pages = new PageTransition(DashboardViewModel);
            theGrid.Children.Add(Pages);
            Grid.SetRow(Pages, 1);
            Grid.SetColumn(Pages, 1);
            Pages.SizeChanged += CalPageItemWidth;

        }
        
        #region PageItem 控件宽度自适应
        private void CalPageItemWidth(object o, SizeChangedEventArgs args) {
            double pageWidth = args.NewSize.Width;
            if (pageWidth > 1230) {
                DashboardViewModel.PageItemWidth = pageWidth / 2 - 20;
            }
            else {
                DashboardViewModel.PageItemWidth = pageWidth - 20;
            }
        }
        #endregion

        #region 切换页面

        // SwitchEvent 路由事件处理器
        private void SwitchPageHandler(object sender, SwitchEventArgs args) {
            FrameworkElement element = sender as FrameworkElement;
            if(args.ItemIndex == -1) {
                loginView = new();
                loginView.Show();
                this.Close();
            }
            else Pages.SwitchPage(args.ItemIndex);
            if (element == this.theGrid) {
                args.Handled = true;
            } 
        }
        #endregion

        // 为路由事件添加 CLR 事件包装器, XAML 编辑器将使用此包装器来生成自动提示
        public event SwitchEventHandler Switch {
            add { AddHandler(SwitchEvent, value); }
            remove { RemoveHandler(SwitchEvent, value); }
        }

    }
}
