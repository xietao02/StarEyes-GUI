using HandyControl.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEyes_GUI.Models {
    public static class LocalData {
        public static string? ID { get; set; }
    }

    public class Server {
        private readonly string connectStr = "server=" + SE.server + ";port=" + SE.port + ";user=" + SE.user
            + ";password=" + SE.password + ";database=" + SE.database + ";charset=utf8mb4;";

        public bool status;
        public static MySqlConnection? connection;
        public bool ConnectServer() {
            MySqlConnection conn = new(connectStr);
            try {
                conn.Open();
                System.Diagnostics.Trace.WriteLine("Database connection succeeded!");
                status = true;
                connection = conn;
                string cmd = "SELECT * FROM `sys_users` WHERE `id` = '10001' AND `password` = '00000';";
                System.Diagnostics.Trace.WriteLine(cmd);
                MySqlCommand Cmd = new(cmd, Server.connection);
                int i = Cmd.ExecuteNonQuery();
                System.Diagnostics.Trace.WriteLine(i);

                cmd = "SELECT * FROM `sys_users`;";
                System.Diagnostics.Trace.WriteLine(cmd);
                Cmd = new(cmd, Server.connection);
                i = Cmd.ExecuteNonQuery();
                System.Diagnostics.Trace.WriteLine(i);
                return true;
            }
            catch (MySqlException ex) {
                HandleException(ex);
                status = false;
                return false;
            }
        }

        private static void HandleException(MySqlException ex) {
            if (ex.Number == 1042) MessageBox.Warning("Please check network connection.");
            else MessageBox.Warning(ex.Message);
        }

        public Server() {
            ConnectServer();
        }

        public bool SQLExecuteNonQuery (string cmd) {
            if (!status) return false;
            else {
                MySqlCommand Cmd = new(cmd, connection);
                try {
                    Cmd.ExecuteNonQuery();
                    return true;
                }
                catch (MySqlException ex) {
                    HandleException(ex);
                    return false;
                }
            }
        }
        
    }
}
