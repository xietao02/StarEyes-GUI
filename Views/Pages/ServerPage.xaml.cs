﻿using StarEyes_GUI.ViewModels;
using System.Windows.Controls;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// ServerPage.xaml 的交互逻辑
    /// </summary>
    public partial class ServerPage : UserControl {
        public ServerPage(DashboardViewModel _DashboardViewModel) {
            this.DashboardViewModel = _DashboardViewModel;
            InitializeComponent();
        }

        private DashboardViewModel DashboardViewModel;
    }
}
