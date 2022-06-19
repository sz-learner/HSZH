using HSZH.Common;
using HSZH.Config;
using HSZH.Hardware.IDCard;
using HSZH.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.BLL
{
    public class HardwareHelper
    {
        private static readonly Lazy<HardwareHelper> LazyInstance = new Lazy<HardwareHelper>(() => new HardwareHelper());
        public static HardwareHelper Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        public IIDCardReader IdCardReader { get; private set; }

        private HardwareHelper()
        {
            if (AppConfig.Instance.SysConfig.IsConnReadIDcardDev)
            {
                IdCardReader = new CVR_IDCard();
            }
            else
            {
                IdCardReader = new TEST_IDCard();
            }
        }
    }
}
