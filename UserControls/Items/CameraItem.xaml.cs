using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.ViewModels.Pages;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// CameraItem.xaml 的交互逻辑
    /// </summary>
    public partial class CameraItem : UserControl {
        public CameraItemViewModel CameraItemViewModel { get; set; }

        #region ItemWidth 依赖属性 
        // CLR 属性包装器
        public double ItemWidth {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        // ItemWidth 依赖属性
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(CameraItem), new PropertyMetadata(.0));

        // 依赖属性 SetBinding 包装
        public new BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding) {
            return BindingOperations.SetBinding(this, dp, binding);
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cameraItemViewModel"></param>
        /// <param name="binding"></param>
        public CameraItem(CameraViewModel cameraViewModel, CameraItemViewModel cameraItemViewModel, Binding binding) {
            InitializeComponent();
            CameraItemViewModel = cameraItemViewModel;
            CameraItemViewModel.CameraViewModel = cameraViewModel;
            DataContext = this;
            CameraItemContent.Children.Add(CameraItemViewModel.VLC);
            
            SetBinding(ItemWidthProperty, binding);
            VLCButton.Command = CameraItemViewModel.SwitchVLC;
            VolumeButton.Command = CameraItemViewModel.SwitchVolume;
            VolumeButton.CommandParameter = VolumeButton;
            EditButton.Command = CameraItemViewModel.EditCamera;
        }
        
                
    }
}
