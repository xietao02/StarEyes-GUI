using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using GMap.NET.WindowsPresentation;
using MySqlX.XDevAPI.Common;
using System;

namespace StarEyes_GUI.Common.Utils {
    public class AMapControl : GMapControl {
    }

    public abstract class AMapProviderBase : GMapProvider {
        public AMapProviderBase() {
            MaxZoom = null;
            RefererUrl = "https://www.amap.com/";
        }

        public override PureProjection Projection {
            get { return MercatorProjection.Instance; }
        }

        GMapProvider[] overlays;

        public override GMapProvider[] Overlays {
            get {
                if (overlays == null) {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }

        public class AMapProvider : AMapProviderBase {
            public static readonly AMapProvider Instance;
            readonly Guid id = new Guid("You GUID here");
            public override Guid Id {
                get { return id; }
            }
            readonly string name = "AMap";
            public override string Name {
                get {
                    return name;
                }
            }
            static AMapProvider() {
                Instance = new AMapProvider();
            }

            public override PureImage GetTileImage(GPoint pos, int zoom) {
                string url = MakeTileImageUrl(pos, zoom, LanguageStr);
                PureImage result;
                try {
                    result = GetTileImageUsingHttp(url);
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    result = null;
                }
                return result;
            }

            string MakeTileImageUrl(GPoint pos, int zoom, string language) {
                string url = string.Format(UrlFormat, pos.X, pos.Y, zoom);
                Console.WriteLine("url:" + url);
                return url;
            }

            static readonly string UrlFormat = "http://webrd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={0}&y={1}&z={2}";
        }
    }
}