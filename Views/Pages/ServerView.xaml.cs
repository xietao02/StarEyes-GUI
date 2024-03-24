using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System;
using System.Windows.Controls;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// ServerPage.xaml 的交互逻辑
    /// </summary>
    public partial class ServerView : UserControl {

        public ServerViewModel ServerViewModel { get; set; } = new();
        public string configPath;
        public ServerView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += ServerViewModel.CalPageItemWidth;
            configPath = Environment.CurrentDirectory;
            configPath += "\\Assets\\documents\\Server_Config.html";
            System.Diagnostics.Debug.WriteLine(configPath);
            browser.Source = new Uri(configPath);
        }

    }
}
