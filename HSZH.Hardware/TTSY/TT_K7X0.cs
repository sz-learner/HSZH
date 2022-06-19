using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware.TTSY
{
    public class TT_K7X0
    {
        private const string dllpath_K7x0 = @"EDCDMM\K7x0\K720_Dll.dll";

        ///////////////////预定义公共错误///////////////////////////
        //#define Bad_CommOpen			-101//串口未打开
        //#define Bad_CommSet				-102//串口设置错误

        //#define Bad_CommTimeouts		-999//串口读写超时设置错误
        //#define Bad_CommQueue			-998//串口缓冲区操作错误

        //        ///////////////////串口的读写的错误/////////////////////////
        //#define Bad_CommRead			-103//串口读超时
        //#define Bad_CommWrite			-104//串口写超时
        //#define ACK_Timeout				-105//串口ACK标识超时
        //#define EOT_Timeout				-106//串口Eot标识超时
        //#define PACKET_Timeout			-107//最后一包数据超时
        //#define WRONG_PacketHead		-108//错误的包头 
        //#define WRONG_PacketLen			-109//错误的包的长度
        //#define WRONG_BCC				-110//BBC校验错误
        //#define Bad_Parameter			-201//传入函数的参数错误
        //#define Bad_CommClose			-202//关闭串口错误
        //#define WRONG_ADDRESS			-203//错误地址

        //#define NOHIDDEVICE				-301
        //#define OPENHIDHANDLEFAILED		-302
        //#define HIDDEVICEHASCLOSED		-303
        //#define CLOSEHIDDEVICEFAILED	-304
        //#define SENDDATATIMEOUT         -305
        //#define RECEIVEDATATIMEOUT		-306
        //#define RECEIVEACKTIMEOUT		-309
        //#define HIDDEVICEHASOOPENED		-307
        //#define WRONGBAGHEAD			-308
        //#define SENDENQTIMEOUT			-310
        //#define ACKERR					-311   

        /////////////////RF610串口操作/////////////////////////////////////
        [DllImport(dllpath_K7x0, EntryPoint = "K720_CommOpen", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int K720_CommOpen(string port);

        [DllImport(dllpath_K7x0, EntryPoint = "K720_CommOpenWithBaud", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int K720_CommOpenWithBaud(string port, int _data);

        [DllImport(dllpath_K7x0, EntryPoint = "K720_CommClose", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int K720_CommClose(int ComHandle);

        [DllImport(dllpath_K7x0, EntryPoint = "K720_GetSysVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern int K720_GetSysVersion(int ComHandle, byte MacAddr, ref string strVersion);

        /**********************************D1801操作函数***************************************************/
        [DllImport(dllpath_K7x0, EntryPoint = "K720_GetVersion", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int K720_GetVersion(int ComHandle, byte MacAddr, byte[] Version, IntPtr RecordInfo);

        [DllImport(dllpath_K7x0, EntryPoint = "K720_SendCmd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int K720_SendCmd(int ComHandle, byte MacAddr, string p_Cmd, int CmdLen, IntPtr RecordInfo);

        [DllImport(dllpath_K7x0, EntryPoint = "K720_Query", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int K720_Query(int ComHandle, byte MacAddr, byte[] StateInfo, IntPtr RecordInfo);

        //[DllImport(dllpath_K7x0, EntryPoint = "K720_SensorQuery", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int K720_SensorQuery(IntPtr ComHandle, BYTE MacAddr, BYTE StateInfo[4], char* RecordInfo);

        //[DllImport(dllpath_K7x0, EntryPoint = "K720_GetCountSum", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int K720_GetCountSum(IntPtr ComHandle, BYTE MacAddr, BYTE StateInfo[11], char* RecordInfo);

        //[DllImport(dllpath_K7x0, EntryPoint = "K720_ClearSendCount", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int K720_ClearSendCount(IntPtr ComHandle, BYTE MacAddr, char* RecordInfo);

        //[DllImport(dllpath_K7x0, EntryPoint = "K720_ClearRecycleCount", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int K720_ClearRecycleCount(IntPtr ComHandle, BYTE MacAddr, char* RecordInfo);

        //[DllImport(dllpath_K7x0, EntryPoint = "K720_AutoTestMac", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int K720_AutoTestMac(IntPtr ComHandle, unsigned char MacAddr, char* RecordInfo);
    }
}
