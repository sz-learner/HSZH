using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware.FaceCheck
{
    public interface IFaceCheck
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        ReturnInfo DoInit();
        /// <summary>
        /// 卸载
        /// </summary>
        /// <returns></returns>
        ReturnInfo DoUnInit();
        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="faceCheckInfo"></param>
        /// <returns></returns>
        ReturnInfo DoFaceDetection(FaceCheckInfo faceCheckInfo);
        /// <summary>
        /// 人脸比对
        /// </summary>
        /// <param name="faceCheckInfo1"></param>
        /// <param name="faceCheckInfo2"></param>
        /// <param name="faceCheckFZ">人脸比对阀值</param>
        /// <returns></returns>
        ReturnInfo DoFaceAlignment(FaceCheckInfo faceCheckInfo1, FaceCheckInfo faceCheckInfo2, int faceCheckFZ);
        /// <summary>
        /// 活体检测
        /// </summary>
        /// <param name="faceCheckInfo"></param>
        /// <returns></returns>
        ReturnInfo DoVivoDetection(FaceCheckInfo faceCheckInfo);
        /// <summary>
        /// 提取特征
        /// </summary>
        /// <param name="faceCheckInfo"></param>
        /// <returns></returns>
        ReturnInfo DoFaceFeature(FaceCheckInfo faceCheckInfo);
    }
}
