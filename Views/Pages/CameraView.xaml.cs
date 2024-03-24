using StarEyes_GUI.ViewModels.Pages;
using System.Windows.Controls;

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