using GMap.NET;
using GMap.NET.WindowsPresentation;
using HandyControl.Controls;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using StarEyes_GUI.Common.Data;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.Items;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static StarEyes_GUI.Common.Data.EventItem;
using static StarEyes_GUI.Common.Utils.AMapProviderBase;

namespace StarEyes_GUI.Views.Pages {
    /// <summary>
    /// OverviewPage.xaml 的交互逻辑
    /// </summary>
    public partial class OverviewView : UserControl {

        public OverviewViewModel OverviewViewModel { get; set; } = new();
        private StarEyesServer _server = new();

        public string OverviewUserInfo { get; set; } = "用户：" + StarEyesData.UserName;
        public string OverviewOrgInfo { get; set; } = "组织：" + StarEyesData.Organization;
        public ObservableCollection<EventItem> EventList { get; set; }


        public OverviewView() {
            DataContext = this;
            InitializeComponent();
            SizeChanged += OverviewViewModel.CalPageItemWidth;
            new Task(() => {
                Map_Loaded();
            }).Start();
            EventList = new ObservableCollection<EventItem>() {
                new EventItem() {}
            };
            UnreadEventList.ItemsSource = EventList;
        }

        private void Map_Loaded() {
            try {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("ditu.google.cn");
            }
            catch {
                Dispatcher.Invoke(new Action(() => {
                    MainMap.Manager.Mode = AccessMode.CacheOnly;
                }));
                System.Diagnostics.Debug.WriteLine("网络错误，使用缓存加载地图");
            }
            Dispatcher.Invoke(new Action(() => {
                MainMap.CacheLocation = "./StayEyesCache/map/"; //缓存位置
                MainMap.MapProvider = AMapProvider.Instance; //加载高德地图
                MainMap.MinZoom = 4;  //最小缩放
                MainMap.MaxZoom = 18; //最大缩放
                MainMap.Zoom = 15;
                MainMap.Position = new PointLatLng(35.9433, 104.157083);
                MainMap.DragButton = MouseButton.Left;
                MainMap.ShowCenter = false; //不显示中心十字点
            }));
        }

        /// <summary>
        /// 更新Overview
        /// </summary>
        public void SycOverviewView() {
            //System.Diagnostics.Debug.WriteLine("开始更新Overview");
            Dispatcher.Invoke(new Action(() => {
                MainMap.Markers.Clear();
                mapList.Children.Clear();
                foreach (CameraItem cameraItem in StarEyesData.CameraList) {
                    string name = cameraItem.CameraItemViewModel.CameraName;
                    double lon, lat;
                    bool flag1, flag2;
                    flag1 = Double.TryParse(cameraItem.CameraItemViewModel.CameraPosLon.ToString(), out lon);
                    flag2 = Double.TryParse(cameraItem.CameraItemViewModel.CameraPosLat.ToString(), out lat);
                    if (flag1 && flag2) {
                        PointLatLng point = new PointLatLng(lat, lon);
                        GMapMarker marker = new GMapMarker(point);
                        if (cameraItem.CameraItemViewModel.CameraStatus) {
                            mapItem1 mapItem = new(MainMap, name, lon, lat);
                            mapList.Children.Add(mapItem);
                            marker.Shape = new Ellipse { Width = 10, Height = 10, Stroke = Brushes.DarkGreen, StrokeThickness = 4, Fill = Brushes.DarkGreen };
                            MainMap.Markers.Add(marker);
                        }
                        else {
                            mapItem0 mapItem = new(MainMap, name, lon, lat);
                            mapList.Children.Add(mapItem);
                            marker.Shape = new Ellipse { Width = 10, Height = 10, Stroke = Brushes.OrangeRed, StrokeThickness = 4, Fill = Brushes.OrangeRed };
                            MainMap.Markers.Add(marker);
                        }
                    }
                }
            }));

            // 更新摄像头总数
            OverviewViewModel.OverviewCamInfo = "摄像头总数：" + StarEyesData.CameraList.Count.ToString();

            MySqlDataReader reader;
            string cmd;

            // 更新待处理事件数
            Dispatcher.Invoke(new Action(() => {
                EventList.Clear();
            }));
            int count = 0;
            foreach (var cameraItem in StarEyesData.CameraList) {
                string camid = cameraItem.CameraItemViewModel.CameraID;
                cmd = string.Format("SELECT * FROM events WHERE `cam_id`='{0}' AND `have_read`='0'", camid);
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
                        Dispatcher.Invoke(new Action(() => {
                            EventList.Add(eventItem);
                        }));
                    }
                    reader.Close();
                    OverviewViewModel.UnReadEventCountInfo = "待处理事件数：" + count.ToString();
                }
                else OverviewViewModel.UnReadEventCountInfo = "待处理事件数：0";
            }

            // 更新检测事件总数
            count = 0;
            foreach (var cameraItem in StarEyesData.CameraList) {
                int count_singleCam = 0;
                string camid = cameraItem.CameraItemViewModel.CameraID;
                cmd = string.Format("SELECT * FROM events WHERE `cam_id`='{0}'", camid);
                reader = _server.GetSQLReader(cmd);
                if (reader != null) {
                    while (reader.Read()) {
                        count++;
                        count_singleCam++;
                    }
                }
                reader.Close();
                cmd = string.Format("UPDATE cameras SET event_num = '{0}' WHERE cam_id = '{1}'", count_singleCam.ToString(), camid);
                _server.ExecuteNonQuerySQL(cmd);
            }
            OverviewViewModel.OverviewEventInfo = "检测事件总数：" + count.ToString();
        }

        private void MainMap_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            // This will zoom the map control when the mouse wheel is over it
            MainMap.Zoom += e.Delta > 0 ? 1 : -1;
            // This will prevent the scroll viewer from scrolling when the mouse wheel is over the map control
            e.Handled = true;
        }

        private void UnreadEventList_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            // This will scroll the scroll viewer when the mouse wheel is over the data grid
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            // This will prevent the data grid from scrolling itself
            e.Handled = true;
        }
    }

}
