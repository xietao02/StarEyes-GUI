using StarEyes_GUI.Models;
using StarEyes_GUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarEyes_GUI.ViewModels {
    public class DashboardViewModel : NotificationObject {

		private string _Twinkling;
		public string Twinkling {
			get { return _Twinkling; }
			set {
				_Twinkling = value;
				RaisePropertyChanged("Twinkling");
			}
		}

        private DashboardModel _DashboardModel = new();
        public DashboardModel DashboardModel {
			get { return _DashboardModel; }
			set {
				_DashboardModel = value;
				RaisePropertyChanged("DashboardModel");
			}
		}

		public DashboardViewModel() {
			CheckEventNum();
			Twinkling = "Visible";
		}

        public void CheckEventNum() {
			if (_DashboardModel.eventNum == 0) {
                _DashboardModel.notifToolTip = "没有事件待处理";
                _DashboardModel.bellSrc = "/Assets/icons/bell.png";
            }
			else {
                _DashboardModel.notifToolTip = "有" + _DashboardModel.eventNum.ToString() + "件事件未处理！";
                _DashboardModel.bellSrc = "/Assets/icons/bell-active.png";

				Thread thread = new Thread(new ThreadStart(NotifTwinkling));
				thread.IsBackground = true;
				thread.Start();
			}
        }

        private void NotifTwinkling() {
            while(DashboardModel.eventNum != 0) {
                Thread.Sleep(500);
				Twinkling = "Hidden";
				Thread.Sleep(500);
				Twinkling = "Visible";
			}
        }

    }
}