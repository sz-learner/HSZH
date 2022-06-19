using HSZH.Common;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Config
{
    public class AppConfig
    {
        private static string directoryHomePath = Path.Combine(FileHelper.GetLocalPath(), @"ApplicationData\Config");

        private static readonly Lazy<AppConfig> LazyInstance = new Lazy<AppConfig>(() => new AppConfig());
        public static AppConfig Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        public SysConfig SysConfig { get; private set; }

        public ReturnInfo LoadConfig()
        {
            try
            {
                if (!Directory.Exists(directoryHomePath))
                    Directory.CreateDirectory(directoryHomePath);
                string configPath = Path.Combine(directoryHomePath, "SysConfig.json");
                if (!File.Exists(configPath))
                {
                    File.Create(configPath).Close();
                    SysConfig = new SysConfig();
                    SetConfigDefaultValue(SysConfig);
                    return new ReturnInfo();
                }

                SysConfig = JsonHelper.ConvertToObject<SysConfig>(FileHelper.ReadFileContent(configPath, Encoding.UTF8));
                SetConfigDefaultValue(SysConfig);
                return new ReturnInfo();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("加载大集中参数发生异常，异常信息:" + ex.Message);
                ReturnInfo rinfo = new ReturnInfo();
                rinfo.ExeResult = ReturnType.Exception;
                rinfo.ReturnValue = null;
                rinfo.MessageInfo = ex.Message;
                return rinfo;
            }
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <param name="model"></param>
        private static void SetConfigDefaultValue(SysConfig model)
        {
            if (model.AppExitPassWord.IsEmpty())
                model.AppExitPassWord = "123456";
            if (model.FaceCheckFZ == 0)
                model.FaceCheckFZ = 60;
            if (model.TT_K7X0_ComPortName.IsEmpty())
                model.TT_K7X0_ComPortName = "COM1";
            SaveSysConfig(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool SaveSysConfig(SysConfig model)
        {
            if (model.IsNotEmpty())
            {
                var configjson = JsonHelper.ToJson(model);
                var configPath = Path.Combine(directoryHomePath, "SysConfig.json");
                FileHelper.WriteText(configPath, configjson, Encoding.UTF8);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
