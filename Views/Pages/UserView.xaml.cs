using StarEyes_GUI.Common.Data;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// UserPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserView : UserControl {

        public UserViewModel UserViewModel { get; set; } = new();
        private List<UserInfo> userInfoList { get; set; }
        public UserView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += UserViewModel.CalPageItemWidth;
            userInfoList = new List<UserInfo>() {
                new UserInfo() {
                    ID = StarEyesData.ID,
                    Name = StarEyesData.UserName,
                    Organization = StarEyesData.Organization,
                    Wechat = StarEyesData.Wechat,
                    Email = StarEyesData.Email,
                    Phone = StarEyesData.Phone
                }
            };
            UserInfo.ItemsSource = userInfoList;
        }

        private void ChangeInfo(object sender, System.Windows.RoutedEventArgs e) {
            HandyControl.Controls.MessageBox.Info("请联系管理员修改！", "提示");
        }
    }

    class UserInfo {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Wechat { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
