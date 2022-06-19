using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Models
{
    public enum ReturnType
    {
        Success = 0,
        Failed = 1,
        Exception = 2,
        Error = 3,
    }

    [Serializable]
    public partial class ReturnInfo
    {
        public ReturnInfo()
        {

        }

        public ReturnInfo(ReturnType rtype, string rmessage)
        {
            this.ExeResult = rtype;
            this.MessageInfo = rmessage;
        }

        public ReturnInfo(ReturnType rType, string messInfo, object rValue, int sMarker)
        {
            this.ExeResult = rType;
            this.MessageInfo = messInfo;
            this.ReturnValue = rValue;
            this.SignalMarker = sMarker;
        }

        /// <summary>
        /// 枚举标记
        /// </summary>
        public ReturnType ExeResult { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string MessageInfo { get; set; }
        /// <summary>
        /// 预留存储
        /// </summary>
        public object ReturnValue { get; set; }
        /// <summary>
        /// 信号标记
        /// </summary>
        public long SignalMarker { get; set; }
        /// <summary>
        /// 功能ID
        /// </summary>
        public string FucID { get; set; }

        public const string SuccessString = "00000";
        public const string SuccessNoRectords = "00020";
        public const string FailString = "00010";
        public const string ExceptionString = "0001a";


        public bool IsSucceed
        {
            get { return ExeResult == ReturnType.Success; }
        }

        public bool IsException
        {
            get { return ExeResult == ReturnType.Exception; }
        }

        public static ReturnInfo CreateError(string messageInfo = "")
        {
            return new ReturnInfo(ReturnType.Error, messageInfo);
        }

        public static ReturnInfo CreateFailed(string messageInfo = "")
        {
            return new ReturnInfo(ReturnType.Failed, messageInfo);
        }

        public static ReturnInfo CreateSuccess(string messageInfo = "")
        {
            return new ReturnInfo(ReturnType.Success, messageInfo);
        }

        public static ReturnInfo CreateException(Exception ex)
        {
            return new ReturnInfo(ReturnType.Exception, ex.ToString());
        }

        public override string ToString()
        {
            return string.Format("[ReturnInfo]状态:{0},消息:{1}", this.ExeResult.ToString(), MessageInfo);
        }
    }
}
