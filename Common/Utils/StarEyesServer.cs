using HandyControl.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading;
using System.Xml;

namespace StarEyes_GUI.Common.Utils {
    /// <summary>
    /// 服务器连接
    /// </summary>
    public class StarEyesServer {
        
        private readonly static string connectStr = String.Format("server={0};port={1};user={2};password={3};database={4};CharSet=utf8;",
            SE.server, SE.port, SE.user, SE.password, SE.database);

        /// <summary>
        /// 封装好的静态的数据库连接
        /// </summary>
        private static MySqlConnection _connection = null;
        public MySqlConnection Connection {
            get {
                if (_connection == null) {
                    if (InitConnection()) {
                        return _connection;
                    }
                    else return null;
                }
                else if (_connection.State == ConnectionState.Closed) {
                    if (ReOpenConnection(3)) {
                        return _connection;
                    }
                    else return null;
                }
                else if (_connection.State == ConnectionState.Open) {
                    return _connection;
                }
                else return null;
            }
            
            set {
                _connection = value;
            }
        }


        #region 数据库使用方法
        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <returns></returns>
        private bool InitConnection() {
            _connection = new(connectStr);
            return ReOpenConnection(3);
        }
        
        /// <summary>
        /// 数据库连接重连方法
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        private bool ReOpenConnection(int times = 1) {
            if (times > 0) {
                times--;
                if (_connection != null) {
                    try {
                        _connection.Open();
                        return true;
                    }
                    catch (MySqlException ex) {
                        Console.WriteLine("数据库连接失败：[" + ex.Number + "]" + ex.Message + "剩余自动重连次数：" + times.ToString());
                        Thread.Sleep(1000);
                        return ReOpenConnection(times);
                    }
                }
                else {
                    return InitConnection();
                }
            }
            else return false;
        }

        /// <summary>
        /// 执行查询类的数据库操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public MySqlDataReader GetSQLReader(string cmd) {
            if (Connection != null) {
                MySqlCommand Cmd = new(cmd, Connection);
                try {
                    return Cmd.ExecuteReader();
                }
                catch (MySqlException ex) {
                    Console.WriteLine("数据库操作异常：[" + ex.Number + "]" + ex.Message);
                    return null;
                }
            }
            else return null;
        }

        /// <summary>
        /// 执行非查询类的数据库操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuerySQL(string cmd) {
            if (Connection != null) {
                MySqlCommand Cmd = new(cmd, Connection);
                try {
                    return Cmd.ExecuteNonQuery(); ;
                }
                catch (MySqlException ex) {
                    Console.WriteLine("数据库操作异常：[" + ex.Number + "]" + ex.Message);
                    return -1;
                }
            }
            else return -1;
        }

        public int ExecuteNonQuerySQL(string[] cmds) {
            int rows = -1;
            if (Connection != null) {
                for (int i = 0; i < cmds.Length; i++) {
                    if (cmds[i] != null) {
                        MySqlCommand Cmd = new(cmds[i], Connection);
                        try {
                            rows += Cmd.ExecuteNonQuery();
                        }
                        catch (MySqlException ex) {
                            Console.WriteLine("数据库操作异常：[" + ex.Number + "]" + ex.Message);
                            return -1;
                        }
                    }
                }
                return rows;
            }
            else return -1;
        }
        #endregion
        
    }
}
