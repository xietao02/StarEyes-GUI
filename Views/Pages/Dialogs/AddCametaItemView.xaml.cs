using System;
using System.Device.Location;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using StarEyes_GUI.Common.Data;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.ViewModels.Pages;

namespace StarEyes_GUI.Views.Pages.Dialogs {
    /// <summary>
    /// _addCametaItemView.xaml 的交互逻辑
    /// </summary>
    public partial class AddCametaItemView : Window {
        public CameraViewModel CameraViewModel { get; set; }
        private StarEyesServer _server = new();
        private GeoCoord _getGeoCoord = new();
        private GeoCoordinate _geoCoordinate;

        private bool _nameFormat = true;
        private bool _ipFormat = true;
        private bool _portFormat = true;
        private bool _acountFormat = true;
        private bool _passwordFormat = true;
        private bool _posLonFormat = true;
        private bool _posLatFormat = true;

        /// <summary>
        /// 初始化新增摄像头窗口
        /// </summary>
        public AddCametaItemView(CameraViewModel cameraViewModel) {
            CameraViewModel = cameraViewModel;
            InitializeComponent();
            DataContext = this;
            CameraNameBox.Focus();
            Closed += _getGeoCoord.Stop;

            Thread getGeoCoordThread = new Thread(new ThreadStart(() => {
                if (cameraViewModel.ComputerPosLat == .0 && cameraViewModel.ComputerPosLon == .0) {
                    _geoCoordinate = _getGeoCoord.GetGeoCoord();
                    cameraViewModel.ComputerPosLat = _geoCoordinate.Latitude;
                    cameraViewModel.ComputerPosLon = _geoCoordinate.Longitude;
                }
            }));
            getGeoCoordThread.Start();
        }

        #region 输入验证逻辑
        private void CameraNameBox_LostFocus(object sender, RoutedEventArgs e) {
            if (CameraNameBox.Text.Length == 0) {
                _nameFormat = false;
                CameraNameBox.IsError = true;
            }
            else if (CameraNameBox.Text.Length > 16) {
                CameraNameBox.ErrorStr = "名称不能超过 16 个字符";
                _nameFormat = false;
                CameraNameBox.IsError = true;
            }
            else {
                _nameFormat = true;
                CameraNameBox.IsError = false;
            }
        }

        private void CameraIPBox_LostFocus(object sender, RoutedEventArgs e) {
            if (CameraIPBox.Text.Length == 0) {
                _ipFormat = false;
                CameraIPBox.IsError = true;
            }
            else if (CameraIPBox.Text.Length != 0) {
                CameraIPBox.ErrorStr = "IP 地址格式错误";
                string regexStrIPV4 = (@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$");
                if (Regex.IsMatch(CameraIPBox.Text, regexStrIPV4)) {
                    _ipFormat = true;
                    CameraIPBox.IsError = false;
                }
                else {
                    _ipFormat = false;
                    CameraIPBox.IsError = true;
                }
            }
            else {
                _ipFormat = true;
                CameraIPBox.IsError = false;
            }
        }

        private void CameraPortBox_LostFocus(object sender, RoutedEventArgs e) {
            if (CameraPortBox.Text.Length == 0) {
                _portFormat = false;
                CameraPortBox.IsError = true;
            }
            else if (CameraPortBox.Text.Length != 0) {
                CameraPortBox.ErrorStr = "端口号格式错误";
                _portFormat = true;
                int port;
                if (int.TryParse(CameraPortBox.Text, out port)) {
                    if (port < 0 || port > 65535) {
                        _portFormat = false;
                        CameraPortBox.IsError = true;
                    }
                    else {
                        _portFormat = true;
                        CameraPortBox.IsError = false;
                    }
                }
                else {
                    _portFormat = false;
                    CameraPortBox.IsError = true;
                }
            }
            else {
                _portFormat = true;
                CameraPortBox.IsError = false;
            }
        }

        private void RTSPAccountBox_LostFocus(object sender, RoutedEventArgs e) {
            if (RTSPAccountBox.Text.Length == 0) {
                _acountFormat = false;
                RTSPAccountBox.IsError = true;
            }
            else if (RTSPAccountBox.Text.Length > 16) {
                RTSPAccountBox.ErrorStr = "用户名不能超过 16 个字符";
                _acountFormat = false;
                RTSPAccountBox.IsError = true;
            }
            else {
                _acountFormat = true;
                RTSPAccountBox.IsError = false;
            }
        }

        private void RTSPPasswordBox_LostFocus(object sender, RoutedEventArgs e) {
            if (RTSPPasswordBox.Text.Length == 0) {
                _passwordFormat = false;
                RTSPPasswordBox.IsError = true;
            }
            else if (RTSPPasswordBox.Text.Length > 16) {
                RTSPPasswordBox.ErrorStr = "密码不能超过 16 个字符";
                _passwordFormat = false;
                RTSPPasswordBox.IsError = true;
            }
            else {
                _passwordFormat = true;
                RTSPPasswordBox.IsError = false;
            }
        }

        private void CameraPosLonBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraPosLonBox.ErrorStr = "纬度格式错误";
            if (CameraPosLonBox.Text.Length != 0) {
                _posLonFormat = true;
                double posLon;
                if (double.TryParse(CameraPosLonBox.Text, out posLon)) {
                    if (posLon < -90.0 || posLon > 90.0) {
                        _posLonFormat = false;
                        CameraPosLonBox.IsError = true;
                    }
                    else {
                        _posLonFormat = true;
                        CameraPosLonBox.IsError = false;
                    }
                }
                else {
                    _posLonFormat = false;
                    CameraPosLonBox.IsError = true;
                }
            }
            else {
                _posLonFormat = true;
                CameraPosLonBox.IsError = false;
            }
        }

