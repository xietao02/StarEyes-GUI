using System;
using System.Device.Location;
using System.Threading;

namespace StarEyes_GUI.Common.Utils {
    /// <summary>
    /// 获取经纬度信息
    /// </summary>
    public class GeoCoord {

        private GeoCoordinate _coordinate = null;
        private bool _stop = false;
        
        /// <summary>
        /// (需创建新线程执行)
        /// 尝试获取经纬度信息，获取失败返回经纬度(0,0)。
        /// </summary>
        /// <returns></returns>
        public GeoCoordinate GetGeoCoord() {
            bool gotCoord = false, showTip = false;
            while (!gotCoord && !_stop) {
                GeoCoordinateWatcher watcher = new();
                watcher.PositionChanged += (sender, e) => {
                    _coordinate = e.Position.Location;
                    if (!_coordinate.IsUnknown) {
                        watcher.Stop();
                        gotCoord = true;
                    }
                };
                watcher.Start();
                Thread.Sleep(5000);
                watcher.Dispose();
                if(!showTip && !gotCoord) {
                    HandyControl.Controls.MessageBox.Warning("请检查设备定位功能是否开启，或是否授权 StarEyes 使用地理位置信息！", "无法获取位置信息");
                    showTip = true;
                }
            }
            if (!gotCoord) _coordinate = new(.0, .0);
            return _coordinate;
        }

        public void Stop(object sender, EventArgs e) {
            _stop = true;
        }
    }
}
