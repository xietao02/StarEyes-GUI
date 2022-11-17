using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.ViewModels.Pages;
using StarEyes_GUI.Views.Pages.Dialogs;
using Vlc.DotNet.Wpf;

namespace StarEyes_GUI.UserControls.UCViewModels {
    public class CameraItemViewModel : NotificationObject {
        public CameraViewModel CameraViewModel;

        #region 摄像头属性

        public bool CanConnect = true;
        public bool IsVLCOpen = false;
        public bool IsEditViewShow = false;
        public VlcControl VLC;
        
        private string _rtsp;
        private Assembly _currentAssembly;
        private string _currentDirectory;
        private DirectoryInfo _vlcLibDirectory;
        private string[] _options = new string[] { "--file-logging", "-vvv", "--logfile=VLC.log" };
        
        /// <summary>
        /// 摄像头名称
        /// </summary>
        private string _cameraName;
        public string CameraName {
            get { return _cameraName; }
            set {
                _cameraName = value;
                RaisePropertyChanged("CameraName");
            }
        }

        /// <summary>
        /// 摄像头 id
        /// </summary>
        public string Info_CameraID { get; set; }
        private string _cameraID;
        public string CameraID {
            get { return _cameraID; }
            set {
                _cameraID = value;
                Info_CameraID = "id：" + CameraID;
                RaisePropertyChanged("Info_CameraID");
            }
        }

