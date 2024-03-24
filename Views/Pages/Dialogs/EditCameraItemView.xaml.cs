using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
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
            CameraNameBox.ErrorStr = "名称不能超过 16 个字符";
            if (CameraNameBox.Text.Length > 16) {
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

        private void RTSPAccountBox_LostFocus(object sender, RoutedEventArgs e) {
            RTSPAccountBox.ErrorStr = "用户名不能超过 16 个字符";
            if (RTSPAccountBox.Text.Length > 16) {
                acountFormat = false;
                RTSPAccountBox.IsError = true;
            }
            else {
                acountFormat = true;
                RTSPAccountBox.IsError = false;
            }
        }

        private void RTSPPasswordBox_LostFocus(object sender, RoutedEventArgs e) {
            RTSPPasswordBox.ErrorStr = "密码不能超过 16 个字符";
            if (RTSPPasswordBox.Text.Length > 16) {
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
            CameraPosLonBox.ErrorStr = "经度格式错误";
            if (CameraPosLonBox.Text.Length != 0) {
                posLatFormat = true;
                double posLat;
                if (double.TryParse(CameraPosLonBox.Text, out posLat)) {
                    if (posLat < -180.0 || posLat > 180.0) {
                        posLatFormat = false;
                        CameraPosLonBox.IsError = true;
                    }
                    else {
                        posLatFormat = true;
                        CameraPosLonBox.IsError = false;
                    }
                }
                else {
                    posLatFormat = false;
                    CameraPosLonBox.IsError = true;
                }
            }
            else {
                posLatFormat = true;
                CameraPosLonBox.IsError = false;
            }
        }

        private void CameraPosLatBox_LostFocus(object sender, RoutedEventArgs e) {
            CameraPosLatBox.ErrorStr = "纬度格式错误";
            if (CameraPosLatBox.Text.Length != 0) {
                posLonFormat = true;
                double posLon;
                if (double.TryParse(CameraPosLatBox.Text, out posLon)) {
                    if (posLon < -90.0 || posLon > 90.0) {
                        posLonFormat = false;
                        CameraPosLatBox.IsError = true;
                    }
                    else {
                        posLonFormat = true;
                        CameraPosLatBox.IsError = false;
                    }
                }
                else {
                    posLonFormat = false;
                    CameraPosLatBox.IsError = true;
                }
            }
            else {
                posLonFormat = true;
                CameraPosLatBox.IsError = false;
            }
        }

        #endregion

        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        #region 确认修改摄像头信息
        /// <summary>
        /// 输入验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                RTSPAccountBox.Focus();
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

        /// <summary>
        /// 修改标志位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e) {
            CameraItemViewModel.IsEditViewShow = false;
        }

        /// <summary>
        /// 修改信息
        /// </summary>
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
            if (RTSPAccountBox.Text.Length != 0) {
                changes += "RTSP账号、";
                CameraItemViewModel.RTSPAcount = RTSPAccountBox.Text;
                cmds[NumOfChanged++] = "UPDATE cameras SET rtsp_acount = '" + RTSPAccountBox.Text + "' WHERE cam_id = " + CameraItemViewModel.CameraID;
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
        #endregion
        
        /// <summary>
        /// 删除摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e) {
            string ask = string.Format("删除摄像头[{0}](_id:{1})？", CameraItemViewModel.CameraName, id);
            if(HandyControl.Controls.MessageBox.Show(ask, "操作确认", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                string cmd = string.Format("DELETE FROM cameras WHERE(cam_id = '{0}' )", id);
                Thread thread = new Thread(new ThreadStart(() => {
                    if (Server.ExecuteNonQuerySQL(cmd) == -1) {
                        HandyControl.Controls.MessageBox.Error("删除摄像头失败", "网络错误");
                    }
                    else {
                        CameraItemViewModel.RequestDeletion(id);
                        HandyControl.Controls.MessageBox.Success("删除摄像头成功", "提示");
                    }
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        this.Close();
                    }));
                }));
                thread.IsBackground = true;
                thread.Start();
            }
        }

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
                CameraEventNumBox.Focus();
            }
        }

        private void CameraEventNumBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
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