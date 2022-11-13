using StarEyes_GUI.Models;
using StarEyes_GUI.Utils;
using System.Threading;
using System.Device.Location;
using System;

namespace StarEyes_GUI.ViewModels {
    public class DashboardViewModel : NotificationObject {
        public DashboardViewModel() {
            GetGeoCoordinate TryGetGeoCoordinate = new();
        }
    }
}