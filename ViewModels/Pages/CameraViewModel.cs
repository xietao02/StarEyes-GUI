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
        public List<CameraItem> CameraList = new();
        private List<CameraItem> _newCameraList = new();
        public Binding Binding;
        public bool IsAddViewShow = false;
        private StarEyesServer _server = new();
        private AddCametaItemView _addCametaItemView;


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
            Binding = new Binding("ItemWidth") { Source = this };
        }
        
        /// <summary>
        /// 同步服务器摄像头列表
        /// </summary>
        public bool SycCameraView() {
            bool Result = false;
            Application.Current.Dispatcher.Invoke(new Action(() => {
                BadConnCameraNum = 0;
                string cmd = string.Format("SELECT * FROM cameras WHERE `organization`='{0}'", StarEyesData.Organization);
                MySqlDataReader reader = _server.GetSQLReader(cmd);
                if (reader != null) {
                    // 获取服务器摄像头列表
                    _newCameraList = new();
                    while (reader.Read()) {
                        CameraItemViewModel cameraItemViewModel = new(reader[0].ToString(), reader[1].ToString(), reader[3].ToString(), reader[4].ToString(),
                                            reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString());
                        CameraItem cameraItem = new(this, cameraItemViewModel, Binding);
                        _newCameraList.Add(cameraItem);
                    }
                    reader.Close();
                    
                    // 比较服务器摄像头列表与本地摄像头列表
                    if (CameraList.Count == 0) {
                        // 本地摄像头列表为空，直接添加
                        CameraList = _newCameraList;
                        foreach (var cameraItem in CameraList) {
                            Page.Children.Add(cameraItem);
                        }
                    }
                    else {
                        // 本地摄像头列表不为空，比较
                        int indexOfCameraList = CameraList.Count;
                        for (indexOfCameraList--; indexOfCameraList >= 0; indexOfCameraList--) {
                            var cameraItem = CameraList[indexOfCameraList];
                            bool found = false;
                            int indexOfNewCameraList = _newCameraList.Count;
                            CameraItem newCameraItem = null;
                            for (indexOfNewCameraList--; indexOfNewCameraList >= 0; indexOfNewCameraList--) {
                                newCameraItem = _newCameraList[indexOfNewCameraList];
                                if (cameraItem.CameraItemViewModel.CameraID == newCameraItem.CameraItemViewModel.CameraID) {
                                    _newCameraList.Remove(newCameraItem);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                // 本地摄像头列表中存在，服务器摄像头列表中不存在，删除
                                if (cameraItem.CameraItemViewModel.IsVLCOpen) {
                                    cameraItem.CameraItemViewModel.ExecuteSwitchVLC(null);
                                }
                                CameraList.Remove(cameraItem);
                                Page.Children.RemoveAt(indexOfCameraList + 1);
                            }
                            else {
                                // 更新摄像头其他信息
                                cameraItem.CameraItemViewModel.CameraName = newCameraItem.CameraItemViewModel.CameraName;
                                cameraItem.CameraItemViewModel.CameraStatus = newCameraItem.CameraItemViewModel.CameraStatus;
                                cameraItem.CameraItemViewModel.CameraPosLon = newCameraItem.CameraItemViewModel.CameraPosLon;
                                cameraItem.CameraItemViewModel.CameraPosLat = newCameraItem.CameraItemViewModel.CameraPosLat;
                                cameraItem.CameraItemViewModel.CameraIP = newCameraItem.CameraItemViewModel.CameraIP;
                                cameraItem.CameraItemViewModel.CameraPort = newCameraItem.CameraItemViewModel.CameraPort;
                                cameraItem.CameraItemViewModel.RTSPAcount = newCameraItem.CameraItemViewModel.RTSPAcount;
                                cameraItem.CameraItemViewModel.RTSPPassword = newCameraItem.CameraItemViewModel.RTSPPassword;
                                cameraItem.CameraItemViewModel.CameraEventNum = newCameraItem.CameraItemViewModel.CameraEventNum;
                            }
                        }
                        foreach (var newCameraItem in _newCameraList) {
                            // 将服务器中新增的摄像头添加至本地
                            CameraList.Add(newCameraItem);
                            Page.Children.Add(newCameraItem);
                        }
                    }

                    // 重新计算摄像头总数、连接异常摄像头数
                    TotalCameraNum = CameraList.Count;
                    CameraList.ForEach(theCameraItem => {
                        if (!theCameraItem.CameraItemViewModel.CameraStatus) {
                            BadConnCameraNum++;
                        }
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
            CameraList.ForEach(theCameraItem => {
                if (theCameraItem.CameraItemViewModel.IsVLCOpen) {
                    theCameraItem.CameraItemViewModel.ExecuteSwitchVLC(null);
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
