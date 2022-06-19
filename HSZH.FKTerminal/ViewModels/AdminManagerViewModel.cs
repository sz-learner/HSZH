using HSZH.Common;
using HSZH.FKTerminal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HSZH.FKTerminal.ViewModels
{
    public class AdminManagerViewModel : ObservableObject
    {
        public override void DoNextFunction(object obj)
        {
            var nflag = obj.ToString();
            switch (nflag)
            {
                case "0":
                    MainWindowViewModel.Instance.ExitAppFun();
                    break;
                case "3":
                    Restart();
                    break;
                case "5":
                    PauseService();
                    break;
                case "7":
                    MainWindowViewModel.Instance.ReturnHome();
                    break;
                case "YJCS":
                    break;
            }
        }

        public override void DoExitFunction(object obj)
        {
            MainWindowViewModel.Instance.ReturnHome();
        }

        private void PauseService()
        {
            PauseServiceWindow pauseServiceWin = new PauseServiceWindow();
            pauseServiceWin.SelectPauseServiceCallBack = (pauseServiceReason) =>
            {
                if (pauseServiceReason.IsNotEmpty())
                {
                    MainWindowViewModel.Instance.SetFrameMessagePage(pauseServiceReason, false, false, "");
                    return;
                }
            };
            pauseServiceWin.ShowDialog();
        }

        /// <summary>
        /// 关闭软件
        /// </summary>
        private void Restart()
        {
            Application.Exit();
            Application.ExitThread();
            Application.Restart();
        }
    }
}
