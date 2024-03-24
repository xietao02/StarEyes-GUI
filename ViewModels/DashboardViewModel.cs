using System;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Windows;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.Views;

namespace StarEyes_GUI.ViewModels {
    public class DashboardViewModel : NotificationObject {
        public PagePresenter PagePresenter = new();
        public Header Header = new();
        public Sidebar Sidebar = new();
        public LoginView LoginView;
        private bool UpdateStatus = true;
        private Thread StarEyesUpdateThread;

        /// <summary>
        /// 启动同步进程
        /// </summary>
        private void StartUpdateThread() {
            StarEyesUpdateThread = new(new ThreadStart(() => {
                bool isTipShown = false;
                while (true) {
                    //System.Diagnostics.Debug.WriteLine("StarEyes更新开始");
                    UpdateStarEyes();
                    Thread.Sleep(10000);
                    //Thread.Sleep(5000);
                    if (PagePresenter.CameraView.CameraViewModel.UpdateStatus) {
                        UpdateStatus = true;
                    }
                    else UpdateStatus = false;
                    if (UpdateStatus) {
                        isTipShown = false;
                        Sidebar.OfflineMode(false);
                        Thread.Sleep(20000);
                    }
                    else {
                        if (!isTipShown) {
                            HandyControl.Controls.MessageBox.Error("无法连接服务器，数据同步失败！", "网络错误");
                            isTipShown = true;
                            Sidebar.OfflineMode(true);
                        }
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
        public void UpdateStarEyes() {
            PagePresenter.CameraView.CameraViewModel.SycCameraView();
            PagePresenter.OverviewView.SycOverviewView();
            Header.HeaderViewModel.CheckEventNum(PagePresenter.EventView.SycEventView());
        }

        public DashboardViewModel() {
            StartUpdateThread();
        }

        public void DestructDashboardVM() {
            StopUpdateThread();

            // 关闭所有摄像头组件的活动
            PagePresenter.CameraView.CameraViewModel.DisposeVideoItem();
        }
    }
}