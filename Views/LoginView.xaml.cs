using StarEyes_GUI.ViewModels;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarEyes_GUI.Views {
    /// <summary>
    /// 登陆界面
    /// </summary>
    public partial class LoginView : Window {
        public LoginViewModel LoginViewModel { get; set; } = new();
        public DashboardView DashboardView { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginView() {
            InitializeComponent();
            DataContext = this;
            Account.Focus();
            Account.TextChanged += LoginViewModel.CheckAccount;
            Password.PasswordChanged += LoginViewModel.CheckPassWord;
        }

        /// <summary>
        /// 播放背景动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var mp4Path = AppDomain.CurrentDomain.BaseDirectory + @"Assets\videos\globe-hevc.mp4";
            LoginBG.Source = new Uri(mp4Path, UriKind.RelativeOrAbsolute);
            LoginBG.Play();
        }
        
        /// <summary>
        /// 循环播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_bg_MediaEnded(object sender, RoutedEventArgs e) {
            LoginBG.Stop();
            LoginBG.Play();
        }

        /// <summary>
        /// 焦点跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                Password.Focus();
            }
        }

        /// <summary>
        /// 触发登陆验证事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                if (!LoginViewModel.IDFormat) Account.Focus();
                else {
                    Thread thread = new Thread(new ThreadStart(() => {
                        if (LoginViewModel.LoginAuth()) {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                DashboardView = new();
                                DashboardView.Show();
                                Close();
                            }));
                        }
                    }));
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }
                
    }
}
