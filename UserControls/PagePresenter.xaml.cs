using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using StarEyes_GUI.Views.Pages;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// PagePresenter.xaml 的交互逻辑
    /// </summary>
    public partial class PagePresenter : UserControl {
        private int _curIndex, _toIndex = 0;
        private UserControl[] _pages = new UserControl[6];

        public OverviewView OverviewView = new OverviewView();
        public CameraView CameraView = new CameraView();
        public EventView EventView = new EventView();
        public ServerView ServerView = new ServerView();
        public UserView UserView = new UserView();
        public AboutView AboutView = new AboutView();

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagePresenter() {
            InitializeComponent();
            _pages[0] = OverviewView;
            _pages[1] = CameraView;
            _pages[2] = EventView;
            _pages[3] = ServerView;
            _pages[4] = UserView;
            _pages[5] = AboutView;
            foreach (UserControl page in _pages) {
                page.Loaded += FadeInPage;
            }
            contentPresenter.Content = _pages[0];
        }

        #region 页面切换动画
        /// <summary>
        /// 切换页面
        /// </summary>
        /// <param name="index"></param>
        public void ToPage(int index) {
            if (_curIndex != index) {
                _toIndex = index;
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    FadeOutPage();
                }));
            }
        }

        /// <summary>
        /// 页面的 FadeOut 效果
        /// </summary>
        private void FadeOutPage() {
            Storyboard fadeOutStroyBoard = (Resources["FadeOut"] as Storyboard).Clone();
            fadeOutStroyBoard.Completed += SwitchPage;
            fadeOutStroyBoard.Begin(contentPresenter);
        }

        /// <summary>
        /// 切换 contentPresenter 内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchPage(object sender, EventArgs e) {
            //contentPresenter.Content = null;
            contentPresenter.Content = _pages[_toIndex];
        }

        /// <summary>
        /// 页面的 FadeIn 效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FadeInPage(object sender, RoutedEventArgs e) {
            Storyboard fadeInStoryBoard = (Resources["FadeIn"] as Storyboard).Clone();
            fadeInStoryBoard.Begin(contentPresenter);
            _curIndex = _toIndex;
        }
        #endregion
    }
}
