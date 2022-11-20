using System;
using System.Threading;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.Views;

namespace StarEyes_GUI.ViewModels {
    public class DashboardViewModel : NotificationObject {
        public PagePresenter PagesPresenter = new();
        public Header Header = new();
        public Sidebar Sidebar = new();
        public LoginView LoginView;
        private Thread StarEyesUpdateThread;

        /// <summary>
        /// 启动同步进程
        /// </summary>
        private void StartUpdateThread() {
            StarEyesUpdateThread = new(new ThreadStart(() => {
                bool isTipShown = false;
                while (true) {
                    Console.WriteLine("开始更新");
                    if (UpdateStarEyes()) {
                        isTipShown = false;
                        Thread.Sleep(10000);
                    }
                    else {
                        if (!isTipShown) {
                            HandyControl.Controls.MessageBox.Error("无法连接服务器，数据同步失败！", "网络错误");
                            isTipShown = true;
                        }
                        Thread.Sleep(5000);
                    }
                }
            }));
            StarEyesUpdateThread.IsBackground = true;
            StarEyesUpdateThread.Start();
        }

        /// <summary>
        /// 暂停同步进程
        /// </summary>
        private void StopUpdateThread() {
            StarEyesUpdateThread.Abort();
        }

        /// <summary>
        /// 同步服务器函数
        /// </summary>
        public bool UpdateStarEyes() {
            bool UpdateStatus = false;

            // 同步 CameraView
            UpdateStatus = PagesPresenter.CameraView.CameraViewModel.SycCameraView();
            return UpdateStatus;
        }

        public DashboardViewModel() {
            StartUpdateThread();
        }

        public void DestructDashboardVM() {
            StopUpdateThread();

            // 关闭视频流
            PagesPresenter.CameraView.CameraViewModel.CloseVideoStream();
        }
    }
}