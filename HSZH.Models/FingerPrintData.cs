using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Models
{
    [Serializable]
    public partial class FingerPrintData
    {
        public FingerPrintData()
        {

        }

        public FingerPrintData(int i)
        {
            FingerPrintNumber = 12;
            FingerPrintName = "右手拇指";
            FingerPrintQuality = 65;
            FingerPrintdata = null;

        }

        /// <summary>
        /// 指纹编号
        /// </summary>
        public byte FingerPrintNumber { get; set; }
        /// <summary>
        /// 指纹指位名称
        /// </summary>
        public string FingerPrintName { get; set; }
        /// <summary>
        /// 指纹质量
        /// </summary>
        public byte FingerPrintQuality { get; set; }
        /// <summary>
        /// 特征数据
        /// </summary>
        public byte[] FingerPrintdata { get; set; }
        /// <summary>
        /// 指纹压缩图像数据
        /// </summary>
        public byte[] FingerCompressTXdata { get; set; }
        /// <summary>
        /// 图像数据
        /// </summary>
        public byte[] ImageData { get; set; }
        /// <summary>
        /// 图像质量
        /// </summary>
        public int ImageQuality { get; set; }
        /// <summary>
        /// 指位
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] TPL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TPLQuality { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] WSQ1K { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] WSQ4K { get; set; }
        /// <summary>
        /// 指纹照片
        /// </summary>
        public string Finger_JPEG { get; set; }
        /// <summary>
        /// 指纹采集现场照片
        /// </summary>
        public string Finger_CJXCZP { get; set; }
    }
}
