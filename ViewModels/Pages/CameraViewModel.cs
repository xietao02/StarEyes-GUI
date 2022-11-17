using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MySql.Data.MySqlClient;
using StarEyes_GUI.Common.Data;
using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.Views.Pages.Dialogs;

namespace StarEyes_GUI.ViewModels.Pages {
    public class CameraViewModel : PageViewModelBase{
        public WrapPanel Page;
        private List<CameraItem> _cameraList = new();
        private StarEyesServer _server = new();
        private Binding _binding;
        private AddCametaItemView _addCametaItemView;
        public bool IsAddViewShow = false;


        #region 依赖属性
        public string Info_TotalCameraNum { get; set; }
        public string Info_AllCameraStatus { get; set; } = "加载中...";
        public string Style_AllCameraStatus_BG { get; set; } = "Black";
        public string Style_AllCameraStatus_FG { get; set; } = "White";
        public string SwitchAllCameraStatusInfo { get; set; }
        private int _totalCameraNum;
        public int TotalCameraNum {
            get { return _totalCameraNum; }
            set {
                _totalCameraNum = value;
                if(value == 0) {
                    SwitchAllCameraStatusInfo = "Hidden";
                }
                else SwitchAllCameraStatusInfo = "Visible";
                Info_TotalCameraNum = "组织 [" + StarEyesData.Organization + "] 摄像头总数：" + value;
                RaisePropertyChanged("Info_TotalCameraNum");
            }
        }
        
        private int _badConnCameraNum;
        public int BadConnCameraNum {
            get { return _badConnCameraNum; }
            set {
                _badConnCameraNum = value;
                if (value == 0) {
                    Info_AllCameraStatus = "所有摄像头连接正常";
                    Style_AllCameraStatus_FG = "White";
                    Style_AllCameraStatus_BG = "#29b94c";
                }
                else {
                    Info_AllCameraStatus = value + " 个摄像头连接异常";
                    Style_AllCameraStatus_FG = "Black";
                    Style_AllCameraStatus_BG = "#eeb500";
                }
                RaisePropertyChanged("Info_AllCameraStatus");
                RaisePropertyChanged("Style_AllCameraStatus_FG");
                RaisePropertyChanged("Style_AllCameraStatus_BG");
            }
        }
        public string Info_ComputerPosLat { get; set; } = "位置信息未知";
        private double _computerPosLat = .0;
        public double ComputerPosLat {
            get { return _computerPosLat; }
            set {
                _computerPosLat = value;
                if (value == .0) {
                    Info_ComputerPosLat = "位置信息未知";
                }
                else {
                    Info_ComputerPosLat = "默认使用当前位置经度：" + value;
                }
                RaisePropertyChanged("Info_ComputerPosLat");
            }
        }

        public string Info_ComputerPosLon { get; set; } = "位置信息未知";
        private double _computerPosLon = .0;
        public double ComputerPosLon {
            get { return _computerPosLon; }
            set {
                _computerPosLon = value;
                if (value == .0) {
                    Info_ComputerPosLon = "位置信息未知";
                }
                else {
                    Info_ComputerPosLon = "默认使用当前位置纬度：" + value;
                }
                RaisePropertyChanged("Info_ComputerPosLon");
            }
        }
        #endregion

        public CameraViewModel() {
            _binding = new Binding("ItemWidth") { Source = this };
        }
        
        /// <summary>
        /// 同步服务器摄像头列表
        /// </summary>
        public bool SycCameraView() {
            bool Result = false;
            Application.Current.Dispatcher.Invoke(new Action(() => {
                TotalCameraNum = 0;
                BadConnCameraNum = 0;
                string cmd = string.Format("SELECT * FROM cameras WHERE `organization`='{0}'", StarEyesData.Organization);
                MySqlDataReader reader = _server.GetSQLReader(cmd);
                if (reader != null) {
                    _cameraList.Clear();
                    while (reader.Read()) {
                        CameraItemViewModel CameraItemViewModel = new(reader[0].ToString(), reader[1].ToString(), reader[3].ToString(), reader[4].ToString(),
                                            reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString());
                        CameraItem cameraItem = new(CameraItemViewModel, _binding);
                        _cameraList.Add(cameraItem);
                        if (CameraItemViewModel.CameraStatus) {
                            TotalCameraNum++;
                        }
                        else {
                            BadConnCameraNum++;
                        }
                    }
                    reader.Close();
                    TotalCameraNum = _cameraList.Count;
                    Console.WriteLine("Page旧摄像头数:" + (Page.Children.Count - 1));
                    Page.Children.RemoveRange(1, Page.Children.Count - 1);
                    Console.WriteLine("清除成功");
                    _cameraList.ForEach(theCameraItem => {
                        Page.Children.Add(theCameraItem);
                        Console.WriteLine("增加摄像头：" + theCameraItem.CameraItemViewModel.CameraName);
                    });
                    Result = true;
                }
                else Result = false;
            }));
            return Result;
        }

        /// <summary>
        ///  关闭未关闭的摄像流
        /// </summary>
        public void CloseVideoStream() {
            _cameraList.ForEach(theCameraItem => {
                if (theCameraItem.CameraItemViewModel.IsVLCOpen) {
                    theCameraItem.CameraItemViewModel.ExecuteCloseVLC(null);
                }
            });
        }

        /// <summary>
        /// 新增摄像头
        /// </summary>
        public DelegateCommand AddCameraItem => new DelegateCommand(ExecuteAddCameraItem, CanExecuteAddCameraItem);

        void ExecuteAddCameraItem(object obj) {
            IsAddViewShow = true;
            _addCametaItemView = new(this);
            _addCametaItemView.Show();
        }

        bool CanExecuteAddCameraItem(object obj) {
            if (!IsAddViewShow) return true;
            else {
                HandyControl.Controls.MessageBox.Info("新增摄像头窗口已打开！", "提示");
                return false;
            }
        }

    }
}
