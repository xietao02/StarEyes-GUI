using StarEyes_GUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEyes_GUI.ViewModels.Pages {
    public class CameraViewModel : PageViewModelBase{


        /// <summary>
        /// 服务器验证登录权限
        /// </summary>
        public DelegateCommand LoginAuthCommand => new DelegateCommand(obj => {
            if (!server.status) server.ConnectServer();
            else {
                string cmd = string.Format("SELECT * FROM sys_users WHERE `id`='{0}' AND `password`='{1}'", ID, PW);
                try {
                    MySqlDataReader reader = server.SQLExecuteReader(cmd);
                    if (reader != null) {
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
