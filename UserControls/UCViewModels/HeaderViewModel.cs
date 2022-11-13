using StarEyes_GUI.Models;
using StarEyes_GUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarEyes_GUI.UserControls.UCViewModels {
    public class HeaderViewModel : NotificationObject {

        #region 依赖属性
        private string _UserToolTip;
        public string UserToolTip {
            get { return _UserToolTip; }
            set {
                _UserToolTip = value;
                RaisePropertyChanged("UserToolTip");
            }
        }

        private string _NotifToolTip;
        public string NotifToolTip {
            get { return _NotifToolTip; }
            set {
                _NotifToolTip = value;
                RaisePropertyChanged("NotifToolTip");
            }
        }

        private string _BellSrc;
        public string BellSrc {
            get { return _BellSrc; }
            set {
                _BellSrc = value;
                RaisePropertyChanged("BellSrc");
            }
        }

        private string _Twinkling;
        public string Twinkling {
            get { return _Twinkling; }
            set {
                _Twinkling = value;
                RaisePropertyChanged("Twinkling");
            }
        }

        #endregion

        public HeaderViewModel() {
            CheckEventNum();
            Twinkling = "Visible";
            UserToolTip = "用户ID:" + StarEyesModel.ID;
        }

        public void CheckEventNum() {
            if (StarEyesModel.EventNum == 0) {
                NotifToolTip = "没有事件待处理";
                BellSrc = "/Assets/icons/bell.png";
            }
            else {
                NotifToolTip = "有" + StarEyesModel.EventNum.ToString() + "件事件未处理！";
                BellSrc = "/Assets/icons/bell-active.png";

                Thread thread = new Thread(new ThreadStart(NotifTwinkling));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void NotifTwinkling() {
            while (StarEyesModel.EventNum != 0) {
                Thread.Sleep(500);
                Twinkling = "Hidden";
                Thread.Sleep(500);
                Twinkling = "Visible";
            }
        }

    }
}
