using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// SideBarItem.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class SideBarItem : UserControl {
        public SideBarItem() {
            InitializeComponent();
            DataContext = this;
        }

        public string ItemName { get; set; }

        public int ItemIndex { get; set; }

        /// <summary>
        /// 注册 ItemWidth 依赖属性
        /// </summary>
        public string ItemWidth {
            get { return (string)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(string), typeof(SideBarItem), new PropertyMetadata("200"));


        /// <summary>
        ///  注册 IconSrc 依赖属性
        /// </summary>
        public string IconSrc {
            get { return (string)GetValue(IconSrcProperty); }
            set { SetValue(IconSrcProperty, value); }
        }

        public static readonly DependencyProperty IconSrcProperty =
            DependencyProperty.Register("IconSrc", typeof(string), typeof(SideBarItem),
                new PropertyMetadata("/Assets/icons/bell.png"));


        #region 注册 SwitchEvent 路由事件

        // 定义 SwitchEventHandler 委托
        public delegate void SwitchEventHandler(object sender, SwitchEventArgs args);

        // 声明并注册路由事件
        public static readonly RoutedEvent SwitchEvent = EventManager.RegisterRoutedEvent(
            name: "Switch",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(SwitchEventHandler),
            ownerType: typeof(SideBarItem));

        // 为路由事件添加 CLR 事件包装器, XAML 编辑器将使用此包装器来生成自动提示
        public event SwitchEventHandler Switch {
            add { AddHandler(SwitchEvent, value); }
            remove { RemoveHandler(SwitchEvent, value); }
        }

        // 激发路由事件的方法。
        private void OnSwitch(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SideBarItem.SwitchEvent, this);
            args.ItemIndex = this.ItemIndex;
            this.RaiseEvent(args);
        }
        #endregion

    }

    /// <summary>
    /// 定义 SwitchEvent 路由事件携带的参数
    /// </summary>
    public class SwitchEventArgs : RoutedEventArgs {
        public SwitchEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        public int ItemIndex { get; set; }
    }

}
