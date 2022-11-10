using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// EventPage.xaml 的交互逻辑
    /// </summary>
    public partial class EventView : UserControl {

        public EventViewModel EventViewModel { get; set; } = new();
        public EventView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += EventViewModel.CalPageItemWidth;
        }
    }
}
