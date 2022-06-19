using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.FKTerminal
{
    public class Gloval : IDisposable
    {
        /// <summary>
        /// 静态属性
        /// </summary>
        //public static Gloval Instance
        //{
        //    get { return SingletonProvider<Gloval>.Instance; }
        //}

        public static IdCardInfo CardInfoModel = null;

        public void Dispose()
        {
           
        }
    }
}
