using HandyControl.Controls;
using MySql.Data.MySqlClient;
using System;

namespace StarEyes_GUI.Utils {
    /// <summary>
    /// 服务器连接
    /// </summary>
    public class StarEyesServer {
        private readonly string connectStr = String.Format("server={0};port={1};user={2};password={3};database={4};CharSet=utf8;",
            SE.server, SE.port, SE.user, SE.password, SE.database);
        public bool status;
        public static MySqlConnection? connection;
        public bool ConnectServer() {
            MySqlConnection conn = new(connectStr);
            try {
                conn.Open();
                System.Diagnostics.Trace.WriteLine("Database connection succeeded!");
                status = true;
                connection = conn;
                return true;
            }
            catch (MySqlException ex) {
                HandleException(ex);
                status = false;
                return false;
            }
        }

        public static void HandleException(MySqlException ex) {
            if (ex.Number == 1042) MessageBox.Warning("Please check network connection.");
            else System.Diagnostics.Trace.WriteLine("[" + ex.Number + "]" + ex.Message);
        }

        public StarEyesServer() {
            ConnectServer();
        }

        public bool SQLExecuteNonQuery(string cmd) {
            if (!status) return false;
            else {
                MySqlCommand Cmd = new(cmd, connection);
                try {
                    System.Diagnostics.Trace.WriteLine(cmd);
                    int i = Cmd.ExecuteNonQuery();
                    System.Diagnostics.Trace.WriteLine("Result: " + i);
                    return true;
                }
                catch (MySqlException ex) {
                    HandleException(ex);
                    return false;
                }
            }
        }

        public MySqlDataReader SQLExecuteReader(string cmd) {
            if (!status) return null;
            else {
                MySqlCommand Cmd = new(cmd, connection);
                try {
                    System.Diagnostics.Trace.WriteLine(cmd);
                    MySqlDataReader reader = Cmd.ExecuteReader();
                    return reader;
                }
                catch (MySqlException ex) {
                    HandleException(ex);
                    return null;
                }
            }
        }

    }
}
