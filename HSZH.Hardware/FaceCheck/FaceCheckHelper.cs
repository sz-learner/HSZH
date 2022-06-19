using HSZH.Common;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware.FaceCheck
{
    public partial class FaceCheckHelper
    {
        private IFaceCheck faceCheck = null;

        public FaceCheckHelper()
        {
            
        }

        public static FaceCheckHelper Instance
        {
            get { return SingletonProvider<FaceCheckHelper>.Instance; }
        }

        private void Init(FaceCheckType faceCheckType)
        {
            switch (faceCheckType)
            {
                case FaceCheckType.YiSheng:
                    //faceCheck = new FaceCheckToYS();
                    break;
                case FaceCheckType.BaiDu:
                    //faceCheck = new FaceCheckToBaiDu();
                    break;
                case FaceCheckType.HongRuan:
                    faceCheck = new FaceCheckToHongRuan();
                    break;
                default:
                    //faceCheck = new FaceCheckToBaiDu();
                    break;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public ReturnInfo DoInit(FaceCheckType type, bool IsOpenFaceCheck = true)
        {
            Init(type);
            if (IsOpenFaceCheck)
                return faceCheck.DoInit();
            return ReturnInfo.CreateSuccess();
        }
        /// <summary>
        /// 卸载
        /// </summary>
        /// <returns></returns>
        public ReturnInfo DoUnInit(bool IsOpenFaceCheck = true)
        {
            if (IsOpenFaceCheck)
                return faceCheck.DoUnInit();
            return ReturnInfo.CreateSuccess();
        }
        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="faceCheckInfo"></param>
        /// <param name="IsOpenFaceCheck"></param>
        /// <returns></returns>
        public ReturnInfo DoFaceDetection(FaceCheckInfo faceCheckInfo, bool IsOpenFaceCheck = true)
        {
            if (IsOpenFaceCheck)
                return faceCheck.DoFaceDetection(faceCheckInfo);
            return ReturnInfo.CreateSuccess();
        }
        /// <summary>
        /// 人脸比对
        /// </summary>
        /// <param name="xcrxFaceCheckInfo"></param>
        /// <param name="zjzpFaceCheckInfo"></param>
        /// <param name="faceCheckFZ"></param>
        /// <param name="IsOpenFaceCheck"></param>
        /// <returns></returns>
        public ReturnInfo DoFaceAlignment(FaceCheckInfo xcrxFaceCheckInfo, FaceCheckInfo zjzpFaceCheckInfo, int faceCheckFZ, bool IsOpenFaceCheck = true)
        {
            if (IsOpenFaceCheck)
                return faceCheck.DoFaceAlignment(xcrxFaceCheckInfo, zjzpFaceCheckInfo, faceCheckFZ);
            return ReturnInfo.CreateSuccess();
        }
        /// <summary>
        /// 活体检测
        /// </summary>
        /// <param name="faceCheckInfo"></param>
        /// <param name="IsOpenFaceCheck"></param>
        /// <returns></returns>
        public ReturnInfo DoVivoDetection(FaceCheckInfo faceCheckInfo, bool IsOpenFaceCheck = true)
        {
            if (IsOpenFaceCheck)
                return faceCheck.DoVivoDetection(faceCheckInfo);
            return ReturnInfo.CreateSuccess();
        }
        /// <summary>
        /// 提取特征
        /// </summary>
        /// <param name="faceCheckInfo"></param>
        /// <param name="IsOpenFaceCheck"></param>
        /// <returns></returns>
        public ReturnInfo DoFaceFeature(FaceCheckInfo faceCheckInfo, bool IsOpenFaceCheck = true)
        {
            if (IsOpenFaceCheck)
                return faceCheck.DoFaceFeature(faceCheckInfo);
            return ReturnInfo.CreateSuccess();
        }
    }
}
