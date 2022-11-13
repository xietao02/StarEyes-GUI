using Org.BouncyCastle.Tls;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.Utils;
using StarEyes_GUI.ViewModels.Pages;
using System;
using System.Text;
using System.Text.RegularExpressions;
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
        }

        

        #region 输入验证逻辑
        private void CameraNameBox_LostFocus(object sender, RoutedEventArgs e) {
            if (CameraNameBox.Text.Length == 0) {
                CameraNameBox.ErrorStr = "名称不能为空";
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
                CameraIPBox.ErrorStr = "IP 不能为空";
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
                CameraPortBox.ErrorStr = "端口号不能为空";
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
                RTSPAcountBox.ErrorStr = "用户名不能为空";
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
                RTSPPasswordBox.ErrorStr = "密码不能为空";
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
                ChangeInfo();
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            CameraViewModel.isAddViewShow = false;
        }

        private void ChangeInfo() {
            if (CameraPosLonBox.Text.Length == 0) {
                // 获取经纬度
            }
            else {
                
            }
            if (CameraPosLatBox.Text.Length == 0) {
                
            }
            else {
                
            }
            // 生成 ID
            HandyControl.Controls.MessageBox.Success("新增摄像头成功！摄像头 ID：" +  "提示");
        }
    }
}
