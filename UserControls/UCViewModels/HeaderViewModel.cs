using System.Threading;
using StarEyes_GUI.Common.Data;
using StarEyes_GUI.Common.Utils;

namespace StarEyes_GUI.UserControls.UCViewModels {
    public class HeaderViewModel : NotificationObject {

        #region 依赖属性
        private string _userToolTip;
        public string UserToolTip {
            get { return _userToolTip; }
            set {
                _userToolTip = value;
                RaisePropertyChanged("UserToolTip");
            }
        }

        private string _notifToolTip;
        public string NotifToolTip {
            get { return _notifToolTip; }
            set {
                _notifToolTip = value;
                RaisePropertyChanged("NotifToolTip");
            }
        }

        private string _bellSrc;
        public string BellSrc {
            get { return _bellSrc; }
            set {
                _bellSrc = value;
                RaisePropertyChanged("BellSrc");
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public HeaderViewModel() {
            CheckEventNum(0);
            UserToolTip = "用户ID:" + StarEyesData.ID;
        }

        /// <summary>
        /// 检查未处理事件数量
        /// </summary>
        public void CheckEventNum(int eventNum) {
            if (eventNum == 0) {
                NotifToolTip = "没有事件待处理";
                BellSrc = "/Assets/icons/bell.png";
            }
            else {
                NotifToolTip = "有" + eventNum.ToString() + "件事件未处理！";
                BellSrc = "/Assets/icons/bell-active.png";
            }
        }
        #endregion
    }
}
