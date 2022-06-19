using HSZH.Common;
using HSZH.FKTerminal.ViewModels;
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

namespace HSZH.FKTerminal
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //private bool AltDown = false;

        public MainWindow()
        {
            InitializeComponent();
            //this.Width = this.Width / 2;
            //this.Height = this.Height / 2;

            this.DataContext = MainWindowViewModel.Instance;
            this.Closed += MainWindow_Closed;
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            //    this.PreviewKeyUp += MainWindow_PreviewKeyUp;
            //}
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            MainWindowViewModel.Instance.CloseShowTime();
            Win32API.SetTaskbarState(AppBarStates.AlwaysOnTop);
            System.Diagnostics.Process.GetCurrentProcess().Dispose();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        //#region 屏蔽ALT+F4关闭程序
        //private void MainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
        //        AltDown = false;
        //}

        //private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
        //    {
        //        AltDown = true;
        //    }
        //    else if (e.SystemKey == Key.F4 && AltDown)
        //    {
        //        e.Handled = true;
        //    }
        //}
        //#endregion
    }
}
