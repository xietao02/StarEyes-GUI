using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using StarEyes_GUI.Common.Data;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.Views;

namespace StarEyes_GUI.ViewModels {

    /// <summary>
    /// 登陆界面交互逻辑
    /// </summary>
    public class LoginViewModel : NotificationObject {

        private StarEyesServer _server = new();
        public bool IDFormat;
        public bool PWFormat;
        private string _id { get; set; }
        private string _pw;

        #region 依赖属性
        private string _id_Null = "Hidden";
        public string ID_Null {
            get { return _id_Null; }
            set {
                _id_Null = value;
                RaisePropertyChanged("ID_Null");
            }
        }

        private string _pw_NULL = "Hidden";
        public string PW_NULL {
            get { return _pw_NULL; }
            set {
                _pw_NULL = value;
                RaisePropertyChanged("PW_NULL");
            }
        }

        private string _id_Error = "Hidden";
        public string ID_Error {
            get { return _id_Error; }
            set {
                _id_Error = value;
                RaisePropertyChanged("ID_Error");
            }
        }

        private string _auth_Error = "Hidden";
        public string Auth_Error {
            get { return _auth_Error; }
            set {
                _auth_Error = value;
                RaisePropertyChanged("Auth_Error");
            }
        }

        private string _network_Error = "Hidden";
        public string Network_Error {
            get { return _network_Error; }
            set {
                _network_Error = value;
                RaisePropertyChanged("Network_Error");
            }
        }

        private string _loading = "Hidden";
        public string Loading {
            get { return _loading; }
            set {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }
        #endregion

        #region 登录验证
        /// <summary>
        /// 检查账号格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void CheckAccount(object sender, TextChangedEventArgs args) {
            TextBox textBox = sender as TextBox;
            IDFormat = true;
            if (textBox.Text == "") {
                ID_Null = "Visible";
                ID_Error = "Hidden";
                IDFormat = false;
            }
            else {
                if (textBox.Text.Length != 5) {
                    IDFormat = false;
                }
                else {
                    for (int i = 0; i < 5; i++) {
                        if (!Char.IsNumber(textBox.Text[i])) {
                            IDFormat = false;
                            break;
                        }
                    }
                }
                if (IDFormat) {
                    ID_Error = "Hidden";
                    ID_Null = "Hidden";
                    _id = textBox.Text;
                }
                else {
                    ID_Error = "Visible";
                    ID_Null = "Hidden";
                }
            }
            args.Handled = true;
        }

        /// <summary>
        /// 检查密码格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void CheckPassWord(object sender, RoutedEventArgs args) {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox.Password == "") {
                PW_NULL = "Visible";
                Auth_Error = "Hidden";
                PWFormat = false;
            }
            else {
                PW_NULL = "Hidden";
                PWFormat = true;
                _pw = passwordBox.Password;
            }
            args.Handled = true;
        }

        /// <summary>
        /// 服务器验证登录权限
        /// </summary>
        public bool LoginAuth() {
            if (Loading == "Hidden") {
                if (PWFormat) {
                    Auth_Error = "Hidden";
                    Network_Error = "Hidden";
                    Loading = "Visible";
                    string cmd = string.Format("SELECT * FROM sys_users WHERE `id`='{0}' AND `password`='{1}'", _id, _pw);
                    MySqlDataReader reader = _server.GetSQLReader(cmd);
                    if (reader != null) {
                        if (reader.Read()) {
                            StarEyesData.ID = _id;
                            StarEyesData.UserName = reader[2].ToString();
                            StarEyesData.Wechat = reader[3].ToString();
                            StarEyesData.Email = reader[4].ToString();
                            StarEyesData.Phone = reader[5].ToString();
                            StarEyesData.Organization = reader[6].ToString();
                            reader.Close();
                            return true;
                        }
                        else {
                            Thread.Sleep(5000);
                            Loading = "Hidden";
                            Auth_Error = "Visible";
                            reader.Close();
                            return false;
                        }
                    }
                    else {
                        HandyControl.Controls.MessageBox.Error("请检查本机网络或防火墙设置。", "登录超时");
                        Loading = "Hidden";
                        Network_Error = "Visible";
                        return false;
                    }
                }
                else {
                    PW_NULL = "Visible";
                    return false;
                }
            }
            else return false;
        }
        #endregion
    }
}
