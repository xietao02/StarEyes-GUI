using MySql.Data.MySqlClient;
using StarEyes_GUI.Commands;
using StarEyes_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StarEyes_GUI.ViewModels {
	public class LoginViewModel : NotificationObject {
        private LoginModel _LoginModel = new();
		Server server;
        public LoginModel LoginModel {
			get { return _LoginModel; }
			set {
				_LoginModel = value;
				RaisePropertyChanged("LoginModel");
			}
		}

        
		public DelegateCommand LoginAuthCommand => new DelegateCommand(obj => {
			if (!server.status) server.ConnectServer();
			if (server.status) {
                string cmd = "SELECT * FROM `sys_users` WHERE `id` = '" + LoginModel.ID + "' AND `password` = '" + LoginModel.PW + "';";
				System.Diagnostics.Trace.WriteLine(cmd);
                MySqlCommand Cmd = new(cmd, Server.connection);
				int i = Cmd.ExecuteNonQuery();
				System.Diagnostics.Trace.WriteLine(i);

                cmd = "SELECT * FROM `sys_users`;";
                System.Diagnostics.Trace.WriteLine(cmd);
                Cmd = new(cmd, Server.connection);
                i = Cmd.ExecuteNonQuery();
                System.Diagnostics.Trace.WriteLine(i);
            }
        });
        
        public LoginViewModel() {
			server = new();
		}
    }
}

