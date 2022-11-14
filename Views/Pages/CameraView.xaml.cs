using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using StarEyes_GUI.Views.Pages.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// CameraPage.xaml 的交互逻辑
    /// </summary>
    public partial class CameraView : UserControl {
        public CameraViewModel CameraViewModel { get; set; } = new();
        public AddCametaItemView AddCametaItemView;

        public CameraView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += CameraViewModel.CalPageItemWidth;
            CameraViewModel.binding = new Binding("ItemWidth") { Source = CameraViewModel };
            CameraViewModel.Page = page;

            Thread thread = new Thread(new ThreadStart(() => {
                CameraViewModel.SycCameraView();
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private void AddCamera_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (!CameraViewModel.isAddViewShow) {
                CameraViewModel.isAddViewShow = true;
                AddCametaItemView = new(CameraViewModel);
                AddCametaItemView.Show();
            }
            else {
                HandyControl.Controls.MessageBox.Info("新增摄像头窗口已打开！", "提示");
            }
        }
    }
}