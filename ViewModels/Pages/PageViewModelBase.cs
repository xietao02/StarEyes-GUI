using StarEyes_GUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEyes_GUI.ViewModels.Pages {
    public class PageViewModelBase : NotificationObject{

        #region PageItem 控件宽度自适应
        
        private double _ItemWidth;
        public double ItemWidth {
            get { return _ItemWidth; }
            set {
                _ItemWidth = value;
                RaisePropertyChanged("ItemWidth");
            }
        }

        private double _ItemMaxWidth;
        public double ItemMaxWidth {
            get { return _ItemMaxWidth; }
            set {
                _ItemMaxWidth = value;
                RaisePropertyChanged("ItemMaxWidth");
            }
        }

        public void CalPageItemWidth(object sender, System.Windows.SizeChangedEventArgs e) {
            double pageWidth = e.NewSize.Width;
            if (pageWidth > 1230) {
                ItemWidth = pageWidth / 2 - 20;
            }
            else {
                ItemWidth = pageWidth - 20;
            }
            ItemMaxWidth = pageWidth - 20;
        }
        #endregion
    }
}
