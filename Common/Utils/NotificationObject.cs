using System.ComponentModel;

namespace StarEyes_GUI.Common.Utils {

    /// <summary>
    /// ViewModel的基类，监听Properties是否变化。
    /// </summary>
    public class NotificationObject : INotifyPropertyChanged{
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName){
            if (PropertyChanged != null){
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
