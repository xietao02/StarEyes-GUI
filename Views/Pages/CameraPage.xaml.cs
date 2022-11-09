using StarEyes_GUI.ViewModels;
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
    public partial class CameraPage : UserControl {
        public CameraPage(DashboardViewModel _DashboardViewModel) {
            this.DashboardViewModel = _DashboardViewModel;
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var vlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            var options = new string[]
            {
                 "--file-logging", "-vvv", "--logfile=VLCLogs.log"
            };

            this.MyControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);
        }

        private DashboardViewModel DashboardViewModel;

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.MyControl.SourceProvider.MediaPlayer.Play(new Uri("rtsp://admin:Aa123456@192.168.1.115:554/stream1&channel=1"));
            System.Diagnostics.Trace.WriteLine("done");
        }
    }
}