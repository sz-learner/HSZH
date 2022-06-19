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
        private int ComHandle = -1;
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

            LogHelper.Instance.WriteInfo($"demo");
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
                var pCmd = obj?.ToString();
                var macAddr = Convert.ToByte(SelectdDeviceMacItem.Code);

                LogHelper.Instance.WriteInfo($"K720_SendCmd {pCmd}, Mac：{macAddr}");
                IntPtr pRecordInfo = MemoryUtil.Malloc(1024);
                int ret = TT_K7X0.K720_SendCmd(ComHandle, macAddr, pCmd, pCmd.Length, pRecordInfo);
                LogHelper.Instance.WriteInfo($"K720_SendCmd Ret {ret}");
                StatuMsg = $"K720_SendCmd {pCmd}，Mac：{macAddr}，Ret：{ret}";

                var sRecordInfo = Marshal.PtrToStringAnsi(pRecordInfo);
                MemoryUtil.FreeArray(pRecordInfo);
            }
            catch (Exception ex)
            {
                StatuMsg = $"操作异常：{ex.Message}";
            }
        }

        private void K720_CommOpen()
        {
            string com = SelectDataItem.Code;
            ComHandle = TT_K7X0.K720_CommOpen(com);
            StatuMsg = $"K720_CommOpen，Param：{com}，Ret：{ComHandle}";
        }

        private void K720_CommClose()
        {
            int ret = TT_K7X0.K720_CommClose(ComHandle);
            StatuMsg = $"K720_CommClose，Ret：{ret}";
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
