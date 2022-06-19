using HSZH.Common;
using HSZH.Config;
using HSZH.Hardware.TTSY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HSZH.FKTerminal.ViewModels
{
    public class CardStatusViewModel : ObservableObject
    {
        private string _MessageInfo;
        public string MessageInfo
        {
            get { return _MessageInfo; }
            set { _MessageInfo = value; base.RaisePropertyChanged("MessageInfo"); }
        }

        private Thread thread = null;

        public override void DoInitFunction(object obj)
        {
            MainWindowViewModel.Instance.OpenTimeOut(300);
            try
            {
                thread = new Thread(Read);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError(ex);
                MessageInfo = "抱歉，设备发生异常，请联系工作人员！";
            }
        }

        public override void DoExitFunction(object obj)
        {
            MainWindowViewModel.Instance.StopTimeOut();
            CloseThread();
            MainWindowViewModel.Instance.ReturnHome();
        }

        private void Read()
        {
            while (true)
            {
                try
                {
                    string comPortName = AppConfig.Instance.SysConfig.TT_K7X0_ComPortName;
                    MessageInfo = $"正在打开{comPortName}，请稍后...";                    
                    break;
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteError(ex);                    
                    MainWindowViewModel.Instance.SetFrameMessagePage("抱歉，发卡设备发生异常，请联系工作人员！");
                }
            }
        }

        private void CloseThread()
        {
            if (thread != null)
                thread.Abort();
        }
    }
}
