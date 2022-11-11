using HandyControl.Data;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace StarEyes_GUI.Views.Pages.Dialogs {
    /// <summary>
    /// EditCameraItemView.xaml 的交互逻辑
    /// </summary>
    public partial class EditCameraItemView : Window {
        CameraItemViewModel CameraItemViewModel;
        public string theTitle { get; set; }
        public string CameraName { get; set; }
        public string CameraEventNum { get; set; }
        public string CameraIP { get; set; }
        public string CameraPort { get; set; }
        public string RTSPAcount { get; set; }
        public string RTSPPassword { get; set; }
        public string CameraPosLat { get; set; }
        public string CameraPosLon { get; set; }
        
        public EditCameraItemView(CameraItemViewModel cameraItemViewModel) {
            CameraItemViewModel = cameraItemViewModel;
            theTitle = "StarEyes - 编辑摄像头信息 - id:" + cameraItemViewModel.CameraID;
            CameraName = CameraItemViewModel.CameraName;
            CameraEventNum = CameraItemViewModel.CameraEventNum;
            CameraIP = CameraItemViewModel.CameraIP;
            CameraPort = CameraItemViewModel.CameraPort;
            RTSPAcount = CameraItemViewModel.RTSPAcount;
            RTSPPassword = CameraItemViewModel.RTSPPassword;
            CameraPosLat = CameraItemViewModel.CameraPosLat;
            CameraPosLon = CameraItemViewModel.CameraPosLon;
            DataContext = this;
            InitializeComponent();
            System.Diagnostics.Trace.WriteLine(CameraItemViewModel.CameraName);
        }
        public bool testt() {
            return true;
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e) {
            CameraItemViewModel.isEditViewShow = false;
        }
    }
}
