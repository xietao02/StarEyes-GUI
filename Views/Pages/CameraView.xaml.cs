using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using StarEyes_GUI.Views.Pages.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// CameraPage.xaml 的交互逻辑
    /// </summary>
    public partial class CameraView : UserControl {

        public CameraViewModel CameraViewModel { get; set; } = new();
        public List<CameraItem> CameraItemList = new();
        public CameraView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += CameraViewModel.CalPageItemWidth;
            Binding binding = new Binding("ItemWidth") { Source = CameraViewModel };

            
            
            CameraItemViewModel CameraItemViewModel1 = new(true, "10001", "monitor1", true, "10", "10", "12", "192.168.1.112", "554", "admin", "Aa123456");
            CameraItem cameraItem1 = new(CameraItemViewModel1, binding);
            page.Children.Add(cameraItem1);

            CameraItemViewModel CameraItemViewModel2 = new(false, "10002", "monitor2", true, "10", "10", "2", "192.168.1.115", "553", "admin", "Aa123456");
            CameraItem cameraItem2 = new(CameraItemViewModel2, binding);
            page.Children.Add(cameraItem2);

            CameraItemViewModel CameraItemViewModel3 = new(true, "10003", "monitor3", false, "10", "10", "0", "192.168.1.116", "554", "admin1", "Aa1234567");
            CameraItem cameraItem3 = new(CameraItemViewModel3, binding);
            page.Children.Add(cameraItem3);
        }


        
    }
}