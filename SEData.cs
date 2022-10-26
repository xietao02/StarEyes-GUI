using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using HandyControl.Controls;

namespace StarEyes_GUI {
    internal class LocalData {
        public static string? ID { get; set; }
    }

    internal class ServerData {
        private readonly string  connectStr = "server=" + SE.server + ";port=" + SE.port + ";user=" + SE.user
            + ";password=" + SE.password + ";database=" + SE.database + ";";
        public bool ConnectServer() {
            MySqlConnection conn = new(connectStr);
            try {
                conn.Open();
                System.Diagnostics.Trace.WriteLine("Database connection succeeded!");
                return true;
            }
            catch (MySqlException ex) {
                HandleException(ex);
                return false;
            }
        }

        private void HandleException(MySqlException ex) {
            if (ex.Number == 1042) MessageBox.Warning("Please check network connection.");
            else MessageBox.Warning(ex.Message);
        }
    }
}
