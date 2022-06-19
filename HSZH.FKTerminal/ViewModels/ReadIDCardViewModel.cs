using HSZH.BLL;
using HSZH.Common;
using HSZH.Config;
using HSZH.Hardware;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HSZH.FKTerminal.ViewModels
{
    public class ReadIDCardViewModel : ObservableObject
    {
        private Thread ReadIDCardThread = null;

        public override void DoInitFunction(object obj)
        {
            TTS.PlaySound("刷身份证");
            MainWindowViewModel.Instance.OpenTimeOut();
            MainWindowViewModel.Instance.TimeOutEventAction += CloseThead;
            ReadIDCardThread = new Thread(new ThreadStart(ReadIDCard));
            ReadIDCardThread.IsBackground = true;
            ReadIDCardThread.Start();
        }

        public override void DoExitFunction(object obj)
        {
            MainWindowViewModel.Instance.StopTimeOut();
            CloseThead();
            MainWindowViewModel.Instance.ReturnHome();
        }

        private void CloseThead()
        {
            if (ReadIDCardThread == null)
                return;
            ReadIDCardThread.Abort(0);
            ReadIDCardThread = null;
        }

        private void ReadIDCard()
        {
            while (true)
            {
                ReturnInfo rinfo = HardwareHelper.Instance.IdCardReader.DoReadIDCardInfo(out IdCardInfo model);
                if (rinfo.IsSucceed)
                {
                    if (!AppConfig.Instance.SysConfig.IsConnReadIDcardDev)
                        Thread.Sleep(3000);
                    MainWindowViewModel.Instance.StopTimeOut();
                    Gloval.CardInfoModel = model;
                    MainWindowViewModel.Instance.SetFrameTraget("FaceCheck");
                    return;
                }
                Thread.Sleep(100);
            }
        }
    }
}
