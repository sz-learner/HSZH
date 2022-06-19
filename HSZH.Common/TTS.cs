using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Common
{
    public partial class TTS
    {
        private static SoundPlayer sp = new SoundPlayer();
        public TTS()
        {

        }

        /// <summary>
        /// 播放声音文件
        /// </summary>
        /// <param name="SoundFile">声音文件路径</param>
        public static void PlaySound(string SoundFile)
        {
            try
            {
                SoundFile = System.IO.Path.Combine(FileHelper.GetLocalPath(), @"ApplicationData\Sound\" + SoundFile + ".wav");
                if (File.Exists(SoundFile))
                {
                    sp.SoundLocation = SoundFile;
                    sp.Play();
                }
                else
                {
                    Log.Instance.WriteError(SoundFile + " 文件不存在！\r\n");
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError(ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite + "\r\n" + ex.Message);
            }
        }
        /// <summary>
        /// 暂停声音
        /// </summary>
        public static void StopSound()
        {
            TTS.sp?.Stop();
        }
    }
}
