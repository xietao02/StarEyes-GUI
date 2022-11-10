using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using StarEyes_GUI.Views.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// PageTransition.xaml 的交互逻辑
    /// </summary>
    public partial class PageTransition : UserControl {

        public PageTransition() {
            InitializeComponent();
            InitPage();
        }

        UserControl[] Pages = new UserControl[6];
        private int curIndex, toIndex;

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitPage() {
            Pages[0] = new OverviewView();
            Pages[1] = new CameraView();
            Pages[2] = new EventView();
            Pages[3] = new ServerView();
            Pages[4] = new UserView();
            Pages[5] = new AboutView();
            toIndex = 0;
            UserControl pageTo = Pages[toIndex];
            pageTo.Loaded += PageTo_Loaded;
            contentPresenter.Content = pageTo;
        }

        #region 页面切换动画
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
        #endregion
    }
}
