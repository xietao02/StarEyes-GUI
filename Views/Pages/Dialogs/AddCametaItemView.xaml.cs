using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using StarEyes_GUI.Models;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.Utils;
using StarEyes_GUI.ViewModels.Pages;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StarEyes_GUI.Views.Pages.Dialogs {
    /// <summary>
    /// AddCametaItemView.xaml 的交互逻辑
    /// </summary>
    public partial class AddCametaItemView : Window{
        public CameraViewModel CameraViewModel { get; set; }
        private StarEyesServer Server = new();
        GetGeoCoordinate TryGetGeoCoordinate;
        public int ID;

        bool nameFormat = true;
        bool ipFormat = true;
        bool portFormat = true;
        bool acountFormat = true;
        bool passwordFormat = true;
        bool posLonFormat = true;
        bool posLatFormat = true;

        /// <summary>
        /// 初始化新增摄像头窗口
        /// </summary>
        public AddCametaItemView(CameraViewModel cameraViewModel) {
            CameraViewModel = cameraViewModel;
            InitializeComponent();
            DataContext = this;
            CameraNameBox.Focus();
            TryGetGeoCoordinate = new(cameraViewModel);
        }

        #region 输入验证逻辑
        private void CameraNameBox_LostFocus(object sender, RoutedEventArgs e) {
            if (CameraNameBox.Text.Length == 0) {
                nameFormat = false;
                CameraNameBox.IsError = true;
            }
            else if (CameraNameBox.Text.Length > 20) {
                CameraNameBox.ErrorStr = "名称不能超过 20 个字符";
                nameFormat = false;
                CameraNameBox.IsError = true;
            }
            else {
                nameFormat = true;
                CameraNameBox.IsError = false;
            }
        }

        private void CameraIPBox_LostFocus(object sender, RoutedEventArgs e) {
            if (CameraIPBox.Text.Length == 0) {
                ipFormat = false;
                CameraIPBox.IsError = true;
            }
            else if (CameraIPBox.Text.Length != 0) {
                CameraIPBox.ErrorStr = "IP 地址格式错误";
                string regexStrIPV4 = (@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$");
                if (Regex.IsMatch(CameraIPBox.Text, regexStrIPV4)) {
                    ipFormat = true;
                    CameraIPBox.IsError = false;
                }
                else {
                    ipFormat = false;
                    CameraIPBox.IsError = true;
                }
            }
            else {
                ipFormat = true;
                CameraIPBox.IsError = false;
            }
        }

        private void CameraPortBox_LostFocus(object sender, RoutedEventArgs e) {
            if (CameraPortBox.Text.Length == 0) {
                portFormat = false;
                CameraPortBox.IsError = true;
            }
            else if (CameraPortBox.Text.Length != 0) {
                CameraPortBox.ErrorStr = "端口号格式错误";
                portFormat = true;
                int port;
                if (int.TryParse(CameraPortBox.Text, out port)) {
                    if (port < 0 || port > 65535) {
                        portFormat = false;
                        CameraPortBox.IsError = true;
                    }
                    else {
                        portFormat = true;
                        CameraPortBox.IsError = false;
                    }
                }
                else {
                    portFormat = false;
                    CameraPortBox.IsError = true;
                }
            }
            else {
                portFormat = true;
                CameraPortBox.IsError = false;
            }
        }

        private void RTSPAcountBox_LostFocus(object sender, RoutedEventArgs e) {
            if (RTSPAcountBox.Text.Length == 0) {
                acountFormat = false;
                RTSPAcountBox.IsError = true;
            }
            else if (RTSPAcountBox.Text.Length > 20) {
                RTSPAcountBox.ErrorStr = "用户名不能超过 20 个字符";
                acountFormat = false;
                RTSPAcountBox.IsError = true;
            }
            else {
                acountFormat = true;
                RTSPAcountBox.IsError = false;
            }
        }

        private void RTSPPasswordBox_LostFocus(object sender, RoutedEventArgs e) {
            if (RTSPPasswordBox.Text.Length == 0) {
                passwordFormat = false;
                RTSPPasswordBox.IsError = true;
            }
            else if (RTSPPasswordBox.Text.Length > 20) {
                RTSPPasswordBox.ErrorStr = "密码不能超过 20 个字符";
                passwordFormat = false;
                RTSPPasswordBox.IsError = true;
            }
            else {
                passwordFormat = true;
                RTSPPasswordBox.IsError = false;
            }
        }

        private void CameraPosLonBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraPosLonBox.ErrorStr = "纬度格式错误";
            if (CameraPosLonBox.Text.Length != 0) {
                posLonFormat = true;
                double posLon;
                if (double.TryParse(CameraPosLonBox.Text, out posLon)) {
                    if (posLon < -90.0 || posLon > 90.0) {
                        posLonFormat = false;
                        CameraPosLonBox.IsError = true;
                    }
                    else {
                        posLonFormat = true;
                        CameraPosLonBox.IsError = false;
                    }
                }
                else {
                    posLonFormat = false;
                    CameraPosLonBox.IsError = true;
                }
            }
            else {
                posLonFormat = true;
                CameraPosLonBox.IsError = false;
            }
        }

        private void CameraPosLatBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraPosLatBox.ErrorStr = "经度格式错误";
            if (CameraPosLatBox.Text.Length != 0) {
                posLatFormat = true;
                double posLat;
                if (double.TryParse(CameraPosLatBox.Text, out posLat)) {
                    if (posLat < -180.0 || posLat > 180.0) {
                        posLatFormat = false;
                        CameraPosLatBox.IsError = true;
                    }
                    else {
                        posLatFormat = true;
                        CameraPosLatBox.IsError = false;
                    }
                }
                else {
                    posLatFormat = false;
                    CameraPosLatBox.IsError = true;
                }
            }
            else {
                posLatFormat = true;
                CameraPosLatBox.IsError = false;
            }
        }
        #endregion

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e) {
            if (!nameFormat) {
                CameraNameBox.Focus();
            }
            else if (!ipFormat) {
                CameraIPBox.Focus();
            }
            else if (!portFormat) {
                CameraPortBox.Focus();
            }
            else if (!acountFormat) {
                RTSPAcountBox.Focus();
            }
            else if (!passwordFormat) {
                RTSPPasswordBox.Focus();
            }
            else if (!posLonFormat) {
                CameraPosLonBox.Focus();
            }
            else if (!posLatFormat) {
                CameraPosLatBox.Focus();
            }
            else {
                SaveInfo();
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            CameraViewModel.isAddViewShow = false;
        }

        private void SaveInfo() {
            string cmd;
            bool Status = true;
            string cam_id = "-1", 
                cam_name = CameraNameBox.Text, 
                organization = StarEyesModel.Organization, 
                status = "0",
                pos_lon, 
                pos_lat, 
                ip = CameraIPBox.Text, 
                port = CameraPortBox.Text,
                rtsp_acount = RTSPAcountBox.Text, 
                rtsp_password = RTSPPasswordBox.Text, 
                event_num = "0";
            if (CameraPosLonBox.Text.Length == 0) {
                pos_lon = CameraViewModel.ComputerPosLon;
            }
            else {
                pos_lon = CameraPosLonBox.Text;
            }
            if (CameraPosLatBox.Text.Length == 0) {
                pos_lat = CameraViewModel.ComputerPosLat;
            }
            else {
                pos_lat = CameraPosLatBox.Text;
            }
            Thread thread = new Thread(new ThreadStart(() => {
                bool isIDUnique = false;
                while (!isIDUnique && Status) {
                    Random rdID = new Random();
                    cam_id = rdID.Next(10000, 100000).ToString();
                    cmd = string.Format("SELECT * FROM cameras WHERE `cam_id`='{0}'", cam_id);
                    MySqlDataReader reader = Server.GetSQLReader(cmd);
                    if (reader != null) {
                        if (!reader.Read()) {
                            isIDUnique = true;
                        }
                        reader.Close();
                    }
                    else {
                        Status = false;
                        HandyControl.Controls.MessageBox.Error("新增摄像头失败！", "网络错误");
                    }
                }
                if (Status) {
                    cmd = string.Format("INSERT INTO cameras (cam_id, cam_name, organization, status, pos_lon, pos_lat, ip, port, rtsp_acount, rtsp_password, event_num)" +
                    " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                    cam_id, cam_name, organization, status, pos_lon, pos_lat, ip, port, rtsp_acount, rtsp_password, event_num);
                    if (Server.ExecuteNonQuerySQL(cmd) == -1) Status = false;
                    if (Status) HandyControl.Controls.MessageBox.Success("新增摄像头成功！摄像头 ID：" + cam_id, "提示");
                    CameraViewModel.SycCameraView();
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
