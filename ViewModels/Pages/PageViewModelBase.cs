﻿using StarEyes_GUI.Common.Utils;

namespace StarEyes_GUI.ViewModels.Pages {
    public class PageViewModelBase : NotificationObject{

        #region PageItem 控件宽度自适应
        
        private static double _itemWidth;
        public double ItemWidth {
            get { return _itemWidth; }
            set {
                _itemWidth = value;
                RaisePropertyChanged("ItemWidth");
            }
        }

        private static double _itemMaxWidth;
        public double ItemMaxWidth {
            get { return _itemMaxWidth; }
            set {
                _itemMaxWidth = value;
                RaisePropertyChanged("ItemMaxWidth");
            }
        }

        private static double _itemHalfWidth;
        public double itemHalfWidth {
            get { return _itemHalfWidth; }
            set {
                _itemHalfWidth = value;
                RaisePropertyChanged("itemHalfWidth");
            }
        }


        private static double _mapWidth;
        public double mapWidth {
            get { return _mapWidth; }
            set {
                _mapWidth = value;
                RaisePropertyChanged("mapWidth");
            }
        }


        public void CalPageItemWidth(object sender, System.Windows.SizeChangedEventArgs args) {
            double pageWidth = args.NewSize.Width;
            if (pageWidth > 1230) {
                ItemWidth = pageWidth / 2 - 20;
            }
            else {
                ItemWidth = pageWidth - 20;
            }
            ItemMaxWidth = pageWidth - 20;
            mapWidth = ItemMaxWidth - 205;
            itemHalfWidth = ItemMaxWidth / 2 - 10;
        }
        #endregion
    }
}
