using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.FKTerminal.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        public override void DoNextFunction(object obj)
        {
            string flag = obj?.ToString();
            switch (flag)
            {
                case "1":
                    MainWindowViewModel.Instance.SetFrameTraget("ReadIDCard");
                    break;
            }
        }
    }
}
