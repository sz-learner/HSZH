using HSZH.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware.TTSY
{
    public class TT_K7X0Helper
    {
        /// <summary>
        /// 发卡（到取卡位置）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_DC(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "DC");
        }

        /// <summary>
        /// 收卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_CP(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "DC");
        }

        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_RS(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "RS");
        }

        /// <summary>
        /// 允许蜂鸣（卡少，卡空，出错蜂鸣器会报警）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_BE(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "BE");
        }

        /// <summary>
        /// 停止蜂鸣器工作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_BD(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "BD");
        }

        /// <summary>
        /// 回收卡片到发卡箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_DB(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "DB");
        }

        /// <summary>
        /// 发卡到取卡位置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_FC0(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "FC0");
        }

        /// <summary>
        /// 发卡到卡口外
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_FC4(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "FC4");
        }

        /// <summary>
        /// 发卡到传感器2
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_FC6(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "FC6");
        }

        /// <summary>
        /// 发卡到读卡位置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_FC7(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "FC7");
        }

        /// <summary>
        /// 前端进卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_FC8(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "FC8");
        }

        /// <summary>
        /// 设置闪灯频率,其中X=1-14表示1秒闪烁X次 X=15-28,表示(X-13)秒闪烁1次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SendCmd_LPX(TT_K7X0_SendCmdParamModel model)
        {
            return SendCmd(model, "LPX");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        public static bool SendCmd(TT_K7X0_SendCmdParamModel model, string cmdParam)
        {
            IntPtr pRecordInfo = Marshal.AllocHGlobal(1024);
            LogHelper.Instance.WriteDebug($"K720_SendCmd，MacAddr：{model.MacAddr}, CmdParam：{cmdParam}");
            int iRet = TT_K7X0.K720_SendCmd(model.ComHandle, model.MacAddr, cmdParam, cmdParam.Length, pRecordInfo);
            LogHelper.Instance.WriteDebug($"K720_SendCmd，Ret：{iRet}");

            if (iRet != 0)
            {
                Marshal.FreeHGlobal(pRecordInfo);
                return false;
            }

            Marshal.FreeHGlobal(pRecordInfo);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stateInfo"></param>
        /// <returns></returns>
        public static bool GetCountSum(TT_K7X0_SendCmdParamModel model, out byte[] stateInfo)
        {
            stateInfo = new byte[11];
            IntPtr pRecordInfo = Marshal.AllocHGlobal(1024);
            LogHelper.Instance.WriteDebug($"GetCountSum，MacAddr：{model.MacAddr}");
            int iRet = TT_K7X0.K720_GetCountSum(model.ComHandle, model.MacAddr, stateInfo, pRecordInfo);
            LogHelper.Instance.WriteDebug($"GetCountSum，Ret：{iRet}");

            if (iRet != 0)
            {
                Marshal.FreeHGlobal(pRecordInfo);
                return false;
            }

            Marshal.FreeHGlobal(pRecordInfo);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="comHandle"></param>
        /// <returns></returns>
        public static bool CommOpen(string comName, out IntPtr comHandle)
        {
            LogHelper.Instance.WriteDebug($"K720_CommOpen");
            comHandle = TT_K7X0.K720_CommOpen(comName);
            LogHelper.Instance.WriteDebug($"K720_CommOpen，Ret：{comHandle.ToInt32()}");

            if (comHandle == IntPtr.Zero)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comHandle"></param>
        /// <returns></returns>
        public static bool CommClose(IntPtr comHandle)
        {
            LogHelper.Instance.WriteDebug($"CommClose");
            int iRet = TT_K7X0.K720_CommClose(comHandle);
            LogHelper.Instance.WriteDebug($"CommClose，Ret：{iRet}");

            if (iRet != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comHandle"></param>
        /// <returns></returns>
        public static bool GetVersion(TT_K7X0_SendCmdParamModel model, out byte[] version)
        {
            version = new byte[20];
            IntPtr pRecordInfo = Marshal.AllocHGlobal(1024);
            LogHelper.Instance.WriteDebug($"CommClose");
            int iRet = TT_K7X0.K720_GetVersion(model.ComHandle, model.MacAddr, version, pRecordInfo);
            LogHelper.Instance.WriteDebug($"CommClose，Ret：{iRet}");

            if (iRet != 0)
            {
                Marshal.FreeHGlobal(pRecordInfo);
                return false;
            }

            Marshal.FreeHGlobal(pRecordInfo);
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TT_K7X0_SendCmdParamModel
    {
        public IntPtr ComHandle { get; set; }

        public byte MacAddr { get; set; }
    }
}
