using MySql.Data.MySqlClient;
using StarEyes_GUI.Utils;
using StarEyes_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace StarEyes_GUI.ViewModels
{

    /// <summary>
    /// 登陆界面交互逻辑
    /// </summary>
    public class LoginViewModel : NotificationObject {

        public StarEyesServer server;
        
        private LoginModel _LoginModel = new();
        public LoginModel LoginModel {
			get { return _LoginModel; }
			set {
				_LoginModel = value;
				RaisePropertyChanged("LoginModel");
			}
		}
        
        /// <summary>
        /// 服务器验证登录权限
        /// </summary>
		public DelegateCommand LoginAuthCommand => new DelegateCommand(obj => {
			if (!server.status) server.ConnectServer();
			else {
                string cmd = string.Format("SELECT * FROM sys_users WHERE `id`='{0}' AND `password`='{1}'", LoginModel.ID, LoginModel.PW);
				try {
					MySqlDataReader reader = server.SQLExecuteReader(cmd);
                    if(reader != null) {
                        if (reader.Read()) {
                            LoginModel.Auth = true;
                            StarEyesModel.ID = LoginModel.ID;
                        }
                        else LoginModel.Auth = false;
                        reader.Close();
                    }
                }
                catch (MySqlException ex) {
                    StarEyesServer.HandleException(ex);
                }
                
            }
        });
        
        public LoginViewModel() {
			server = new();
		}
    }
}

