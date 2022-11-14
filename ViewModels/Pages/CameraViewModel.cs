using MySql.Data.MySqlClient;
using StarEyes_GUI.Models;
using StarEyes_GUI.UserControls;
using StarEyes_GUI.UserControls.UCViewModels;
using StarEyes_GUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarEyes_GUI.ViewModels.Pages {
    public class CameraViewModel : PageViewModelBase{
        public WrapPanel Page;
        public List<CameraItem> CameraList = new();
        private StarEyesServer Server = new();
        public Binding binding;
        public bool isAddViewShow = false;


        #region 依赖属性
        public string Info_AllCameraStatus { get; set; }
        public string Info_TotalCameraNum { get; set; }
        public string Style_AllCameraStatus_BG { get; set; }
        public string Style_AllCameraStatus_FG { get; set; }
        public string SwitchAllCameraStatusInfo { get; set; }
        private int _TotalCameraNum;
        public int TotalCameraNum {
            get { return _TotalCameraNum; }
            set {
                _TotalCameraNum = value;
                if(value == 0) {
                    SwitchAllCameraStatusInfo = "Hidden";
                }
                else SwitchAllCameraStatusInfo = "Visible";
                Info_TotalCameraNum = "组织 [" + StarEyesModel.Organization + "] 摄像头总数：" + value;
                RaisePropertyChanged("Info_TotalCameraNum");
            }
        }
        
        private int _BadConnCameraNum;
        public int BadConnCameraNum {
            get { return _BadConnCameraNum; }
            set {
                _BadConnCameraNum = value;
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
            }
        }
        public string Info_ComputerPosLat { get; set; } = "无法获取位置信息";
        private string _ComputerPosLat = "0";
        public string ComputerPosLat {
            get { return _ComputerPosLat; }
            set {
                _ComputerPosLat = value;
                Info_ComputerPosLat = "默认使用当前位置经度：" + value;
                RaisePropertyChanged("Info_ComputerPosLat");
            }
        }

        public string Info_ComputerPosLon { get; set; } = "无法获取位置信息";
        private string _ComputerPosLon = "0";
        public string ComputerPosLon {
            get { return _ComputerPosLon; }
            set {
                _ComputerPosLon = value;
                Info_ComputerPosLon = "默认使用当前位置纬度：" + value ;
                RaisePropertyChanged("Info_ComputerPosLon");
            }
        }
        #endregion

        /// <summary>
        /// 同步服务器摄像头列表
        /// </summary>
        public bool SycCameraView() {
            bool Result = false;
            Application.Current.Dispatcher.Invoke(new Action(() => {
                TotalCameraNum = 0;
                BadConnCameraNum = 0;
                string cmd = string.Format("SELECT * FROM cameras WHERE `organization`='{0}'", StarEyesModel.Organization);
                MySqlDataReader reader = Server.GetSQLReader(cmd);
                if (reader != null) {
                    CameraList.Clear();
                    while (reader.Read()) {
                        CameraItemViewModel CameraItemViewModel = new(reader[0].ToString(), reader[1].ToString(), reader[3].ToString(), reader[4].ToString(),
                                            reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString());
                        CameraItemViewModel.CameraViewModel = this;
                        CameraItem cameraItem = new(CameraItemViewModel, binding);
                        CameraList.Add(cameraItem);
                        if (CameraItemViewModel.CameraStatus) {
                            TotalCameraNum++;
                        }
                        else {
                            BadConnCameraNum++;
                        }
                    }
                    reader.Close();
                    TotalCameraNum = CameraList.Count;
                    Console.WriteLine("Page旧摄像头数:" + (Page.Children.Count - 1));
                    Page.Children.RemoveRange(1, Page.Children.Count - 1);
                    Console.WriteLine("清除成功");
                    CameraList.ForEach(theCameraItem => {
                        Page.Children.Add(theCameraItem);
                    });
                    Result = true;
                }
                else Result = false;
            }));
            return Result;
        }
        
    }
}
