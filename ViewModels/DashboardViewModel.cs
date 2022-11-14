using StarEyes_GUI.Models;
using StarEyes_GUI.Utils;
using System.Threading;
using System.Device.Location;
using System;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.Views;
using MySql.Data.MySqlClient;
using System.Windows;

namespace StarEyes_GUI.ViewModels {
    public class DashboardViewModel : NotificationObject {
        public PageTransition PagesPresenter;
        public Header Header;
        public SideBar Sidebar;
        public LoginView loginView;
        private Thread StarEyesUpdateThread;
        private void StartUpdateThread() {
            StarEyesUpdateThread = new(new ThreadStart(() => {

            }));
            StarEyesUpdateThread.IsBackground = true;
            StarEyesUpdateThread.Start();
        }
        private void StopUpdateThread() {
            StarEyesUpdateThread.Abort();
        }

        /// <summary>
        /// 同步服务器函数
        /// </summary>
        public void UpdateStarEyes() {
            // 同步 CameraView
            //PagesPresenter.Pages[1];
        }
        
        public DashboardViewModel() {
            //StartUpdate();
        }

        ~DashboardViewModel() {
            //StopUpdate();
        }
    }
}