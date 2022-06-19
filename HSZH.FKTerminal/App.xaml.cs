using HSZH.Common;
using HSZH.FKTerminal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HSZH.FKTerminal
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //CheckAdministrator();
            if (Win32API.AppIsRun("HSZH.FKTerminal"))
            {
                Win32API.SetTaskbarState(AppBarStates.AutoHide);
                this.DispatcherUnhandledException += App_DispatcherUnhandledException;
                this.StartupUri = new Uri("MainWindow.xaml", UriKind.RelativeOrAbsolute);
            }
            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Instance.WriteError("应用程序发生异常，异常信息：" + e.Exception.Message);
            MainWindowViewModel.Instance.SetFrameMessagePage("应用程序发生异常，请联系开发人员！", false, false, "");
            e.Handled = true;
        }
    }
}
