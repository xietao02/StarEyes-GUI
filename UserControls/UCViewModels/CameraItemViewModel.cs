using Org.BouncyCastle.Tls;
using StarEyes_GUI.Utils;
using StarEyes_GUI.Views.Pages.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vlc.DotNet.Wpf;

namespace StarEyes_GUI.UserControls.UCViewModels {
    public class CameraItemViewModel : NotificationObject {
        public bool Local;
        public string CameraID;
        public bool CameraStatus;
        public string CameraEventNum;
        public string CameraIP;
        public string CameraPort;
        public string RTSPAcount;
        public string RTSPPassword;
        public string CameraPosLat;
        public string CameraPosLon;
        public bool isVLCOpen = false;
        public bool isEditViewShow = false;
        VlcControl VLC;

        #region 依赖属性
        private string _CameraName;
        public string CameraName {
            get { return _CameraName; }
            set {
                _CameraName = value;
                RaisePropertyChanged("CameraName");
            }
        }


        private string _Info_CameraID;
        public string Info_CameraID {
            get { return _Info_CameraID; }
            set {
                _Info_CameraID = value;
                RaisePropertyChanged("Info_CameraID");
            }
        }

        private string _Info_CameraStatus;
        public string Info_CameraStatus {
            get { return _Info_CameraStatus; }
            set {
                _Info_CameraStatus = value;
                RaisePropertyChanged("Info_CameraStatus");
            }
        }

        private string _Info_CameraPos;
        public string Info_CameraPos {
            get { return _Info_CameraPos; }
            set {
                _Info_CameraPos = value;
                RaisePropertyChanged("Info_CameraPos");
            }
        }

        private string _Info_CameraEventNum;
        public string Info_CameraEventNum {
            get { return _Info_CameraEventNum; }
            set {
                _Info_CameraEventNum = value;
                RaisePropertyChanged("Info_CameraEventNum");
            }
        }

        private string _Info_CameraIP;
        public string Info_CameraIP {
            get { return _Info_CameraIP; }
            set {
                _Info_CameraIP = value;
                RaisePropertyChanged("Info_CameraIP");
            }
        }

        private string _SwitchOpenViewButton;
        public string SwitchOpenViewButton {
            get { return _SwitchOpenViewButton; }
            set {
                _SwitchOpenViewButton = value;
                RaisePropertyChanged("SwitchOpenViewButton");
            }
        }

        private string _SwitchCloseViewButton;
        public string SwitchCloseViewButton {
            get { return _SwitchCloseViewButton; }
            set {
                _SwitchCloseViewButton = value;
                RaisePropertyChanged("SwitchCloseViewButton");
            }
        }
        #endregion

        #region 命令
        /// <summary>
        /// 打开摄像头视频流
        /// </summary>
        public DelegateCommand OpenVLC => new DelegateCommand(ExecuteOpenVLC, CanExecuteOpenVLC);

        void ExecuteOpenVLC(object obj) {
            if (!isVLCOpen) {
                isVLCOpen = true;
                VLC.SourceProvider.MediaPlayer.Play(new Uri("rtsp://admin:Aa123456@192.168.1.115:554/stream1&channel=1"));
                VLC.Visibility = System.Windows.Visibility.Visible;
                SwitchOpenViewButton = "False";
                SwitchCloseViewButton = "True";
            }
        }

        bool CanExecuteOpenVLC(object obj) {
            if (Local && CameraStatus) {
                return true;
            }
            else {
                return false;
            }
        }
        

        /// <summary>
        /// 关闭摄像头视频流
        /// </summary>
        public DelegateCommand CloseVLC => new DelegateCommand(ExecuteCloseVLC, CanExecuteCloseVLC);

        void ExecuteCloseVLC(object obj) {
            if (isVLCOpen) {
                isVLCOpen = false;
                new Task(() => {
                    VLC.SourceProvider.MediaPlayer.Stop();
                }).Start();
                VLC.Visibility = System.Windows.Visibility.Hidden;
                SwitchOpenViewButton = "True";
                SwitchCloseViewButton = "False";
            }
        }

        bool CanExecuteCloseVLC(object obj) {
            if (Local && CameraStatus) {
                return true;
            }
            else {
                return false;
            }
        }
        #endregion


        public CameraItemViewModel(VlcControl VLC) {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var vlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            var options = new string[]{
                     "--file-logging", "-vvv", "--logfile=VLCLogs.log"
                };
            VLC.SourceProvider.CreatePlayer(vlcLibDirectory, options);
            this.VLC = VLC;
        }
    }
}