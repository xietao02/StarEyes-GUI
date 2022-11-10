using MySql.Data.MySqlClient;
using StarEyes_GUI.Utils;
using StarEyes_GUI.Models;

namespace StarEyes_GUI.ViewModels
{

    /// <summary>
    /// 登陆界面交互逻辑
    /// </summary>
    public class LoginViewModel : NotificationObject {

        public StarEyesServer server;

        public string? ID { get; set; }
        public string PW;
        public bool Auth;
        
        public LoginViewModel() {
            server = new();
            Auth = false;
        }

        /// <summary>
        /// 服务器验证登录权限
        /// </summary>
        public DelegateCommand LoginAuthCommand => new DelegateCommand(obj => {
			if (!server.status) server.ConnectServer();
			else {
                string cmd = string.Format("SELECT * FROM sys_users WHERE `id`='{0}' AND `password`='{1}'", ID, PW);
				try {
					MySqlDataReader reader = server.SQLExecuteReader(cmd);
                    if(reader != null) {
                        if (reader.Read()) {
                            Auth = true;
                            StarEyesModel.ID = ID;
                        }
                        else Auth = false;
                        reader.Close();
                    }
                }
                catch (MySqlException ex) {
                    StarEyesServer.HandleException(ex);
                }
                
            }
        });
    }
}

