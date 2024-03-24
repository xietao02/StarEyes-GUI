using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEyes_GUI.ViewModels.Pages {
    public class EventViewModel : PageViewModelBase{
		private string _EventCountInfo;
		public string EventCountInfo {
			get { return _EventCountInfo; }
			set {
				_EventCountInfo = value;
				RaisePropertyChanged("EventCountInfo");
			}
		}
	}
}
