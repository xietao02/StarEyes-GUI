using HandyControl.Controls;
using MySql.Data.MySqlClient;
using System;

namespace StarEyes_GUI.Utils {
    /// <summary>
    /// 服务器连接
    /// </summary>
    public class StarEyesServer {
        private readonly static string connectStr = String.Format("server={0};port={1};user={2};password={3};database={4};CharSet=utf8;",
            SE.server, SE.port, SE.user, SE.password, SE.database);

        public static bool status = false;

        private static MySqlConnection? _connection;
        public MySqlConnection? connection {
            get {
                System.Diagnostics.Trace.WriteLine("connection.State: " + _connection.State);
                if (_connection == null || !status) {
                    if (ConnectServer()) {
                        return _connection;
                    }
                    else {
                        System.Diagnostics.Trace.WriteLine("数据库连接失败！");
                        return null;
                    }
                }
                else {
                    return _connection;
                }
            }
            
            set {
                _connection = value;
            }
        }

        private bool ConnectServer() {
            connection = new(connectStr);
            try {
                connection.Open();
                System.Diagnostics.Trace.WriteLine("数据库连接成功！");
                status = true;
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

        public bool SQLExecuteNonQuery(string cmd) {
            if (!status) return false;
            else {
                MySqlCommand Cmd = new(cmd, connection);
                try {
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

        public MySqlDataReader? SQLExecuteReader(string cmd) {
            if (!status) {
                if (!ConnectServer()) return null;

            }
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
