using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls.UCViewModels;

namespace StarEyes_GUI.Views.Pages.Dialogs {
    /// <summary>
    /// editCameraItemView.xaml 的交互逻辑
    /// </summary>
    public partial class EditCameraItemView : Window {
        public CameraItemViewModel CameraItemViewModel { get; set; }
        private StarEyesServer Server = new();
        public string theTitle { get; set; }
        private string id;
        
        bool nameFormat = true;
        bool ipFormat = true;
        bool portFormat = true;
        bool acountFormat = true;
        bool passwordFormat = true;
        bool eventNumFormat = true;
        bool posLonFormat = true;
        bool posLatFormat = true;
        
        /// <summary>
        /// 初始化编辑窗口
        /// </summary>
        /// <param name="cameraItemViewModel"></param>
        public EditCameraItemView(CameraItemViewModel cameraItemViewModel) {
            InitializeComponent();
            CameraItemViewModel = cameraItemViewModel;
            id = cameraItemViewModel.CameraID;
            theTitle = "StarEyes - 编辑摄像头信息 - id:" + id;
            DataContext = this;
            CameraNameBox.Focus();
        }

        #region 输入验证逻辑
        private void CameraNameBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraNameBox.ErrorStr = "名称不能超过 20 个字符";
            if (CameraNameBox.Text.Length > 20) {
                nameFormat = false;
                CameraNameBox.IsError = true;
            }
            else {
                nameFormat = true;
                CameraNameBox.IsError = false;
            }
        }

        private void CameraIPBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraIPBox.ErrorStr = "IP 地址格式错误";
            if (CameraIPBox.Text.Length != 0) {
                string regexStrIPV4 = (@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$");
                if (Regex.IsMatch(CameraIPBox.Text, regexStrIPV4)){
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
            CameraPortBox.ErrorStr = "端口号格式错误";
            if (CameraPortBox.Text.Length != 0) {
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
            RTSPAcountBox.ErrorStr = "用户名不能超过 20 个字符";
            if (RTSPAcountBox.Text.Length > 20) {
                acountFormat = false;
                RTSPAcountBox.IsError = true;
            }
            else {
                acountFormat = true;
                RTSPAcountBox.IsError = false;
            }
        }

        private void RTSPPasswordBox_LostFocus(object sender, RoutedEventArgs e) {
            RTSPPasswordBox.ErrorStr = "密码不能超过 20 个字符";
            if (RTSPPasswordBox.Text.Length > 20) {
                passwordFormat = false;
                RTSPPasswordBox.IsError = true;
            }
            else {
                passwordFormat = true;
                RTSPPasswordBox.IsError = false;
            }
        }

        private void CameraEventNumBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraEventNumBox.ErrorStr = "事件数格式错误";
            if (CameraEventNumBox.Text.Length != 0) {
                eventNumFormat = true;
                int eventNum;
                if (int.TryParse(CameraEventNumBox.Text, out eventNum)) {
                    if (eventNum < 0) {
                        eventNumFormat = false;
                        CameraEventNumBox.IsError = true;
                    }
                    else {
                        eventNumFormat = true;
                        CameraEventNumBox.IsError = false;
                    }
                }
                else {
                    eventNumFormat = false;
                    CameraEventNumBox.IsError = true;
                }
            }
            else {
                eventNumFormat = true;
                CameraEventNumBox.IsError = false;
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
            else if (!eventNumFormat) {
                CameraEventNumBox.Focus();
            }
            else if (!posLonFormat) {
                CameraPosLonBox.Focus();
            }
            else if (!posLatFormat) {
                CameraPosLatBox.Focus();
            }
            else {
                ChangeInfo();
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            CameraItemViewModel.IsEditViewShow = false;
        }

        private void ChangeInfo() {
            int NumOfChanged = 0;
            string changes = "";
            string[] cmds = new string[8];
            if (CameraNameBox.Text.Length != 0) {
                changes += "名称、";
                CameraItemViewModel.CameraName = CameraNameBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET cam_name = '" + CameraNameBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (CameraIPBox.Text.Length != 0) {
                changes += "IP、";
                CameraItemViewModel.CameraIP = CameraIPBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET ip = '" + CameraIPBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (CameraPortBox.Text.Length != 0) {
                changes += "端口、";
                CameraItemViewModel.CameraPort = CameraPortBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET port = '" + CameraPortBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (RTSPAcountBox.Text.Length != 0) {
                changes += "RTSP账号、";
                CameraItemViewModel.RTSPAcount = RTSPAcountBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET rtsp_acount = '" + RTSPAcountBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (RTSPPasswordBox.Text.Length != 0) {
                changes += "RTSP密码、";
                CameraItemViewModel.RTSPPassword = RTSPPasswordBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET rtsp_password = '" + RTSPPasswordBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (CameraEventNumBox.Text.Length != 0) {
                changes += "事件检测数、";
                CameraItemViewModel.CameraEventNum = CameraEventNumBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET event_num = '" + CameraEventNumBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (CameraPosLonBox.Text.Length != 0) {
                changes += "纬度、";
                CameraItemViewModel.CameraPosLon = CameraPosLonBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET pos_lon = '" + CameraPosLonBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (CameraPosLatBox.Text.Length != 0) {
                changes += "经度、";
                CameraItemViewModel.CameraPosLat = CameraPosLatBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET pos_lat = '" + CameraPosLatBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
            }
            if (NumOfChanged == 0) {
                HandyControl.Controls.MessageBox.Info("未进行任何修改！", "提示");
            }
            else {
                Thread thread = new Thread(new ThreadStart(() =>{
                    if (Server.ExecuteNonQuerySQL(cmds) == -1) {
                        HandyControl.Controls.MessageBox.Error("修改信息失败", "网络错误");
                    }
                    else {
                        string num = "共修改" + NumOfChanged.ToString() + "项摄像头信息：";
                        changes = changes.Substring(0, changes.Length - 1);
                        string info = num + changes + "。";
                        HandyControl.Controls.MessageBox.Success(info, "修改成功");
                    }
                }));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e) {
            string ask = string.Format("删除摄像头[{0}](_id:{1})？", CameraItemViewModel.CameraName, id);
            if(HandyControl.Controls.MessageBox.Show(ask, "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                string cmd = string.Format("DELETE FROM cameras WHERE(cam_id = '{0}' )", id);
                Thread thread = new Thread(new ThreadStart(() => {
                    if (Server.ExecuteNonQuerySQL(cmd) == -1) {
                        HandyControl.Controls.MessageBox.Error("删除摄像头失败", "网络错误");
                    }
                    else {
                        HandyControl.Controls.MessageBox.Success("删除摄像头成功", "提示");
                        // 发出更新事件
                    }
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        this.Close();
                    }));
                }));
                thread.IsBackground = true;
                thread.Start();
            }
        }
    }
}