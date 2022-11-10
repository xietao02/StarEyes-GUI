using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.IO;
using System.Reflection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// CameraItem.xaml 的交互逻辑
    /// </summary>
    public partial class CameraItem : UserControl {

        public PageViewModelBase GetItemWidth { get; set; } = new();
        
        public CameraItem() {
            InitializeComponent();
            DataContext = this;
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var vlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            var options = new string[]
            {
                 "--file-logging", "-vvv", "--logfile=VLCLogs.log"
            };
            this.VLC.SourceProvider.CreatePlayer(vlcLibDirectory, options);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.VLC.SourceProvider.MediaPlayer.Play(new Uri("rtsp://admin:Aa123456@192.168.1.115:554/stream1&channel=1"));
        }
    }


}
