using System;
using System.Device.Location;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

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

        
        public static string GetAddressJsonByLnLa(double lng, double lat, int timeout = 5000) {
            const string key = "b45cb733044b39dfe55a3d1e4ea48424";
            string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={lng}, {lat}";
            string json = "";
            try {
                if (WebRequest.Create(url) is HttpWebRequest req) {
                    req.ContentType = "multipart/form-data";
                    req.Accept = "*/*"; req.UserAgent = "";
                    req.Timeout = timeout;
                    req.Method = "GET";
                    req.KeepAlive = true;
                    if (req.GetResponse() is HttpWebResponse response)
                        using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) {
                            json = sr.ReadToEnd();
                            return json;
                        }
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                json = null;
            }
            return json;
        }

        public static string GetAddressByLnLa(double lon, double lat, int timeout = 5000) {
            string jsonString = GetAddressJsonByLnLa(lon, lat, timeout);
            if (jsonString != null) {
                // 解析为JObject对象
                JObject root = JObject.Parse(jsonString);
                // 通过键值访问formatted_address
                if (root != null) {
                    var regeocode = root["regeocode"];
                    if (regeocode != null && regeocode.Type != JTokenType.Null) {
                        var result = regeocode["formatted_address"];
                        if (result != null && result.Type != JTokenType.Null) {
                            return (string)result;
                        }
                    }
                }
            }
            return String.Format("经纬度:({0}, {1})", lon, lat);
        }
    }
}
