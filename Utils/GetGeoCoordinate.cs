using StarEyes_GUI.Models;
using StarEyes_GUI.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarEyes_GUI.Utils {
    public class GetGeoCoordinate {
        public Thread thread;
        private bool GotCoord = false;
        private bool Reminded = false;

        public GetGeoCoordinate(CameraViewModel cameraViewModel) {
            thread = new Thread(new ThreadStart(() => {
                if(cameraViewModel.ComputerPosLon == "0" || cameraViewModel.ComputerPosLat == "0") {
                    while (!GotCoord) {
                        GeoCoordinateWatcher watcher = new();
                        watcher.PositionChanged += (sender, e) => {
                            var coordinate = e.Position.Location;
                            if (!coordinate.IsUnknown) {
                                watcher.Stop();
                                Console.WriteLine("Lat: {0}, Long: {1}",
                                    coordinate.Latitude,
                                    coordinate.Longitude);
                                cameraViewModel.ComputerPosLat = coordinate.Latitude.ToString();
                                cameraViewModel.ComputerPosLon = coordinate.Longitude.ToString();
                                GotCoord = true;
                            }
                        };
                        watcher.Start();
                        Thread.Sleep(3000);
                        if (!Reminded && (watcher.Permission == GeoPositionPermission.Unknown || watcher.Permission == GeoPositionPermission.Denied)) {
                            HandyControl.Controls.MessageBox.Warning("请检查设备定位功能是否开启，或是否授权 StarEyes 使用地理位置信息！", "无法获取位置信息");
                            Reminded = true;
                        }
                    }
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        ~GetGeoCoordinate() {
            thread.Abort();
        }
    }
}
