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
        private Dictionary<int, string> IDC_State;
        private Dictionary<int, string> IDC_Acition;
        private Dictionary<int, string> IDC_CardBoxState;
        private Dictionary<int, string> IDC_CardPosition;

        public Gloval()
        {
            IDC_State = new Dictionary<int, string>();
            IDC_State.Add(0x39, "回收箱卡满/卡箱预满");
            IDC_State.Add(0x38, "回收箱卡满");
            IDC_State.Add(0x34, "命令不能执行，请点击“复位”");
            IDC_State.Add(0x32, "准备卡失败，请点击“复位”");
            IDC_State.Add(0x31, "卡箱预满");
            IDC_State.Add(0x30, "空闲");

            IDC_Acition = new Dictionary<int, string>();
            IDC_Acition.Add(0x38, "正在发卡");
            IDC_Acition.Add(0x34, "正在收卡");
            IDC_Acition.Add(0x32, "发卡出错，请点击“复位”");
            IDC_Acition.Add(0x31, "收卡出错，请点击“复位”");
            IDC_Acition.Add(0x30, "空闲");

            IDC_CardBoxState = new Dictionary<int, string>();
            IDC_CardBoxState.Add(0x39, "发卡箱已满，无法再回收到发卡箱");
            IDC_CardBoxState.Add(0x38, "发卡箱已满，无法再回收到发卡箱/卡箱预空");
            IDC_CardBoxState.Add(0x34, "重叠卡");
            IDC_CardBoxState.Add(0x32, "卡堵塞");
            IDC_CardBoxState.Add(0x31, "卡箱预空");
            IDC_CardBoxState.Add(0x30, "卡箱为非预空状态");

            IDC_CardPosition = new Dictionary<int, string>();
            IDC_CardBoxState.Add(0x3E, "只有一张卡在传感器2-3位置");
            IDC_CardBoxState.Add(0x3B, "只有一张卡在传感器1-2位置");
            IDC_CardBoxState.Add(0x39, "只有一张卡在传感器1位置");
            IDC_CardBoxState.Add(0x38, "卡箱已空");
            IDC_CardBoxState.Add(0x37, "卡在传感器1-2-3的位置");
            IDC_CardBoxState.Add(0x36, "卡在传感器2-3的位置");
            IDC_CardBoxState.Add(0x35, "卡在传感器取卡位置");
            IDC_CardBoxState.Add(0x34, "卡在传感器3位置");
            IDC_CardBoxState.Add(0x33, "卡在传感器1-2位置(读卡位置)");
            IDC_CardBoxState.Add(0x32, "卡在传感器2位置");
            IDC_CardBoxState.Add(0x31, "卡在传感器1位置(取卡位置)");
            IDC_CardBoxState.Add(0x30, "空闲");
        }

        public void Dispose()
        {
        }

    }
}
