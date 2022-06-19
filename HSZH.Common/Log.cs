using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Common
{
    using log4net;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    #region 日志输出类型枚举
    /// <summary>
    /// 日志输出类型枚举
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug,
        /// <summary>
        /// 信息
        /// </summary>
        Info,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 警告
        /// </summary>
        Warn
    }
    #endregion

    /// <summary>
    /// 通用日志访问类
    /// </summary>
    public class Log
    {
        #region 私有变量
        private static Log _Instance;
        private readonly ILog InfoLogger = LogManager.GetLogger("InfoLog");
        #endregion

        #region 静态属性
        public static Log Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new Log();
                return _Instance;
            }
        }
        #endregion

        #region 构造函数
        private Log()
        {
            string dirpath = FileHelper.GetLocalPath() + @"\ApplicationData\Config";
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            string filepath = FileHelper.GetLocalPath() + @"\ApplicationData\Config\log4net.config";
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
                FileHelper.WriteText(filepath, CreateConfigStr(), Encoding.UTF8);
            }
            FileInfo configFile = new FileInfo(filepath);
            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        #endregion

        #region 析构函数
        ~Log()
        {
            LogManager.Shutdown();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsSerialize"></param>
        /// <param name="type"></param>
        private void WriteLog(object obj, bool IsSerialize = false, LogType type = LogType.Info)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(2);
            MethodBase methodBase = stackFrame.GetMethod();
            string message = "ClassName:" + methodBase.ReflectedType.Name + " FunctionName:" + methodBase.Name + " MessageInfo:";
            if (IsSerialize)
                message += obj.Serialize();
            else
                message += obj.ToString();
            switch (type)
            {
                case LogType.Debug:
                    InfoLogger.Debug(message);
                    break;
                case LogType.Error:
                    InfoLogger.Error(message);
                    break;
                case LogType.Info:
                    InfoLogger.Info(message);
                    break;
                case LogType.Warn:
                    InfoLogger.Warn(message);
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

        #region 生成配置文件字符串
        /// <summary>
        /// 生成配置文件字符串
        /// </summary>
        /// <returns></returns>
        private string CreateConfigStr()
        {
            StringBuilder ConfigStr = new StringBuilder();
            ConfigStr.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            ConfigStr.Append("<configuration>");
            ConfigStr.Append("<configSections>");
            ConfigStr.Append("<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net\"/>");
            ConfigStr.Append("</configSections>");
            ConfigStr.Append("<log4net>");
            ConfigStr.Append("<root>");
            ConfigStr.Append("<level value=\"ERROR\"/>");
            ConfigStr.Append("<level value =\"WARN\"/>");
            ConfigStr.Append("<level value=\"INFO\"/>");
            ConfigStr.Append("<level value=\"DEBUG\"/>");
            ConfigStr.Append("<appender-ref ref=\"ErrorLog\"/>");
            ConfigStr.Append("<appender-ref ref=\"WarnLog\"/>");
            ConfigStr.Append("<appender-ref ref=\"InfoLog\"/>");
            ConfigStr.Append("<appender-ref ref=\"DebugLog\"/>");
            ConfigStr.Append("</root>");
            ConfigStr.Append("<appender name=\"ErrorLog\" type=\"log4net.Appender.RollingFileAppender\">");
            ConfigStr.Append("<param name=\"File\" value=\"Log\"/>");
            ConfigStr.Append("<param name=\"DatePattern\" value=\"/yyyy年MM月/yyyy年MM月dd日'_Error.log'\"/>");
            ConfigStr.Append("<appendToFile value=\"true\"/>");
            ConfigStr.Append("<rollingStyle value=\"Composite\"/>");
            ConfigStr.Append("<staticLogFileName value=\"false\"/>");
            ConfigStr.Append("<maximumFileSize value=\"200MB\"/>");
            ConfigStr.Append("<maxSizeRollBackups value=\"-1\"/>");
            ConfigStr.Append("<layout type=\"log4net.Layout.PatternLayout\"><conversionPattern value=\"%d[%t]%-5p%c[%x]-%m%n\"/></layout>");
            ConfigStr.Append("<filter type=\"log4net.Filter.LevelRangeFilter\"><param name=\"LevelMin\" value=\"ERROR\"/><param name=\"LevelMax\" value=\"ERROR\"/> </filter></appender>");
            ConfigStr.Append("<appender name=\"WarnLog\" type=\"log4net.Appender.RollingFileAppender\">");
            ConfigStr.Append("<param name=\"File\" value=\"Log\"/>");
            ConfigStr.Append("<param name=\"DatePattern\" value=\"/yyyy年MM月/yyyy年MM月dd日'_Warn.log'\"/>");
            ConfigStr.Append("<appendToFile value=\"true\"/>");
            ConfigStr.Append("<rollingStyle value=\"Composite\"/>");
            ConfigStr.Append("<staticLogFileName value=\"false\"/>");
            ConfigStr.Append("<maximumFileSize value=\"200MB\"/>");
            ConfigStr.Append("<maxSizeRollBackups value=\"-1\"/>");
            ConfigStr.Append("<layout type=\"log4net.Layout.PatternLayout\"><conversionPattern value=\"%d[%t]%-5p%c[%x]-%m%n\"/></layout>");
            ConfigStr.Append("<filter type=\"log4net.Filter.LevelRangeFilter\"><param name=\"LevelMin\" value=\"WARN\"/><param name=\"LevelMax\" value=\"WARN\"/> </filter></appender>");
            ConfigStr.Append("<appender name=\"InfoLog\" type=\"log4net.Appender.RollingFileAppender\">");
            ConfigStr.Append("<param name=\"File\" value=\"Log\"/>");
            ConfigStr.Append("<param name=\"DatePattern\" value=\"/yyyy年MM月/yyyy年MM月dd日'_Info.log'\"/>");
            ConfigStr.Append("<appendToFile value=\"true\"/>");
            ConfigStr.Append("<rollingStyle value=\"Composite\"/>");
            ConfigStr.Append("<staticLogFileName value=\"false\"/>");
            ConfigStr.Append("<maximumFileSize value=\"200MB\"/>");
            ConfigStr.Append("<maxSizeRollBackups value=\"-1\"/>");
            ConfigStr.Append("<layout type=\"log4net.Layout.PatternLayout\"><conversionPattern value=\"%d[%t]%-5p%c[%x]-%m%n\"/></layout>");
            ConfigStr.Append("<filter type=\"log4net.Filter.LevelRangeFilter\"><param name=\"LevelMin\" value=\"INFO\"/><param name=\"LevelMax\" value=\"INFO\"/> </filter></appender>");
            ConfigStr.Append("<appender name=\"DebugLog\" type=\"log4net.Appender.RollingFileAppender\">");
            ConfigStr.Append("<param name=\"File\" value=\"Log\"/>");
            ConfigStr.Append("<param name=\"DatePattern\" value=\"/yyyy年MM月/yyyy年MM月dd日'_Debug.log'\"/>");
            ConfigStr.Append("<appendToFile value=\"true\"/>");
            ConfigStr.Append("<rollingStyle value=\"Composite\"/>");
            ConfigStr.Append("<staticLogFileName value=\"false\"/>");
            ConfigStr.Append("<maximumFileSize value=\"200MB\"/>");
            ConfigStr.Append("<maxSizeRollBackups value=\"-1\"/>");
            ConfigStr.Append("<layout type=\"log4net.Layout.PatternLayout\"><conversionPattern value=\"%d[%t]%-5p%c[%x]-%m%n\"/></layout>");
            ConfigStr.Append("<filter type=\"log4net.Filter.LevelRangeFilter\"><param name=\"LevelMin\" value=\"DEBUG\"/><param name=\"LevelMax\" value=\"DEBUG\"/> </filter></appender>");
            ConfigStr.Append("</log4net>");
            ConfigStr.Append("</configuration>");
            return ConfigStr.ToString();
        }
        #endregion
    }
}
