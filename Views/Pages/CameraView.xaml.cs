using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using StarEyes_GUI.Views.Pages.Dialogs;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Path = System.IO.Path;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// CameraPage.xaml 的交互逻辑
    /// </summary>
    public partial class CameraView : UserControl {

        public CameraViewModel CameraViewModel { get; set; } = new();
        public CameraView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += CameraViewModel.CalPageItemWidth;
            CameraItem cameraItem1 = new(true, "10001", "monitor1", "1", "10", "10", "12", "192.168.1.115", "553", "admin", "Aa123456");
            page.Children.Add(cameraItem1);
            cameraItem1.SetBinding(CameraItem.ItemWidthProperty, new Binding("ItemWidth") { Source = CameraViewModel });

            CameraItem cameraItem2 = new(false, "10002", "monitor2", "1", "10", "10", "2", "554", "admin", "Aa123456");
            page.Children.Add(cameraItem2);
            cameraItem2.SetBinding(CameraItem.ItemWidthProperty, new Binding("ItemWidth") { Source = CameraViewModel });


            CameraItem cameraItem3 = new(false, "10003", "monitor3", "0", "10", "10", "0", "555", "admin", "Aa123456");
            page.Children.Add(cameraItem3);
            cameraItem3.SetBinding(CameraItem.ItemWidthProperty, new Binding("ItemWidth") { Source = CameraViewModel });
        }


        
    }
}