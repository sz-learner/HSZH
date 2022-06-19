using System;
using System.Collections.Generic;
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

namespace HSZH.FKTerminal.Controls
{
    /// <summary>
    /// LoadingWait.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWait : UserControl
    {
        public LoadingWait()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LoadMsgInfoProperty =
            DependencyProperty.Register("LoadMsgInfo", typeof(string), typeof(LoadingWait));

        public static readonly DependencyProperty ProForegroundProperty =
            DependencyProperty.Register("ProForeground", typeof(Brush), typeof(LoadingWait), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(29, 123, 239)), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BlProActiveProperty =
           DependencyProperty.Register("BlProActive", typeof(bool), typeof(LoadingWait), new PropertyMetadata(true));

        public string LoadMsgInfo
        {

            get { return (string)GetValue(LoadMsgInfoProperty); }
            set { SetValue(LoadMsgInfoProperty, value); }
        }

        public Brush ProForeground
        {
            get { return (Brush)GetValue(ProForegroundProperty); }
            set { SetValue(ProForegroundProperty, value); }
        }
        /// <summary>
        /// 进度条是否活动
        /// </summary>
        public bool BlProActive
        {
            get { return (bool)GetValue(BlProActiveProperty); }
            set { SetValue(BlProActiveProperty, value); }
        }
    }
}
