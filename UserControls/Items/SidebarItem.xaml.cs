using System.Windows;
using System.Windows.Controls;
using static StarEyes_GUI.UserControls.Sidebar;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// SidebarItem.xaml 的交互逻辑
    /// </summary>
    public partial class SidebarItem : UserControl {
        
        #region SidebarItem 属性
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
            DependencyProperty.Register("ItemWidth", typeof(string), typeof(SidebarItem), new PropertyMetadata("200"));


        /// <summary>
        ///  注册 IconSrc 依赖属性
        /// </summary>
        public string IconSrc {
            get { return (string)GetValue(IconSrcProperty); }
            set { SetValue(IconSrcProperty, value); }
        }

        public static readonly DependencyProperty IconSrcProperty =
            DependencyProperty.Register("IconSrc",
                typeof(string),
                typeof(SidebarItem),
                new PropertyMetadata("/Assets/icons/bell.png"));
        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        public SidebarItem() {
            InitializeComponent();
            DataContext = this;
        }
        
        /// <summary>
        /// 激发路由事件的方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSwitch(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = ItemIndex;
            RaiseEvent(args);
        }

    }
}
