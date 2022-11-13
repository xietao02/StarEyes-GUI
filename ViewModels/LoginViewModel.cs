using MySql.Data.MySqlClient;
using StarEyes_GUI.Utils;
using StarEyes_GUI.Models;
using System.Windows.Navigation;

namespace StarEyes_GUI.ViewModels
{

    /// <summary>
    /// 登陆界面交互逻辑
    /// </summary>
    public class LoginViewModel : NotificationObject {

        private StarEyesServer Server = new();

        public string ID { get; set; }
        public string PW;
        public bool Auth = false;
        public bool Status;


        /// <summary>
        /// 服务器验证登录权限
        /// </summary>
        public DelegateCommand LoginAuthCommand => new DelegateCommand(obj => {
            string cmd = string.Format("SELECT * FROM sys_users WHERE `id`='{0}' AND `password`='{1}'", ID, PW);
            MySqlDataReader reader = Server.GetSQLReader(cmd);
            if (reader != null) {
                Status = true;
                if (reader.Read()) {
                    Auth = true;
                    StarEyesModel.ID = ID;
                }
                else reader.Close();
            }
            else Status = false;
        });

    }
}

