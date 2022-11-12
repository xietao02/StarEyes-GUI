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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cameraItemViewModel"></param>
        /// <param name="binding"></param>
        public CameraItem(CameraItemViewModel cameraItemViewModel, Binding binding) {
            InitializeComponent();
            CameraItemViewModel = cameraItemViewModel;
            CameraItemViewModel.VLC = VLC;
            DataContext = this;
            SetBinding(CameraItem.ItemWidthProperty, binding);
            OpenView.Command = CameraItemViewModel.OpenVLC;
            CloseView.Command = CameraItemViewModel.CloseVLC;
            VolumeButton.Command = CameraItemViewModel.SwitchVolume;
            VolumeButton.CommandParameter = VolumeButton;
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

        /// <summary>
        /// 打开编辑窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
