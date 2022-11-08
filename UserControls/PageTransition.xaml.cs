using StarEyes_GUI.ViewModels;
using StarEyes_GUI.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// PageTransition.xaml 的交互逻辑
    /// </summary>
    public partial class PageTransition : UserControl {
        public PageTransition(DashboardViewModel _DashboardViewModel) {
            this.dashboardViewModel = _DashboardViewModel;
            InitializeComponent();
            InitPage();
        }

        private DashboardViewModel dashboardViewModel;

        UserControl[] Pages = new UserControl[6];
        private int curIndex, toIndex;

        private void InitPage() {
            Pages[0] = new OverviewPage(dashboardViewModel);
            Pages[1] = new CameraPage(dashboardViewModel);
            Pages[2] = new EventPage(dashboardViewModel);
            Pages[3] = new ServerPage(dashboardViewModel);
            Pages[4] = new UserPage(dashboardViewModel);
            Pages[5] = new AboutPage(dashboardViewModel);
            toIndex = 0;
            UserControl pageTo = Pages[toIndex];
            pageTo.Loaded += PageTo_Loaded;
            contentPresenter.Content = pageTo;
        }
        
        public void SwitchPage(int index) {
            if(curIndex != index) {
                toIndex = index;
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    ToPage();
                }));
            }
        }

        private void ToPage() {
            Storyboard hidePage = (Resources["FadeOut"] as Storyboard).Clone();
            hidePage.Completed += HidePage_Completed;
            hidePage.Begin(contentPresenter);
        }

        private void HidePage_Completed(object? sender, EventArgs e) {
            contentPresenter.Content = null;
            UserControl pageTo = Pages[toIndex];
            pageTo.Loaded += PageTo_Loaded;
            contentPresenter.Content = pageTo;
        }   

        private void PageTo_Loaded(object sender, RoutedEventArgs e) {
            Storyboard showPage = (Resources["FadeIn"] as Storyboard).Clone();
            showPage.Begin(contentPresenter);
            curIndex = toIndex;
        }
    }
}
