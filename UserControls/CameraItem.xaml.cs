using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.IO;
using System.Reflection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using StarEyes_GUI.UserControls.UCViewModels;
using Vlc.DotNet.Wpf;
using StarEyes_GUI.Views.Pages.Dialogs;
using HandyControl;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// CameraItem.xaml 的交互逻辑
    /// </summary>
    public partial class CameraItem : UserControl {
        public CameraItemViewModel CameraItemViewModel { get; set; }
        public EditCameraItemView EditCameraItemView;

        public CameraItem(bool Local, string ID, string Name, string Status, string PosLon, string PosLat, string EventNum, string IP = "", string Port = "", string RTSPAcount = "", string RTSPPassword = "") {
            InitializeComponent();
            CameraItemViewModel = new(VLC);
            DataContext = this;
            
            CameraItemViewModel.Local = Local;
            CameraItemViewModel.CameraID = ID;
            CameraItemViewModel.CameraName = Name;
            CameraItemViewModel.CameraEventNum = EventNum;
            CameraItemViewModel.CameraPosLat = PosLat;
            CameraItemViewModel.CameraPosLon = PosLon;
            CameraItemViewModel.CameraIP = IP;
            CameraItemViewModel.CameraPort = Port;
            CameraItemViewModel.RTSPAcount = RTSPAcount;
            CameraItemViewModel.RTSPPassword = RTSPPassword;

            if (Status == "1") {
                CameraItemViewModel.CameraStatus = true;
                CameraItemViewModel.Info_CameraStatus = "连接状态：" + "正常";
                status.Style = (Style)Application.Current.Resources["LabelSuccess"];
            }
            else {
                CameraItemViewModel.CameraStatus = false;
                CameraItemViewModel.Info_CameraStatus = "连接状态：" + "异常";
                status.Style = (Style)Application.Current.Resources["LabelDanger"];
            }
            if (Local) {
                CameraItemViewModel.CameraIP = IP;
                CameraItemViewModel.Info_CameraIP = "IP：" + IP;
                if(CameraItemViewModel.CameraStatus)CameraItemViewModel.SwitchOpenViewButton = "True";
                else CameraItemViewModel.SwitchOpenViewButton = "False";
            }
            else {
                CameraItemViewModel.Info_CameraIP = "IP：非当前局域网内";
                CameraItemViewModel.SwitchOpenViewButton = "False";
            }
            CameraItemViewModel.SwitchCloseViewButton = "False";
            CameraItemViewModel.Info_CameraID = "ID：" + ID;
            CameraItemViewModel.Info_CameraPos = String.Format("经纬度：({0}, {1})", PosLon, PosLat);
            CameraItemViewModel.Info_CameraEventNum = String.Format("累计检测到 {0} 起事件。", EventNum);

            OpenView.Command = CameraItemViewModel.OpenVLC;
            CloseView.Command = CameraItemViewModel.CloseVLC;

        }
        
        // 依赖属性 SetBinding 包装
        public BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding) {
            return BindingOperations.SetBinding(this, dp, binding);
        }

        #region ItemWidth 依赖属性 
        // CLR 属性包装器
        public double ItemWidth {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        // ItemWidth 依赖属性
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(CameraItem), new PropertyMetadata(.0));

        #endregion

        private void Edit_Click(object sender, RoutedEventArgs e) {
            if (!CameraItemViewModel.isEditViewShow) {
                CameraItemViewModel.isEditViewShow = true;
                EditCameraItemView = new(CameraItemViewModel);
                EditCameraItemView.Show();
            }
            else {
                HandyControl.Controls.MessageBox.Info("修改信息窗口已打开！", "提示");
            }
        }
    }
}
