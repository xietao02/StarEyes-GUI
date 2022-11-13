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
        #region 前端输入验证
        public LoginViewModel LoginViewModel { get; set; } = new();
        
        bool idFormat, pwFormat;

        DashboardView dashboardView;

        public LoginView() {
            InitializeComponent();
            DataContext = this;
            Account.Focus();
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
        /// ID前端输入验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_TextChanged(object sender, TextChangedEventArgs e) {
            idFormat = true;
            if (Account.Text == "") {
                ID_null.Visibility = Visibility.Visible;
                ID_error.Visibility = Visibility.Hidden;
                idFormat = false;
            }
            else {
                if (Account.Text.Length != 5) {
                    idFormat = false;
                }
                else {
                    for (int i = 0; i < 5; i++) {
                        if (!Char.IsNumber(Account.Text[i])) {
                            idFormat = false;
                            break;
                        }
                    }
                }
                if (idFormat) {
                    ID_error.Visibility = Visibility.Hidden;
                    ID_null.Visibility = Visibility.Hidden;
                }
                else {
                    ID_error.Visibility = Visibility.Visible;
                    ID_null.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// 密码前端输入验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordChanged(object sender, RoutedEventArgs e) {
            if (Password.Password == "") {
                PW_null.Visibility = Visibility.Visible;
                Auth_error.Visibility = Visibility.Hidden;
                pwFormat = false;
            }
            else {
                PW_null.Visibility = Visibility.Hidden;
                pwFormat = true;
            }
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
                if (!idFormat) Account.Focus();
                else if (pwFormat) {
                    if (Loading.Visibility == Visibility.Hidden) {
                        Auth_error.Visibility = Visibility.Hidden;
                        Network_error.Visibility = Visibility.Hidden;
                        Loading.Visibility = Visibility.Visible;
                        LoginViewModel.PW = Password.Password;
                        Thread thread = new Thread(new ThreadStart(LoginAuth));
                        thread.IsBackground = true;
                        thread.Start();
                    }
                }
                else {
                    PW_null.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion

        #region 登录验证
        private void LoginAuth() {
            LoginViewModel.LoginAuthCommand.Execute(null);
            if (!LoginViewModel.Auth) {
                if (!LoginViewModel.Status) {
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        Loading.Visibility = Visibility.Hidden;
                        Network_error.Visibility = Visibility.Visible;
                    }));
                }
                else {
                    Thread.Sleep(5000);
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        Loading.Visibility = Visibility.Hidden;
                        Auth_error.Visibility = Visibility.Visible;
                    }));
                }
            }
            else {
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    dashboardView = new();
                    dashboardView.Show();
                    this.Close();
                }));
            }
        }

        #endregion
    }
}
