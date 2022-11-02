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
    /// 
    /// </summary>
    public partial class LoginView : Window {
        public LoginViewModel LoginViewModel { get; set; } = new();
        
        bool idFormat, pwFormat;
        public LoginView() {
            InitializeComponent();
            DataContext = this;
            Account.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var mp4Path = AppDomain.CurrentDomain.BaseDirectory + @"Assets\globe-hevc.mp4";
            LoginBG.Source = new Uri(mp4Path, UriKind.RelativeOrAbsolute);
            LoginBG.Play();
        }

        private void Login_bg_MediaEnded(object sender, RoutedEventArgs e) {
            LoginBG.Stop();
            LoginBG.Play();
        }

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

        private void PasswordChanged(object sender, RoutedEventArgs e) {
            if (Password.Password == "") {
                PW_null.Visibility = Visibility.Visible;
                pwFormat = false;
            }
            else {
                PW_null.Visibility = Visibility.Hidden;
                pwFormat = true;
            }
        }

        private void Account_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                Password.Focus();
            }
        }

        private void Password_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                if (!idFormat) Account.Focus();
                else if (pwFormat) {
                    Loading.Visibility = Visibility.Visible;
                    LoginViewModel.LoginModel.PW = Password.Password;
                    Thread thread = new Thread(new ThreadStart(ThreadTask));
                    thread.IsBackground = true;
                    thread.Start();
                }
                else {
                    PW_null.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Trace.WriteLine(LoginViewModel.LoginModel.ID);
            System.Diagnostics.Trace.WriteLine(LoginViewModel.LoginModel.PW);
        }

        private void ThreadTask() {
            LoginViewModel.LoginAuthCommand.Execute(null);
            Thread.Sleep(1000);
            Application.Current.Dispatcher.Invoke(new Action(() => {
                Loading.Visibility = Visibility.Hidden;
            }));
        }

    }
}
