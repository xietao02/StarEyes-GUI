using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEyes_GUI.ViewModels {

    /// <summary>
    /// ViewModel的基类，监听Properties是否变化。
    /// </summary>
    public class NotificationObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
