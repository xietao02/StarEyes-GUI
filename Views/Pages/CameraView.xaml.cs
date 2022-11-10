using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
        }


        
    }
}