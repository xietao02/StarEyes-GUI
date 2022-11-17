using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// Sidebar.xaml 的交互逻辑
    /// </summary>
    public partial class Sidebar : UserControl {
        private int _curIndex = 0, _lastIndex;
        public Sidebar() {
            InitializeComponent();
            SidebarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
            Switch += SwitchItemHandler;
        }

        #region Switch 事件
        // 定义 SwitchEventHandler 委托
        public delegate void SwitchEventHandler(object sender, SwitchEventArgs args);

        // 声明并注册路由事件
        public static readonly RoutedEvent SwitchEvent = EventManager.RegisterRoutedEvent(
            name: "Switch",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(SwitchEventHandler),
            ownerType: typeof(Sidebar));

        // 为路由事件添加 CLR 事件包装器, XAML 编辑器将使用此包装器来生成自动提示
        public event SwitchEventHandler Switch {
            add { AddHandler(SwitchEvent, value); }
            remove { RemoveHandler(SwitchEvent, value); }
        }

        // SwitchEvent 路由事件处理器 - SwitchItemHandler
        private void SwitchItemHandler(object sender, SwitchEventArgs args) {
            FrameworkElement element = sender as FrameworkElement;
            SwitchItem(args.ItemIndex);
        }
        
        // 定义 SwitchEvent 路由事件携带的参数
        public class SwitchEventArgs : RoutedEventArgs {
            public SwitchEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
            public int ItemIndex { get; set; }
        }
        #endregion

        /// <summary>
        /// 切换侧边栏项目高亮 
        /// </summary>
        /// <param name="newIndex"></param>
        public void SwitchItem(int newIndex) {
            if (_curIndex != newIndex) {
                _lastIndex = _curIndex;
                _curIndex = newIndex;
                switch (_curIndex) {
                    case 0:
                        SidebarItem1.IconSrc = "/Assets/icons/overview-active.png";
                        SidebarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 1:
                        SidebarItem2.IconSrc = "/Assets/icons/camera-active.png";
                        SidebarItem2.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 2:
                        SidebarItem3.IconSrc = "/Assets/icons/event-active.png";
                        SidebarItem3.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 3:
                        SidebarItem4.IconSrc = "/Assets/icons/cpu-active.png";
                        SidebarItem4.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 4:
                        SidebarItem5.IconSrc = "/Assets/icons/user-active.png";
                        SidebarItem5.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    case 5:
                        SidebarItem6.IconSrc = "/Assets/icons/more-active.png";
                        SidebarItem6.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xE4, 0x97));
                        break;
                    default:
                        break;
                }
                switch (_lastIndex) {
                    case 0:
                        SidebarItem1.IconSrc = "/Assets/icons/overview.png";
                        SidebarItem1.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 1:
                        SidebarItem2.IconSrc = "/Assets/icons/camera.png";
                        SidebarItem2.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 2:
                        SidebarItem3.IconSrc = "/Assets/icons/event.png";
                        SidebarItem3.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 3:
                        SidebarItem4.IconSrc = "/Assets/icons/cpu.png";
                        SidebarItem4.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 4:
                        SidebarItem5.IconSrc = "/Assets/icons/user.png";
                        SidebarItem5.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    case 5:
                        SidebarItem6.IconSrc = "/Assets/icons/more.png";
                        SidebarItem6.bt.Foreground = new SolidColorBrush(Color.FromRgb(0xE3, 0xE9, 0xF3));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
