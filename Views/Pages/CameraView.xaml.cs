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
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public CameraView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += CameraViewModel.CalPageItemWidth;
            CameraViewModel.Page = page;
            AddButton.Command = CameraViewModel.AddCameraItem;
        }
    }
}