using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using OpenCvSharp;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.ViewModels.Pages;
using StarEyes_GUI.Views.Pages.Dialogs;
using Vlc.DotNet.Wpf;

namespace StarEyes_GUI.UserControls.UCViewModels {
    public class CameraItemViewModel : NotificationObject {
        public CameraViewModel CameraViewModel;
        private Thread UploadVideoStreamThread;

        #region 摄像头属性

        public bool IsVLCOpen = false;
        public bool IsEditViewShow = false;
        private enum UploadStatusEnum {
            cannotUpload,
            notUpload,
            uploadBySelf,
            uploadByOther
        }
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
                    Info_CameraStatus = "状态：正常";
                    Status_Style = "#29b94c";
                    if(UploadStatus == (int)UploadStatusEnum.notUpload || UploadStatus == (int)UploadStatusEnum.cannotUpload) {
                        UploadStatus = (int)UploadStatusEnum.uploadByOther;
                    }
                }
                else {
                    if(UploadStatus != (int)UploadStatusEnum.cannotUpload) {
                        UploadStatus = (int)UploadStatusEnum.notUpload;
                    }
                    Info_CameraStatus = "状态：异常";
                    Status_Style = "#dc303e";
                }
                RaisePropertyChanged("Info_CameraStatus");
                RaisePropertyChanged("Status_Style");
            }
        }

        /// <summary>
        /// 摄像头上传状态
        /// </summary>
        private int _uploadStatus = (int)UploadStatusEnum.notUpload;
        public int UploadStatus {
            get { return _uploadStatus; }
            set {
                _uploadStatus = value;
                if(value == (int)UploadStatusEnum.cannotUpload) {
                    UploadVideoButtonEnable = "True";
                    UploadVideoButtonContent = "无法上传视频流 | 重试";
                }
                else if(value == (int)UploadStatusEnum.notUpload) {
                    UploadVideoButtonEnable = "True";
                    UploadVideoButtonContent = "上传视频流至服务器";
                }
                else if(value == (int)UploadStatusEnum.uploadBySelf) {
                    UploadVideoButtonEnable = "True";
                    UploadVideoButtonContent = "视频流上传中 | 停止";
                }
                else{
                    UploadVideoButtonEnable = "False";
                    UploadVideoButtonContent = "视频流已由其他客户端上传";
                }
            }
        }

        /// <summary>
        /// 上传视频至服务器的按钮内容
        /// </summary>
        private string _uploadVideoButtonContent;
        public string UploadVideoButtonContent {
            get { return _uploadVideoButtonContent; }
            set {
                _uploadVideoButtonContent = value;
                RaisePropertyChanged("UploadVideoButtonContent");
            }
        }

        /// <summary>
        /// 上传视频至服务器的按钮可用逻辑判断
        /// </summary>
        private string _uploadVideoButtonEnable;
        public string UploadVideoButtonEnable {
            get { return _uploadVideoButtonEnable; }
            set {
                _uploadVideoButtonEnable = value;
                RaisePropertyChanged("UploadVideoButtonEnable");
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
                _cameraIP = value;
                if (CameraStatus) {
                    Info_CameraIP = "ip：" + value;
                }
                else {
                    Info_CameraIP = "ip：非当前局域网内";
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

        private string _vlcButtonContent = "尝试建立RTSP会话中";
        public string VLCButtonContent {
            get { return _vlcButtonContent; }
            set {
                _vlcButtonContent = value;
                RaisePropertyChanged("VLCButtonContent");
            }
        }

        private string _vlcButtonEnable = "True";
        public string VLCButtonEnable {
            get { return _vlcButtonEnable; }
            set {
                _vlcButtonEnable = value;
                RaisePropertyChanged("VLCButtonEnable");
            }
        }

        /// <summary>
        /// 视频音量键 Visibility
        /// </summary>
        private string _volumeButtonVisibility = "Hidden";
        public string VolumeButtonVisibility {
            get { return _volumeButtonVisibility; }
            set {
                _volumeButtonVisibility = value;
                RaisePropertyChanged("VolumeButtonVisibility");
            }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 上传视频至服务器
        /// </summary>
        public DelegateCommand UploadVideoStream => new DelegateCommand(ExecuteUploadVideoStream);

        void ExecuteUploadVideoStream(object obj) {
            if (UploadStatus == (int)UploadStatusEnum.notUpload || UploadStatus == (int)UploadStatusEnum.cannotUpload) {
                UploadVideoButtonContent = "尝试上传视频流中";
                Console.WriteLine("尝试上传视频流中");
                new Task(() => {
                    if (CheckVLCConnection()) {
                        UploadStatus = (int)UploadStatusEnum.uploadBySelf;
                        UploadVideoStreamThread = new Thread(new ThreadStart(() => {
                            //VideoCapture videoCapture = new VideoCapture(_rtsp, VideoCaptureAPIs.FFMPEG);
                            VideoCapture videoCapture = new VideoCapture(0, VideoCaptureAPIs.ANY);
                            Mat frame = new Mat();
                            //if (videoCapture.Open("rtsp://admin:Aa123456@192.168.1.105:554/stream1")) Console.WriteLine("连接成功");
                            //else Console.WriteLine("连接失败");
                            Console.WriteLine(_rtsp);
                            string outputDir = "D:\\StayEyesVideos\\";
                            int index = 0;
                            while (true) {
                                index++;
                                VideoWriter videoWriter = new VideoWriter();
                                videoWriter.Open(outputDir + index.ToString() + ".mp4", VideoWriter.FourCC('M', 'P', '4', 'V'), 30.0, new OpenCvSharp.Size(frame.Width, frame.Height), true);
                                for (int frames = 150; frames > 0; frames--) {
                                    if (videoCapture.Read(frame)) Console.WriteLine("yes");
                                    else Console.WriteLine("no");
                                    videoWriter.Write(frame);
                                    Cv2.ImShow("Live", frame);
                                    Console.WriteLine("width: " + frame.Width + " height: " + frame.Height);
                                    Cv2.WaitKey(33);
                                }
                                Console.WriteLine("视频流上传成功" + index);
                            }
                        }));
                        UploadVideoStreamThread.Start();
                    }
                    else {
                        UploadStatus = (int)UploadStatusEnum.cannotUpload;
                    }
                }).Start();
            }
            else if (UploadStatus == (int)UploadStatusEnum.uploadBySelf) {
                UploadVideoStreamThread.Abort();
                UploadStatus = (int)UploadStatusEnum.notUpload;
            }
        }

        /// <summary>
        /// 开关摄像头视频画面
        /// </summary>
        public DelegateCommand SwitchVLC => new DelegateCommand(ExecuteSwitchVLC);

        public void ExecuteSwitchVLC(object obj) {
            if (IsVLCOpen) {
                // 关闭摄像头
                VolumeButtonVisibility = "Hidden";
                IsVLCOpen = false;
                new Task(() => {
                    VLC.SourceProvider.MediaPlayer.Stop();
                }).Start();
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    VLC.Visibility = Visibility.Hidden;
                }));
                VLCButtonContent = "打开视频";
            }
            else {
                VLCButtonContent = "尝试连接摄像头中";
                VLCButtonEnable = "False";
                // 打开摄像头
                new Task(() => {
                    if (CheckVLCConnection()) {
                        IsVLCOpen = true;
                        VLCButtonEnable = "True";
                        VolumeButtonVisibility = "Visible";
                        Application.Current.Dispatcher.Invoke(new Action(() => {
                            VLC.Visibility = Visibility.Visible;
                        }));
                        VLCButtonContent = "关闭视频";
                        VLC.SourceProvider.MediaPlayer.Audio.Volume = 0;
                        VLC.SourceProvider.MediaPlayer.Play(new Uri(_rtsp));
                    }
                    else {
                        VLCButtonEnable = "True";
                        VLCButtonContent = "无法建立RTSP会话 | 重试";
                    }
                }).Start();
                
            }
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
            string status,  // 服务器显示的摄像头状态
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
            Application.Current.Dispatcher.Invoke(new Action(() => {
                VLC = new();
                VLC.Visibility = Visibility.Hidden;
                VLC.SourceProvider.CreatePlayer(_vlcLibDirectory, _options);
            }));
            _rtsp = String.Format("rtsp://{0}:{1}@{2}:{3}/stream1&channel=1",
                RTSPAcount, RTSPPassword, CameraIP, CameraPort);
            // 初始化时自动判断是否能连接上摄像头，检查摄像头上传状态
            new Task(() => {
                VLCButtonEnable = "False";
                if (CheckVLCConnection()) {
                    VLCButtonEnable = "True";
                    VLCButtonContent = "打开视频";
                }
                else {
                    VLCButtonEnable = "True";
                    VLCButtonContent = "无法建立RTSP会话 | 重试";
                    UploadStatus = (int)UploadStatusEnum.cannotUpload;
                }
            }).Start();
            
        }

        /// <summary>
        /// 检查 VLC 连接状态
        /// </summary>
        public bool CheckVLCConnection() {
            try {
            IPAddress ipAddress = IPAddress.Parse(CameraIP);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, int.Parse(CameraPort));
            TcpClient tcpClient = new TcpClient();
                const string CRLF = "\r\n";
                string options = string.Format("OPTIONS rtsp://{0}:{1} RTSP/1.0" + CRLF, CameraIP, CameraPort);
                options += "CSeq: 1" + CRLF + CRLF;
                byte[] buffer = Encoding.UTF8.GetBytes(options);
                tcpClient.Connect(ipEndPoint);
                tcpClient.GetStream().Write(buffer, 0, buffer.Length);
                StreamReader streamReader = new(tcpClient.GetStream());
                if (tcpClient.Connected) {
                    string response = streamReader.ReadLine();
                    if (response != null && response.Equals("RTSP/1.0 200 OK")) {
                        tcpClient.Close();
                        return true;
                    }
                }
                tcpClient.Close();
                return false;
            }
            catch (SocketException) {
                return false;
            }
        }

        /// <summary>
        /// 删除摄像头
        /// </summary>
        public void RequestDeletion(string id) {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                int indexOfPageChildren = CameraViewModel.Page.Children.Count;
                for (indexOfPageChildren--; indexOfPageChildren > 0; indexOfPageChildren--) {
                    CameraItem theCameraItem = CameraViewModel.Page.Children[indexOfPageChildren] as CameraItem;
                    if (theCameraItem.CameraItemViewModel.CameraID == id) {
                        CameraViewModel.Page.Children.Remove(theCameraItem);
                        int indexOfCameraList = CameraViewModel.CameraList.Count;
                        for (indexOfCameraList--; indexOfCameraList >= 0; indexOfCameraList--) {
                            if (CameraViewModel.CameraList[indexOfCameraList].CameraItemViewModel.CameraID == id) {
                                CameraViewModel.CameraList.RemoveAt(indexOfCameraList);
                                break;
                            }
                        }
                        break;
                    }
                }
            }));

        }
        #endregion
    }
}