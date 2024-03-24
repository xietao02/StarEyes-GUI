using StarEyes_GUI.Common.Utils;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StarEyes_GUI.Common.Data {

    public class EventItem : NotificationObject {

        private string _eventID = "null";
        public string eventID {
            get { return _eventID; }
            set {
                _eventID = value;
                RaisePropertyChanged("eventID");
            }
        }

        private string _cameraName = "null";
        public string cameraName {
            get { return _cameraName; }
            set {
                _cameraName = value;
                RaisePropertyChanged("cameraName");
            }
        }

        private string _type = "null";
        public string type {
            get { return _type; }
            set {
                _type = value;
                RaisePropertyChanged("type");
            }
        }

        private string _time = "null";
        public string time {
            get { return _time; }
            set {
                _time = value;
                RaisePropertyChanged("time");
            }
        }

        private string _status = "null";
        public string status {
            get { return _status; }
            set {
                _status = value;
                RaisePropertyChanged("status");
            }
        }

        private string _bt_enable = "False";
        public string bt_enable {
            get { return _bt_enable; }
            set {
                _bt_enable = value;
                RaisePropertyChanged("bt_enable");
            }
        }

        private string _bt_content = "无操作";
        public string bt_content {
            get { return _bt_content; }
            set {
                _bt_content = value;
                RaisePropertyChanged("bt_content");
            }
        }

        private Style _bt_style;
        public Style bt_style {
            get { return _bt_style; }
            set {
                _bt_style = value;
                RaisePropertyChanged("bt_style");
            }
        }

    }
}
