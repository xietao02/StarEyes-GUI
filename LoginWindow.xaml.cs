using HandyControl.Controls;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
namespace StarEyes_GUI {
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : System.Windows.Window {
        bool ID_format, pwFormat, serverConn;
        ServerData Server;
        public LoginWindow() {
            InitializeComponent();
            Account.Focus();
            Server = new();
            serverConn = Server.ConnectServer();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var mp4Path = AppDomain.CurrentDomain.BaseDirectory + @"Resources\globe-hevc.mp4";
            LoginBG.Source = new Uri(mp4Path, UriKind.RelativeOrAbsolute);
            LoginBG.Play();
        }

        private void Login_bg_MediaEnded(object sender, RoutedEventArgs e) {
            LoginBG.Stop();
            LoginBG.Play();
        }

        private void Account_TextChanged(object sender, TextChangedEventArgs e) {
            ID_format = true;
            if (Account.Text == "") {
                ID_null.Visibility = Visibility.Visible;
                ID_error.Visibility = Visibility.Hidden;
                ID_format = false;
            }
            else {
                if (Account.Text.Length != 5) {
                    ID_format = false;
                }
                else {
                    for (int i = 0; i < 5; i++) {
                        if (!Char.IsNumber(Account.Text[i])) {
                            ID_format = false;
                            break;
                        }
                    }
                }
                if (ID_format) {
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
                if (!ID_format) Account.Focus();
                else if (pwFormat) {
                    //new Thread(() => {
                    //    this.Dispatcher.Invoke(new Action(() => {
                    //        Loading.Visibility = Visibility.Visible;
                    //    }));
                    //}).Start();
                    if (!serverConn) serverConn = Server.ConnectServer();
                    if (serverConn) {
                        Login();
                    }
                    //new Thread(() => {
                    //    this.Dispatcher.Invoke(new Action(() => {
                    //        Loading.Visibility = Visibility.Hidden;
                    //    }));
                    //}).Start();
                }
                else {
                    PW_null.Visibility = Visibility.Visible;
                }
            }
        }

        private void Login() {
            this.Close();
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
        }
    }
}
