using StarEyes_GUI.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEyes_GUI.ViewModels.Pages {
    public class OverviewViewModel : PageViewModelBase{

        private string _OverviewCamInfo = "摄像头总数：读取中...";
        public string OverviewCamInfo {
            get { return _OverviewCamInfo; }
            set {
                _OverviewCamInfo = value;
                RaisePropertyChanged("OverviewCamInfo");
            }
        }

        private string _OverviewEventInfo = "检测事件总数：读取中...";
        public string OverviewEventInfo {
            get { return _OverviewEventInfo; }
            set {
                _OverviewEventInfo = value;
                RaisePropertyChanged("OverviewEventInfo");
            }
        }


        private string _UnReadEventCountInfo = "更新事件消息中...";
        public string UnReadEventCountInfo {
            get { return _UnReadEventCountInfo; }
            set {
                _UnReadEventCountInfo = value;
                RaisePropertyChanged("UnReadEventCountInfo");
            }
        }



    }
}
