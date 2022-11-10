using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// SideBar.xaml 的交互逻辑
    /// </summary>
    public partial class SideBar : UserControl {

        int curIndex, lastIndex;
        public SideBar() {
            InitializeComponent();
            curIndex = 0;
            SideBarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
            Switch += SwitchItemHandler;
        }

        // 定义 SwitchEventHandler 委托
        public delegate void SwitchEventHandler(object sender, SwitchEventArgs args);

        // 声明并注册路由事件
        public static readonly RoutedEvent SwitchEvent = EventManager.RegisterRoutedEvent(
            name: "Switch",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(SwitchEventHandler),
            ownerType: typeof(SideBar));

        // 为路由事件添加 CLR 事件包装器, XAML 编辑器将使用此包装器来生成自动提示
        public event SwitchEventHandler Switch {
            add { AddHandler(SwitchEvent, value); }
            remove { RemoveHandler(SwitchEvent, value); }
        }
        
        // SwitchEvent 路由事件处理器
        private void SwitchItemHandler(object sender, SwitchEventArgs args) {
            FrameworkElement element = sender as FrameworkElement;
            SwitchItem(args.ItemIndex);
        }
        
        // 定义 SwitchEvent 路由事件携带的参数
        public class SwitchEventArgs : RoutedEventArgs {
            public SwitchEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
            public int ItemIndex { get; set; }
        }
        
        #region 切换侧边栏项目高亮
        public void SwitchItem(int newIndex) {
            if (curIndex != newIndex) {
                lastIndex = curIndex;
                curIndex = newIndex;
                switch (curIndex) {
                    case 0:
                        SideBarItem1.IconSrc = "/Assets/icons/overview-active.png";
                        SideBarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 1:
                        SideBarItem2.IconSrc = "/Assets/icons/camera-active.png";
                        SideBarItem2.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 2:
                        SideBarItem3.IconSrc = "/Assets/icons/event-active.png";
                        SideBarItem3.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 3:
                        SideBarItem4.IconSrc = "/Assets/icons/cpu-active.png";
                        SideBarItem4.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 4:
                        SideBarItem5.IconSrc = "/Assets/icons/user-active.png";
                        SideBarItem5.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 5:
                        SideBarItem6.IconSrc = "/Assets/icons/more-active.png";
                        SideBarItem6.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    default:
                        break;
                }
                switch (lastIndex) {
                    case 0:
                        SideBarItem1.IconSrc = "/Assets/icons/overview.png";
                        SideBarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 1:
                        SideBarItem2.IconSrc = "/Assets/icons/camera.png";
                        SideBarItem2.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 2:
                        SideBarItem3.IconSrc = "/Assets/icons/event.png";
                        SideBarItem3.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 3:
                        SideBarItem4.IconSrc = "/Assets/icons/cpu.png";
                        SideBarItem4.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 4:
                        SideBarItem5.IconSrc = "/Assets/icons/user.png";
                        SideBarItem5.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 5:
                        SideBarItem6.IconSrc = "/Assets/icons/more.png";
                        SideBarItem6.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