        private void CameraPosLatBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraPosLatBox.ErrorStr = "经度格式错误";
            if (CameraPosLatBox.Text.Length != 0) {
                _posLatFormat = true;
                double posLat;
                if (double.TryParse(CameraPosLatBox.Text, out posLat)) {
                    if (posLat < -180.0 || posLat > 180.0) {
                        _posLatFormat = false;
                        CameraPosLatBox.IsError = true;
                    }
                    else {
                        _posLatFormat = true;
                        CameraPosLatBox.IsError = false;
                    }
                }
                else {
                    _posLatFormat = false;
                    CameraPosLatBox.IsError = true;
                }
            }
            else {
                _posLatFormat = true;
                CameraPosLatBox.IsError = false;
            }
        }
        #endregion

        /// <summary>
        /// 取消添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        #region 确认添加摄像头
        /// <summary>
        /// 输入验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm_Click(object sender, RoutedEventArgs e) {
            if (!_nameFormat) {
                CameraNameBox.Focus();
            }
            else if (!_ipFormat) {
                CameraIPBox.Focus();
            }
            else if (!_portFormat) {
                CameraPortBox.Focus();
            }
            else if (!_acountFormat) {
                RTSPAccountBox.Focus();
            }
            else if (!_passwordFormat) {
                RTSPPasswordBox.Focus();
            }
            else if (!_posLonFormat) {
                CameraPosLonBox.Focus();
            }
            else if (!_posLatFormat) {
                CameraPosLatBox.Focus();
            }
            else {
                SaveInfo();
                this.Close();
            }
        }

        /// <summary>
        /// 修改标志位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e) {
            CameraViewModel.IsAddViewShow = false;
        }

        /// <summary>
        /// 保存摄像头信息
        /// </summary>
        private void SaveInfo() {
            string cmd;
            bool Status = true;
            string cam_id = "-1",
                cam_name = CameraNameBox.Text,
                organization = StarEyesData.Organization,
                status = "0",
                pos_lon,
                pos_lat,
                ip = CameraIPBox.Text,
                port = CameraPortBox.Text,
                rtsp_acount = RTSPAccountBox.Text,
                rtsp_password = RTSPPasswordBox.Text,
                event_num = "0";
            if (CameraPosLonBox.Text.Length == 0) {
                pos_lon = CameraViewModel.ComputerPosLon.ToString();
            }
            else {
                pos_lon = CameraPosLonBox.Text;
            }
            if (CameraPosLatBox.Text.Length == 0) {
                pos_lat = CameraViewModel.ComputerPosLat.ToString();
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
                    MySqlDataReader reader = _server.GetSQLReader(cmd);
                    if (reader != null) {
                        if (!reader.Read()) {
                            isIDUnique = true;
                        }
                        reader.Close();
                    }
                    else Status = false;
                }
                if (Status) {
                    cmd = string.Format("INSERT INTO cameras (cam_id, cam_name, organization, status, pos_lon, pos_lat, ip, port, rtsp_acount, rtsp_password, event_num)" +
                    " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                    cam_id, cam_name, organization, status, pos_lon, pos_lat, ip, port, rtsp_acount, rtsp_password, event_num);
                    if (_server.ExecuteNonQuerySQL(cmd) == 1) {
                        CameraItemViewModel cameraItemViewModel = new(cam_id, cam_name, status, pos_lon, pos_lat, ip, port, rtsp_acount, rtsp_password, event_num);
                        Application.Current.Dispatcher.Invoke(new Action(() => {
                            CameraItem cameraItem = new(CameraViewModel, cameraItemViewModel, CameraViewModel.Binding);
                            CameraViewModel.CameraList.Add(cameraItem);
                            CameraViewModel.Page.Children.Add(cameraItem);
                        }));
                        HandyControl.Controls.MessageBox.Success("新增摄像头成功！摄像头 _id：" + cam_id, "提示");
                    }
                    else HandyControl.Controls.MessageBox.Error("新增摄像头失败！", "网络错误");
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        #endregion


        #region 快捷键跳转
        private void CameraNameBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                CameraIPBox.Focus();
            }
        }

        private void CameraIPBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                CameraPortBox.Focus();
            }
        }

        private void CameraPortBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                RTSPAccountBox.Focus();
            }
        }

        private void RTSPAccountBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                RTSPPasswordBox.Focus();
            }
        }

        private void RTSPPasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                CameraPosLonBox.Focus();
            }
        }

        private void CameraPosLonBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                CameraPosLatBox.Focus();
            }
        }
        #endregion
    }
}
