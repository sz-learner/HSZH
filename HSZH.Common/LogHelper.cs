using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Common
{
    public class LogHelper
    {
        private static readonly Lazy<LogHelper> LazyInstance = new Lazy<LogHelper>(() => new LogHelper());

        public static LogHelper Instance { get { return LazyInstance.Value; } }

        private LogHelper()
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.File(System.IO.Path.Combine(FileHelper.GetLocalPath(), "Log") + "\\log_.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        #region 私有方法
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsSerialize"></param>
        /// <param name="type"></param>
        private void WriteLog(object obj, bool IsSerialize = false, LogType type = LogType.Info)
        {
            //StackTrace stackTrace = new StackTrace();
            //StackFrame stackFrame = stackTrace.GetFrame(2);
            //MethodBase methodBase = stackFrame.GetMethod();
            //string message = "ClassName:" + methodBase.ReflectedType.Name + " FunctionName:" + methodBase.Name + " MessageInfo:";
            string message = string.Empty;
            if (IsSerialize)
                message += obj.Serialize();
            else
                message += obj.ToString();
            switch (type)
            {
                case LogType.Debug:
                    Serilog.Log.Debug(message);
                    break;
                case LogType.Error:
                    Serilog.Log.Error(message);
                    break;
                case LogType.Info:
                    Serilog.Log.Information(message);
                    break;
                case LogType.Warn:
                    Serilog.Log.Warning(message);
                    break;
            }
        }
        #endregion

        #region 普通日志
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsSerialize"></param>
        public void WriteInfo(object obj, bool IsSerialize = false)
        {
            WriteLog(obj, IsSerialize, LogType.Info);
        }

        public void WriteInfo(string messageInfo, object obj)
        {
            string msg = messageInfo + "\n" + obj.Serialize();
            WriteLog(msg, false, LogType.Info);
        }
        #endregion

        #region 异常日志
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsSerialize"></param>
        public void WriteError(object obj, bool IsSerialize = false)
        {
            WriteLog(obj, IsSerialize, LogType.Error);
        }
        #endregion

        #region 调试日志
        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsSerialize"></param>
        public void WriteDebug(object obj, bool IsSerialize = false)
        {
            WriteLog(obj, IsSerialize, LogType.Debug);
        }
        #endregion

        #region 警告日志
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsSerialize"></param>
        public void WriteWarn(object obj, bool IsSerialize = false)
        {
            WriteLog(obj, IsSerialize, LogType.Warn);
        }

        public void WriteWarn(string messageInfo, object obj)
        {
            string msg = messageInfo + "\n" + obj.Serialize();
            WriteLog(msg, false, LogType.Info);
        }
        #endregion
    }
}
