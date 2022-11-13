using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarEyes_GUI.Utils {
    public class GetGeoCoordinate {
        //private bool GotCoord = false;
        //GeoCoordinateWatcher watcher;
        //int index = 0;

        public GetGeoCoordinate() {
            Thread thread = new Thread(new ThreadStart(() => {
                GeoCoordinateWatcher watcher;
                bool GotCoord = false;
                int index = 0;
                while (!GotCoord) {
                    index++;
                    Console.WriteLine("{0}号 watcher 激活", index);
                    watcher = new();
                    watcher.PositionChanged += (sender, e) => {
                        var coordinate = e.Position.Location;
                        if (!coordinate.IsUnknown) {
                            Console.WriteLine("Lat: {0}, Long: {1}, {2}号 watcher 获取的",
                                coordinate.Latitude,
                                coordinate.Longitude,
                                index);
                            watcher.Stop();
                            GotCoord = true;
                        }
                        else {
                            Console.WriteLine("coordinate 未包含位置信息");
                        }
                    };
                    watcher.Start();
                    Console.WriteLine("{0}号 watcher 开始睡眠", index);
                    Thread.Sleep(3000);
                    Console.WriteLine("{0}号 watcher 销毁", index);
                }
            }));
            thread.IsBackground = true;
            thread.Start();
            //thread.Abort();

            //while (!GotCoord) {
            //    index++;
            //    Console.WriteLine("新增线程{0}", index);
            //    Thread thread = new Thread(new ThreadStart(() => {
            //        int thread_index = index;
            //        watcher = new();
            //        watcher.PositionChanged += (sender, e) => {
            //            var coordinate = e.Position.Location;
            //            if (coordinate.IsUnknown != true) {
            //                Console.WriteLine("Lat: {0}, Long: {1}, 线程{2}: 获得经纬度！",
            //                    coordinate.Latitude,
            //                    coordinate.Longitude,
            //                    thread_index);
            //                watcher.Stop();
            //                GotCoord = true;
            //            }
            //            else {
            //                Console.WriteLine("定位失败，无法获取经纬度信息。");
            //            }
            //        };
            //        watcher.Start();
            //        while (true) {
            //            Console.WriteLine("线程{0}: 发送心跳", thread_index);
            //            Thread.Sleep(3000);
            //        }
            //    }));
            //    thread.IsBackground = true;
            //    thread.Start();
            //    Thread.Sleep(10000);
            //    //thread.Abort();
            //}

            //Thread thread = new Thread(new ThreadStart(() => {
            //    watcher = new();
            //    watcher.PositionChanged += (sender, e) => {
            //        var coordinate = e.Position.Location;
            //        if (coordinate.IsUnknown != true) {
            //            Console.WriteLine("Lat: {0}, Long: {1}",
            //                coordinate.Latitude,
            //                coordinate.Longitude);
            //            watcher.Stop();
            //        }
            //        else {
            //            Console.WriteLine("定位失败，无法获取经纬度信息。");
            //        }
            //    };
            //    watcher.Start();
            //}));
            //thread.IsBackground = true;
            //thread.Start();
        }
    }
}
