using HSZH.Common;
using HSZH.Hardware.FaceCheck;
using HSZH.Hardware.TTSY;
using HSZH.Tools.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HSZH.Tools.ViewModels.HardwareTest
{
    public class K7X0ViewModel : ObservableObject
    {
        private TT_K7X0_SendCmdParamModel CmdParamModel;
        private IntPtr ComHandle = IntPtr.Zero;
        public DataItemModel SelectDataItem { get; set; }
        public DataItemModel SelectdDeviceMacItem { get; set; }

        private ObservableCollection<DataItemModel> _DeviceMacList;
        public ObservableCollection<DataItemModel> DeviceMacList
        {
            get { return this._DeviceMacList; }
            set { this._DeviceMacList = value; this.RaisePropertyChanged("DeviceMacList"); }
        }

        private ObservableCollection<DataItemModel> _ItemList;
        public ObservableCollection<DataItemModel> ItemList
        {
            get { return this._ItemList; }
            set { this._ItemList = value; this.RaisePropertyChanged("ItemList"); }
        }

        private string _StatuMsg;
        public string StatuMsg
        {
            get { return this._StatuMsg; }
            set { this._StatuMsg = value; this.RaisePropertyChanged("StatuMsg"); }
        }

        private ICommand _DoK720_SendCmd;
        public ICommand DoK720_SendCmd
        {
            get
            {
                if (this._DoK720_SendCmd == null)
                {
                    this._DoK720_SendCmd = new RelayCommand<object>(
                      (obj) => this.DoK720_SendCmdFunction(obj));
                }
                return _DoK720_SendCmd;
            }
        }

        public K7X0ViewModel()
        {
            ItemList = new ObservableCollection<DataItemModel>();
            string[] comPortsNamesArr = SerialPort.GetPortNames();            
            foreach(string port in comPortsNamesArr)
            {
                ItemList.Add(new DataItemModel() { Code = port, Text = port });
            }
            if (ItemList.Count > 0)
                SelectDataItem = ItemList[0];

            DeviceMacList = new ObservableCollection<DataItemModel>();
            for (int i = 0; i < 16; i++)
            {
                string mac = Convert.ToString(i, 16).ToUpper();
                DeviceMacList.Add(new DataItemModel() { Code = i.ToString(), Text = $"0{mac}" });
            }
            SelectdDeviceMacItem = DeviceMacList.Last();

            CmdParamModel = new TT_K7X0_SendCmdParamModel();
            CmdParamModel.MacAddr = Convert.ToByte(SelectdDeviceMacItem.Code);
        }

        public override void DoNextFunction(object obj)
        {
            try
            {
                string param = obj?.ToString();
                switch (param)
                {
                    case "K720_CommOpen":
                        K720_CommOpen();
                        break;
                    case "K720_CommClose":
                        K720_CommClose();
                        break;
                    case "K720_Query":
                        K720_Query();
                        break;
                    case "K720_GetSysVersion":
                        K720_GetSysVersion();
                        break;
                }
            }
            catch (Exception ex)
            {
                StatuMsg = $"操作异常：{ex.Message}";
            }
        }

        private void DoK720_SendCmdFunction(object obj)
        {
            try
            {
                var bRet = false;
                var pCmd = obj?.ToString();
                switch (obj?.ToString())
                {
                    case "DC": bRet = TT_K7X0Helper.SendCmd_DC(CmdParamModel); break;
                    case "CP": bRet = TT_K7X0Helper.SendCmd_CP(CmdParamModel); break;
                    case "RS": bRet = TT_K7X0Helper.SendCmd_RS(CmdParamModel); break;
                    case "BE": bRet = TT_K7X0Helper.SendCmd_BE(CmdParamModel); break;
                    case "BD": bRet = TT_K7X0Helper.SendCmd_BD(CmdParamModel); break;
                    case "DB": bRet = TT_K7X0Helper.SendCmd_DB(CmdParamModel); break;
                    case "FC0": bRet = TT_K7X0Helper.SendCmd_FC0(CmdParamModel); break;
                    case "FC4": bRet = TT_K7X0Helper.SendCmd_FC4(CmdParamModel); break;
                    case "FC6": bRet = TT_K7X0Helper.SendCmd_FC6(CmdParamModel); break;
                    case "FC7": bRet = TT_K7X0Helper.SendCmd_FC7(CmdParamModel); break;
                    case "FC8": bRet = TT_K7X0Helper.SendCmd_FC8(CmdParamModel); break;
                    case "LPX": bRet = TT_K7X0Helper.SendCmd_LPX(CmdParamModel); break;
                }
                StatuMsg = $"发送命令[{pCmd}]" + (bRet ? "成功" : "失败");
                LogHelper.Instance.WriteInfo(StatuMsg);
            }
            catch (Exception ex)
            {
                StatuMsg = $"操作异常：{ex.Message}";
                LogHelper.Instance.WriteError(ex);
            }
        }

        private void K720_CommOpen()
        {
            string comName = SelectDataItem.Code;
            if (TT_K7X0Helper.CommOpen(comName, out IntPtr comHandle))
            {
                StatuMsg = $"打开{comName}成功";
                LogHelper.Instance.WriteInfo(StatuMsg);
                CmdParamModel.ComHandle = comHandle;
                return;
            }
            StatuMsg = $"打开{comName}失败";
            LogHelper.Instance.WriteInfo(StatuMsg);
        }

        private void K720_CommClose()
        {
            bool bRet = TT_K7X0Helper.CommClose(CmdParamModel.ComHandle);
            StatuMsg = $"关闭COM口" + (bRet ? "成功" : "失败");
        }

        private void K720_Query()
        {
            byte macAddr = Convert.ToByte(SelectdDeviceMacItem.Code);
            byte[] stateInfo = new byte[3];

            IntPtr RecordInfo = MemoryUtil.Malloc(1024);
            int ret = TT_K7X0.K720_Query(ComHandle, macAddr, stateInfo, RecordInfo);
            StatuMsg = $"K720_Query，Mac：{macAddr}，Ret：{ret}，StateInfo：{ByteArrayConvert.byteArrayToHexString(stateInfo)}";
            MemoryUtil.FreeArray(RecordInfo);
        }

        private void K720_GetSysVersion()
        {
            byte macAddr = Convert.ToByte(SelectdDeviceMacItem.Code);
            IntPtr RecordInfo = MemoryUtil.Malloc(1024);
            byte[] sysVersion = new byte[20];
            int ret = TT_K7X0.K720_GetVersion(ComHandle, macAddr, sysVersion, RecordInfo);
            StatuMsg = $"K720_GetVersion，Ret：{ret}, {Encoding.UTF8.GetString(sysVersion).TrimEnd('\0')}";
            Log.Instance.WriteInfo(StatuMsg);
            MemoryUtil.FreeArray(RecordInfo);
        }
    }

    public class DataItemModel
    {
        public string Code { get; set; }
        public string Text { get; set; }
    }
}
