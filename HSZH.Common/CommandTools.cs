using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HSZH.Common
{
    public class CommandTools
    {
        #region 读写Ini
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 读出INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        public static string IniReadValue(string Section, string Key, string path)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, path);
            return temp.ToString();
        }

        /// <summary>
        /// 写入Ini文件
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="Section">节点名</param>
        /// <param name="Key">要写入的项</param>
        /// <param name="Value">要写入的值</param>
        public static void IniWriteValue(string path, string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }
        #endregion

        public static string ImgToBase64(string filepath)
        {
            string result = "";
            try
            {
                if (!File.Exists(filepath))
                    return "";
                Bitmap bmp = new Bitmap(filepath);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                result = Convert.ToBase64String(arr);
                bmp.Dispose();
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        public static BitmapSource ImageToBitmapSource(Bitmap bitmap)
        {
            try
            {
                IntPtr f = bitmap.GetHbitmap();
                var source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(f, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                Win32API.DeleteObject(f);
                bitmap.Dispose();
                return source;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("图片转换异常，异常信息：" + ex.Message);
                return null;
            }
        }

        public static Bitmap CopyBitmap(Bitmap bitmap)
        {
            try
            {
                if (bitmap == null)
                    return null;
                Bitmap dstBitmap = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, bitmap);
                    ms.Seek(0, SeekOrigin.Begin);
                    dstBitmap = (Bitmap)bf.Deserialize(ms);
                    ms.Close();
                }
                return dstBitmap;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError(ex.Message);
                return null;
            }
        }

        public static byte[] GetImageByte(Image img)
        {
            byte[] buffer = null;
            if (!img.Equals(null))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    new Bitmap(img).Save(stream, ImageFormat.Bmp);
                    buffer = new byte[stream.Length];
                    stream.Position = 0L;
                    stream.Read(buffer, 0, Convert.ToInt32(buffer.Length));
                }
            }
            return buffer;
        }
    }
}
