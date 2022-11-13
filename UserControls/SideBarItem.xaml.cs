using System.Windows;
using System.Windows.Controls;
using static StarEyes_GUI.UserControls.SideBar;

namespace StarEyes_GUI.UserControls {
    /// <summary>
    /// SideBarItem.xaml 的交互逻辑
    /// </summary>
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

        

        /// <summary>
        /// 激发路由事件的方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSwitch(object sender, RoutedEventArgs e) {
            SwitchEventArgs args = new SwitchEventArgs(SwitchEvent, this);
            args.ItemIndex = this.ItemIndex;
            this.RaiseEvent(args);
        }

    }
    
    

}
