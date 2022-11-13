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
        #endregion

        /// <summary>
        /// 初始化摄像头列表
        /// </summary>
        public void InitCameraList() {
            TotalCameraNum = 0;
            BadConnCameraNum = 0;
            string cmd = string.Format("SELECT * FROM cameras WHERE `organization`='{0}'", StarEyesModel.Organization);
            MySqlDataReader reader = Server.GetSQLReader(cmd);
            if (reader != null) {
                while (reader.Read()) {
                    CameraItemViewModel CameraItemViewModel = new(reader[0].ToString(), reader[1].ToString(), reader[3].ToString(), reader[4].ToString(),
                                        reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString());
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
            }
            TotalCameraNum = CameraList.Count;
        }
        
    }
}
