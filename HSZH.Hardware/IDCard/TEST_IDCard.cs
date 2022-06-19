using HSZH.Interface;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware.IDCard
{
    public class TEST_IDCard : IIDCardReader
    {
        public ReturnInfo DoReadIDCardInfo(out IdCardInfo model)
        {
            model = new IdCardInfo(1);
            return new ReturnInfo();
        }

        public ReturnInfo DoReadIDCardInit()
        {
            return new ReturnInfo();
        }

        public ReturnInfo Initialize(IdCardInitParam model)
        {
            return new ReturnInfo();
        }
    }
}
