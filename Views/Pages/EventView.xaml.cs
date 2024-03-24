using MySql.Data.MySqlClient;
using StarEyes_GUI.Common.Data;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.ViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// EventPage.xaml 的交互逻辑
    /// </summary>
    public partial class EventView : UserControl {

        public EventViewModel EventViewModel { get; set; } = new();

        public ObservableCollection<EventItem> EventList { get; set; }
        private StarEyesServer _server = new();

        public EventView() {
            InitializeComponent();
            DataContext = this;
            SizeChanged += EventViewModel.CalPageItemWidth;
            EventList = new ObservableCollection<EventItem>() {
                new EventItem() {
                    bt_style = (Style)FindResource("ButtonSuccess")
                }
            };
            AllEventList.ItemsSource = EventList;
        }

        /// <summary>
        ///  将事件标为已处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventSolved(object sender, RoutedEventArgs e) {
            // 获取当前点击的按钮对象
            Button button = sender as Button;
            // 获取按钮所在的DataGridRow对象
            DataGridRow row = (DataGridRow)AllEventList.ItemContainerGenerator.ContainerFromItem(button.DataContext);
            // 获取DataGridRow对象的数据源
            var item = row.Item as EventItem;
            System.Diagnostics.Debug.WriteLine("尝试更新数据库");

            // 更新数据库
            string cmd = string.Format("UPDATE events SET have_read = '1' WHERE `id`='{0}'", item.eventID);
            new Task(() => {
                if (_server.ExecuteNonQuerySQL(cmd) == -1) {
                    HandyControl.Controls.MessageBox.Error("事件状态修改失败", "网络错误");
                }
                else {
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        System.Diagnostics.Debug.WriteLine("开始修改！！");
                        // 修改数据源的属性
                        item.status = "已处理";
                        item.bt_enable = "False";
                        item.bt_content = "无操作";
                        button.Style = (Style)FindResource("ButtonSuccess");
                    }));
                }
            }).Start();
        }

        /// <summary>
        /// 更新 EventView
        /// </summary>
        public int SycEventView() {
            MySqlDataReader reader;
            string cmd;
            int count = 0;

            // 更新待处理事件数
            Dispatcher.Invoke(new Action(() => {
                EventList.Clear();
            }));
            foreach (var cameraItem in StarEyesData.CameraList) {
                string camid = cameraItem.CameraItemViewModel.CameraID;
                cmd = string.Format("SELECT * FROM events WHERE `cam_id`='{0}'", camid);
                reader = _server.GetSQLReader(cmd);
                if (reader != null) {
                    while (reader.Read()) {
                        count++;
                        // 更新未读事件列表
                        EventItem eventItem = new() {
                            eventID = reader[0].ToString(),
                            cameraName = cameraItem.CameraItemViewModel.CameraName,
                            time = reader[2].ToString(),
                            type = reader[4].ToString(),
                        };
                        if (reader[3].ToString() == "1") {
                            eventItem.status = "已处理";
                            eventItem.bt_style = (Style)FindResource("ButtonSuccess");
                        }
                        else {
                            eventItem.status = "未处理";
                            eventItem.bt_enable = "True";
                            eventItem.bt_content = "标为已处理";
                            eventItem.bt_style = (Style)FindResource("ButtonPrimary");
                        }
                        Dispatcher.Invoke(new Action(() => {
                            EventList.Add(eventItem);
                        }));
                    }
                    reader.Close();
                    EventViewModel.EventCountInfo = "检测事件总数：" + count.ToString();
                    
                }
                else EventViewModel.EventCountInfo = "检测事件总数：0";
            }
            return count;
        }

        private void EventReview(object sender, RoutedEventArgs e) {
            HandyControl.Controls.MessageBox.Info("请登录StarEyes网页版查看！\nhttp://star-eyes.cloud", "提示");
        }
    }
}
