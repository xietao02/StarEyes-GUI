using Emgu.CV;
using Emgu.CV.CvEnum;
using StarEyes_GUI.Common.Data;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.ViewModels.Pages;
using StarEyes_GUI.Views.Pages.Dialogs;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Vlc.DotNet.Wpf;

namespace StarEyes_GUI.UserControls.UCViewModels {
    public class CameraItemViewModel : NotificationObject {
        public CameraViewModel CameraViewModel;
        public Task UploadVideoStreamThread = null;

        #region 摄像头属性

        public bool IsVLCOpen = false;
        public bool IsLAN = false;
        public bool IsEditViewShow = false;
        public CancellationTokenSource StopUploadtokenSource;
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
                    if(UploadStatus == (int)UploadStatusEnum.uploadBySelf) {
                        Info_CameraStatus = "等待服务器接收视频流";
                    }
                    else if(UploadStatus == (int)UploadStatusEnum.uploadByOther || UploadStatus == (int)UploadStatusEnum.notUpload) {
                        // 用于初始化时更新 UploadVideoButtonContent
                        UploadStatus = (int)UploadStatusEnum.notUpload;
                        Info_CameraStatus = "状态：异常";
                    }
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
                    CameraStatus = false;
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
                if (IsLAN) {
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
                string lon, lat;
                if (CameraPosLon == "0" || CameraPosLat == "0") {
                    lon = "未知";
                    lat = "未知";
                    _info_CameraPos = "位置未知";
                    RaisePropertyChanged("Info_CameraPos");
                }
                else {
                    lon = CameraPosLon;
                    lat = CameraPosLat;
                    double d_lon, d_lat;
                    new Task(() => {
                        if (Double.TryParse(lon, out d_lon) && Double.TryParse(lat, out d_lat)) {
                            _info_CameraPos = GeoCoord.GetAddressByLnLa(d_lon, d_lat);
                        }
                        else _info_CameraPos = "位置未知";
                        RaisePropertyChanged("Info_CameraPos");
                    }).Start();
                }
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
                System.Diagnostics.Debug.WriteLine("尝试上传视频流中");
                System.Diagnostics.Debug.WriteLine(_rtsp);
                StopUploadtokenSource = new CancellationTokenSource();
                // 创建cache文件夹
                if (!Directory.Exists("./StayEyesCache")) {
                    Directory.CreateDirectory("./StayEyesCache");
                }
                if (!Directory.Exists("./StayEyesCache/vstream")) {
                    Directory.CreateDirectory("./StayEyesCache/vstream");
                }
                if (!Directory.Exists("./StayEyesCache/map")) {
                    Directory.CreateDirectory("./StayEyesCache/map");
                }
                UploadVideoStreamThread = new Task(() => {
                    if (CheckVLCConnection()) {
                        UploadStatus = (int)UploadStatusEnum.uploadBySelf;
                        Backend[] backends = CvInvoke.WriterBackends;
                        int backend_idx = 0; //any backend;
                        foreach (Backend be in backends) {
                            if (be.Name.Equals("MSMF")) {
                                backend_idx = be.ID;
                                break;
                            }
                        }
                        while (!StopUploadtokenSource.Token.IsCancellationRequested) {
                            VideoCapture capture = new VideoCapture(_rtsp);
                            System.Drawing.Size size = new System.Drawing.Size(capture.Width, capture.Height);
                            var now = DateTime.Now; // get current date and time
                            string timestamp = now.ToString("_yyyyMMdd_HHmm"); // format as string
                            string outputVideoName = CameraID + timestamp + ".mp4";
                            string outputDirName = "./StayEyesCache/vstream/" + outputVideoName;
                            while (File.Exists(outputDirName)) {
                                System.Diagnostics.Debug.WriteLine("文件名重复！！！");
                                Thread.Sleep(1000);
                                timestamp = now.ToString("_yyyyMMdd_HHmm");
                                outputVideoName = CameraID + timestamp + ".mp4";
                                outputDirName = "./StayEyesCache/vstream/" + outputVideoName;
                            }
                            System.Diagnostics.Debug.WriteLine("开始生成 " + outputVideoName);
                            VideoWriter writer = new VideoWriter(outputDirName, backend_idx, VideoWriter.Fourcc('H', '2', '6', '4'), 15, size, true);
                            int totalFrames = 900;
                            while (totalFrames-- >= 0) {
                                Mat frame = capture.QueryFrame();
                                if (frame == null) break;
                                writer.Write(frame);
                                frame.Dispose();
                            }
                            writer.Dispose();
                            capture.Dispose();
                            // 创建task进行上传
                            new Task(() => {
                                using (WebClient client = new WebClient()) {
                                    try {
                                        System.Diagnostics.Debug.WriteLine(outputVideoName + " 开始上传");
                                        // 指定要上传的URL
                                        string url = "http://star-eyes.cloud:9090/upload/?Content-Type=multipart/form-data";
                                        // 调用UploadFile方法并获取响应字节数组
                                        byte[] responseArray = client.UploadFile(url, outputDirName);
                                        client.Dispose();
                                        // 将响应字节数组转换为字符串并显示
                                        string responseText = Encoding.ASCII.GetString(responseArray);
                                        System.Diagnostics.Debug.WriteLine(responseText);
                                    }
                                    catch (Exception ex) {
                                        System.Diagnostics.Debug.WriteLine("[上传视频流异常] " + ex.Message);
                                    }
                                    if (File.Exists(outputDirName)) {
                                        try {
                                            //File.Delete(outputDirName);
                                        }
                                        catch (Exception ex) {
                                            System.Diagnostics.Debug.WriteLine("[尝试清空cache文件]" + ex.Message); //输出异常信息
                                        }
                                    }
                                }
                            }).Start();
                        }
                    }
                    else {
                        UploadStatus = (int)UploadStatusEnum.cannotUpload;
                    }
                }, StopUploadtokenSource.Token);
                UploadVideoStreamThread.Start();
            }
            else if (UploadStatus == (int)UploadStatusEnum.uploadBySelf) {
                StopUploadtokenSource.Cancel();
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
                    try {
                        string response = streamReader.ReadLine();
                        if (response != null && response.Equals("RTSP/1.0 200 OK")) {
                            tcpClient.Close();
                            IsLAN = true;
                            return true;
                        }
                    }
                    catch {
                        // no action required
                    }
                }
                tcpClient.Close();
                IsLAN = false;
                return false;
            }
            catch (SocketException) {
                IsLAN = false;
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
                        int indexOfCameraList = StarEyesData.CameraList.Count;
                        for (indexOfCameraList--; indexOfCameraList >= 0; indexOfCameraList--) {
                            if (StarEyesData.CameraList[indexOfCameraList].CameraItemViewModel.CameraID == id) {
                                StarEyesData.CameraList.RemoveAt(indexOfCameraList);
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