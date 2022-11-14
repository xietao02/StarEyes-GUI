using Org.BouncyCastle.Tls;
using StarEyes_GUI.Utils;
using StarEyes_GUI.ViewModels.Pages;
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
        public CameraViewModel CameraViewModel;

        #region 摄像头属性

        public bool canConnect = false;
        public bool isVLCOpen = false;
        public bool isEditViewShow = false;
        public VlcControl VLC;

        Assembly currentAssembly;
        string currentDirectory;
        DirectoryInfo vlcLibDirectory;
        string[] options = new string[] { "--file-logging", "-vvv", "--logfile=VLC.log" };
        
        /// <summary>
        /// 摄像头名称
        /// </summary>
        private string _CameraName;
        public string CameraName {
            get { return _CameraName; }
            set {
                _CameraName = value;
                RaisePropertyChanged("CameraName");
            }
        }

        /// <summary>
        /// 摄像头 ID
        /// </summary>
        public string Info_CameraID { get; set; }
        private string _CameraID;
        public string CameraID {
            get { return _CameraID; }
            set {
                _CameraID = value;
                Info_CameraID = "ID：" + CameraID;
                RaisePropertyChanged("Info_CameraID");
            }
        }

        /// <summary>
        /// 摄像头状态
        /// </summary>
        public string Info_CameraStatus { get; set; }
        public string Status_Style { get; set; }
        private bool _CameraStatus = false;
        public bool CameraStatus { 
            get {
                return _CameraStatus;
            }
            set {
                _CameraStatus = value;
                if (value) {
                    Info_CameraStatus = "连接状态：" + "正常";
                    Status_Style = "#29b94c";
                }
                else {
                    Info_CameraStatus = "连接状态：" + "异常";
                    Status_Style = "#dc303e";
                }
                RaisePropertyChanged("Info_CameraID");
                RaisePropertyChanged("Status_Style");
            }
        }

        /// <summary>
        /// 摄像头检测事件数量
        /// </summary>
        public string Info_CameraEventNum { get; set; }
        private string _CameraEventNum;
        public string CameraEventNum {
            get { return _CameraEventNum; }
            set {
                _CameraEventNum = value;
                Info_CameraEventNum = String.Format("累计检测到 {0} 起事件。", value);
                RaisePropertyChanged("Info_CameraEventNum");
                RaisePropertyChanged("CameraEventNum");
            }
        }

        /// <summary>
        /// 摄像头 IP
        /// </summary>
        public string Info_CameraIP { get; set; }
        private string _CameraIP;
        public string CameraIP {
            get { return _CameraIP; }
            set {
                if (CameraStatus) {
                    Info_CameraIP = "IP：" + value;
                    _CameraIP = value;
                }
                else {
                    Info_CameraIP = "未能与摄像头建立连接";
                    _CameraIP = "未知";
                }
                RaisePropertyChanged("Info_CameraIP");
                RaisePropertyChanged("CameraIP");
            }
        }

        /// <summary>
        /// 摄像头端口
        /// </summary>
        private string _CameraPort;
        public string CameraPort {
            get { return _CameraPort; }
            set {
                _CameraPort = value;
                RaisePropertyChanged("CameraPort");
            }
        }

        /// <summary>
        /// 摄像头 RTSP 账号
        /// </summary>
        private string _RTSPAcount;
        public string RTSPAcount {
            get { return _RTSPAcount; }
            set {
                _RTSPAcount = value;
                RaisePropertyChanged("RTSPAcount");
            }
        }

        /// <summary>
        /// 摄像头 RTSP 密码
        /// </summary>
        private string _RTSPPassword;
        public string RTSPPassword {
            get { return _RTSPPassword; }
            set {
                _RTSPPassword = value;
                RaisePropertyChanged("RTSPPassword");
            }
        }


        /// <summary>
        /// 摄像头地理位置
        /// </summary>
        private string _Info_CameraPos;
        public string Info_CameraPos {
            get { return _Info_CameraPos; }
            set {
                string Lon, Lat;
                if (CameraPosLon == "0") Lon = "未知";
                else Lon = CameraPosLon;
                if (CameraPosLat == "0") Lat = "未知";
                else Lat = CameraPosLat;
                _Info_CameraPos = String.Format("经纬度：({0}, {1})", Lon, Lat);
                RaisePropertyChanged("Info_CameraPos");
            }
        }

        private string _CameraPosLat;
        public string CameraPosLat {
            get { return _CameraPosLat; }
            set {
                _CameraPosLat = value;
                Info_CameraPos = "update";
                RaisePropertyChanged("CameraPosLat");
            }
        }
        
        private string _CameraPosLon;
        public string CameraPosLon {
            get { return _CameraPosLon; }
            set {
                _CameraPosLon = value;
                Info_CameraPos = "update";
                RaisePropertyChanged("CameraPosLon");
            }
        }


        /// <summary>
        /// 切换按钮可视性
        /// </summary>
        public string SwitchOpenViewButton_Enable { get; set; }

        private string _SwitchOpenViewButton;
        public string SwitchOpenViewButton {
            get { return _SwitchOpenViewButton; }
            set {
                _SwitchOpenViewButton = value;
                RaisePropertyChanged("SwitchOpenViewButton");
            }
        }

        private string _SwitchCloseViewButton = "False";
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
                VLC.SourceProvider.CreatePlayer(vlcLibDirectory, options);
                string rtsp = String.Format("rtsp://{0}:{1}@{2}:{3}/stream1&channel=1", RTSPAcount, RTSPPassword, CameraIP, CameraPort);
                VLC.SourceProvider.MediaPlayer.Audio.Volume = 0;
                VLC.SourceProvider.MediaPlayer.Play(new Uri(rtsp));
                VLC.Visibility = System.Windows.Visibility.Visible;
                SwitchOpenViewButton = "Hidden";
                SwitchCloseViewButton = "Visible";
                System.Diagnostics.Trace.WriteLine(rtsp);
            }
        }

        bool CanExecuteOpenVLC(object obj) {
            SwitchOpenViewButton = "Visible";
            if (canConnect && CameraStatus) {
                SwitchOpenViewButton_Enable = "True";
                return true;
            }
            else {
                SwitchOpenViewButton_Enable = "False";
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
                SwitchOpenViewButton = "Visible";
                SwitchCloseViewButton = "Hidden";
            }
        }

        bool CanExecuteCloseVLC(object obj) {
            SwitchCloseViewButton = "Hidden";
            return true;    
        }

        public DelegateCommand SwitchVolume => new DelegateCommand(ExecuteSwitchVolume);

        void ExecuteSwitchVolume(object obj) {
            System.Windows.Controls.Primitives.ToggleButton button = obj as System.Windows.Controls.Primitives.ToggleButton;
            if ((bool)button.IsChecked) {
                VLC.SourceProvider.MediaPlayer.Audio.Volume = 100;
            }
            else {
                VLC.SourceProvider.MediaPlayer.Audio.Volume = 0;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public CameraItemViewModel(string ID, string Name, string Status, string PosLon, string PosLat,
                                    string IP, string Port, string RTSPAcount, string RTSPPassword, string EventNum) {
            CameraID = ID;
            if (Status == "1") CameraStatus = true;
            else CameraStatus = false;
            CameraName = Name;
            CameraEventNum = EventNum;
            CameraPosLat = PosLat;
            CameraPosLon = PosLon;
            CameraIP = IP;
            CameraPort = Port;
            this.RTSPAcount = RTSPAcount;
            this.RTSPPassword = RTSPPassword;

            currentAssembly = Assembly.GetEntryAssembly();
            currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            vlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        }
    }
}