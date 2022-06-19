using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Interface
{
    public interface IIDCardReader
    {
        ReturnInfo Initialize(IdCardInitParam model);

        ReturnInfo DoReadIDCardInit();

        ReturnInfo DoReadIDCardInfo(out IdCardInfo model);
    }
}