        /// <summary>
        /// 摄像头状态
        /// </summary>
        public string Info_CameraStatus { get; set; }
        public string Status_Style { get; set; }
        private bool _cameraStatus = false;
        public bool CameraStatus { 
            get {
                return _cameraStatus;
            }
            set {
                _cameraStatus = value;
                if (value) {
                    Info_CameraStatus = "连接状态：正常";
                    Status_Style = "#29b94c";
                }
                else {
                    Info_CameraStatus = "连接状态：异常";
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
        private string _cameraEventNum;
        public string CameraEventNum {
            get { return _cameraEventNum; }
            set {
                _cameraEventNum = value;
                Info_CameraEventNum = String.Format("累计检测到 {0} 起事件。", value);
                RaisePropertyChanged("Info_CameraEventNum");
                RaisePropertyChanged("CameraEventNum");
            }
        }

        /// <summary>
        /// 摄像头 ip
        /// </summary>
        public string Info_CameraIP { get; set; }
        private string _cameraIP;
        public string CameraIP {
            get { return _cameraIP; }
            set {
                if (CameraStatus) {
                    Info_CameraIP = "ip：" + value;
                    _cameraIP = value;
                }
                else {
                    Info_CameraIP = "未能与摄像头建立连接";
                    _cameraIP = "未知";
                }
                RaisePropertyChanged("Info_CameraIP");
                RaisePropertyChanged("CameraIP");
            }
        }

        /// <summary>
        /// 摄像头端口
        /// </summary>
        private string _cameraPort;
        public string CameraPort {
            get { return _cameraPort; }
            set {
                _cameraPort = value;
                RaisePropertyChanged("CameraPort");
            }
        }

        /// <summary>
        /// 摄像头 RTSP 账号
        /// </summary>
        private string _rtspAcount;
        public string RTSPAcount {
            get { return _rtspAcount; }
            set {
                _rtspAcount = value;
                RaisePropertyChanged("rtspAcount");
            }
        }

        /// <summary>
        /// 摄像头 RTSP 密码
        /// </summary>
        private string _rtspPassword;
        public string RTSPPassword {
            get { return _rtspPassword; }
            set {
                _rtspPassword = value;
                RaisePropertyChanged("rtspPassword");
            }
        }


        /// <summary>
        /// 摄像头地理位置
        /// </summary>
        private string _info_CameraPos;
        public string Info_CameraPos {
            get { return _info_CameraPos; }
            set {
                string Lon, Lat;
                if (CameraPosLon == "0") Lon = "未知";
                else Lon = CameraPosLon;
                if (CameraPosLat == "0") Lat = "未知";
                else Lat = CameraPosLat;
                _info_CameraPos = String.Format("经纬度：({0}, {1})", Lon, Lat);
                RaisePropertyChanged("Info_CameraPos");
            }
        }

        private string _cameraPosLat;
        public string CameraPosLat {
            get { return _cameraPosLat; }
            set {
                _cameraPosLat = value;
                Info_CameraPos = "update";
                RaisePropertyChanged("CameraPosLat");
            }
        }
        
        private string _cameraPosLon;
        public string CameraPosLon {
            get { return _cameraPosLon; }
            set {
                _cameraPosLon = value;
                Info_CameraPos = "update";
                RaisePropertyChanged("CameraPosLon");
            }
        }


        /// <summary>
        /// 切换按钮可视性
        /// </summary>
        public string SwitchOpenViewButton_Enable { get; set; }

        private string _switchOpenViewButton;
        public string SwitchOpenViewButton {
            get { return _switchOpenViewButton; }
            set {
                _switchOpenViewButton = value;
                RaisePropertyChanged("SwitchOpenViewButton");
            }
        }

        private string _switchCloseViewButton = "False";
        public string SwitchCloseViewButton {
            get { return _switchCloseViewButton; }
            set {
                _switchCloseViewButton = value;
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
            if (!IsVLCOpen) {
                IsVLCOpen = true;
                VLC.SourceProvider.MediaPlayer.Audio.Volume = 0;
                VLC.SourceProvider.MediaPlayer.Play(new Uri(_rtsp));
                VLC.Visibility = System.Windows.Visibility.Visible;
                SwitchOpenViewButton = "Hidden";
                SwitchCloseViewButton = "Visible";
                System.Diagnostics.Trace.WriteLine(_rtsp);
            }
        }

        bool CanExecuteOpenVLC(object obj) {
            SwitchOpenViewButton = "Visible";
            if (CanConnect && CameraStatus) {
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

        public void ExecuteCloseVLC(object obj) {
            if (IsVLCOpen) {
                IsVLCOpen = false;
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

        /// <summary>
        /// 切换视频流音频开关
        /// </summary>
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


        /// <summary>
        /// 打开编辑窗口
        /// </summary>
        public DelegateCommand EditCamera => new DelegateCommand(ExecuteEditCamera, CanExecuteEditCamera);

        void ExecuteEditCamera(object obj) {
            IsEditViewShow = true;
            EditCameraItemView editCameraItemView = new(this);
            editCameraItemView.Show();
        }

        bool CanExecuteEditCamera(object obj) {
            if (!IsEditViewShow) return true;
            else {
                HandyControl.Controls.MessageBox.Info("修改信息窗口已打开！", "提示");
                return false;
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public CameraItemViewModel(string id,
            string name,
            string status,
            string posLon,
            string posLat,
            string ip,
            string port,
            string rtspAcount,
            string rtspPassword,
            string eventNum) {
            CameraID = id;
            CameraStatus = status == "1" ? true : false;
            CameraName = name;
            CameraEventNum = eventNum;
            CameraPosLat = posLat;
            CameraPosLon = posLon;
            CameraIP = ip;
            CameraPort = port;
            RTSPAcount = rtspAcount;
            RTSPPassword = rtspPassword;

            _currentAssembly = Assembly.GetEntryAssembly();
            _currentDirectory = new FileInfo(_currentAssembly.Location).DirectoryName;
            _vlcLibDirectory = new DirectoryInfo(Path.Combine(_currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        }

        /// <summary>
        /// 初始化 VLC
        /// </summary>
        public void InitVLC(VlcControl vlc) {
            VLC = vlc;
            VLC.SourceProvider.CreatePlayer(_vlcLibDirectory, _options);
            _rtsp = String.Format("_rtsp://{0}:{1}@{2}:{3}/stream1&channel=1",
                RTSPAcount, RTSPPassword, CameraIP, CameraPort);
            CheckVLCConnection();
        }

        /// <summary>
        /// 检查 VLC 连接状态
        /// </summary>
        public void CheckVLCConnection() {
            // todo
        }
        #endregion
    }
}