using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.Threading;
using System;
using System.Windows.Controls;
using static StarEyes_GUI.ViewModels.Pages.AboutViewModel;
using System.IO;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// AboutPage.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView : UserControl {

        public AboutViewModel AboutViewModel { get; set; } = new();
        public AboutView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += AboutViewModel.CalPageItemWidth;
            string about = "Copyright ©2023 星眸 star-eyes.cloud\nAll Rights Reserved.";
            aboutText.Text = about;
        }

    }
}
