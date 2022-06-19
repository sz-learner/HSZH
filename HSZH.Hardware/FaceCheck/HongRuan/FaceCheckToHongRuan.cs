using HSZH.Common;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware.FaceCheck
{
    public class FaceCheckToHongRuan : IFaceCheck
    {
        private FaceEngine faceEngine = new FaceEngine();

        public ReturnInfo DoInit()
        {
            try
            {
                if (faceEngine.GetEngineStatus())
                {
                    Log.Instance.WriteInfo("引擎已初始化");
                    return ReturnInfo.CreateSuccess();
                }

                int retcode = faceEngine.ASFOfflineActivation("license.dat");
                if (retcode != 0 && retcode != 90114)
                {
                    Log.Instance.WriteInfo("激活SDK失败, 错误码: " + retcode);
                    return ReturnInfo.CreateError();
                }
                Log.Instance.WriteInfo("激活SDK成功: " + retcode);

                DetectionMode detectMode = DetectionMode.ASF_DETECT_MODE_IMAGE;
                ASF_OrientPriority orientPriority = ASF_OrientPriority.ASF_OP_0_ONLY;
                int detectFaceMaxNum = 5;
                int combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_IMAGEQUALITY | FaceEngineMask.ASF_LIVENESS | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_MASKDETECT;
                retcode = faceEngine.ASFInitEngine(detectMode, orientPriority, detectFaceMaxNum, combinedMask);
                if (retcode != 0)
                {
                    Log.Instance.WriteInfo("引擎初始化失败, 错误码: " + retcode);
                    return ReturnInfo.CreateError();
                }

                Log.Instance.WriteInfo("引擎初始化成功");
                return ReturnInfo.CreateSuccess();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("初始化人脸识别算法异常：" + ex.Message);
                return ReturnInfo.CreateException(ex);
            }
        }

        public ReturnInfo DoUnInit()
        {
            try
            {
                faceEngine.ASFUninitEngine();
                return ReturnInfo.CreateSuccess();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("释放人脸识别算法引擎异常：" + ex.Message);
                return ReturnInfo.CreateException(ex);
            }
        }

        public ReturnInfo DoFaceAlignment(FaceCheckInfo xcrxFaceCheckInfo, FaceCheckInfo zjzpFaceCheckInfo, int faceCheckFZ)
        {
            Log.Instance.WriteWarn($"人脸特征比对");
            FaceFeature xcrxFaceFeature = xcrxFaceCheckInfo.Feature as FaceFeature;
            FaceFeature zjzpFaceFeature = zjzpFaceCheckInfo.Feature as FaceFeature;
            int retcode = faceEngine.ASFFaceFeatureCompare(xcrxFaceFeature, zjzpFaceFeature, out float similarity, ASF_CompareModel.ASF_ID_PHOTO);
            Log.Instance.WriteWarn($"特征比对结果：{retcode}");
            if (retcode != 0)
            {
                Log.Instance.WriteWarn("人脸特征比对失败");
                return ReturnInfo.CreateFailed();
            }
            Log.Instance.WriteWarn("特征比对相似度返回值：" + similarity.ToString());
            if (similarity.ToString().IndexOf("E") > -1)
                similarity = 0f;
            if (Convert.ToInt32(similarity * 100.0) < faceCheckFZ)
            {
                Log.Instance.WriteWarn("比对失败, 比对相似度低于设定阀值");
                return ReturnInfo.CreateFailed();
            }
            return ReturnInfo.CreateSuccess();
        }

        public ReturnInfo DoFaceDetection(FaceCheckInfo faceCheckInfo)
        {
            if (faceCheckInfo.Image == null)
            {
                return ReturnInfo.CreateFailed("现场照片为空");
            }

            int retcode = faceEngine.ASFDetectFacesEx(faceCheckInfo.Image, out MultiFaceInfo faceInfo);
            if (retcode != 0 || faceInfo.faceNum < 1)
            {
                return ReturnInfo.CreateFailed("未检测到人脸信息");
            }

            retcode = CheckMask(faceCheckInfo.Image, faceInfo);
            if (retcode != 0)
            {
                return ReturnInfo.CreateFailed("检测到带口罩");
            }

            retcode = faceEngine.ASFFaceFeatureExtractEx(faceCheckInfo.Image, faceInfo, ASF_RegisterOrNot.ASF_REGISTER, out FaceFeature feature, GetMaxFaceIndex(faceInfo));
            if (retcode != 0 || feature == null)
            {
                Log.Instance.WriteWarn($"提取人脸特征失败：{retcode}");
                return ReturnInfo.CreateFailed();
            }

            faceCheckInfo.FaceInfo = faceInfo;
            faceCheckInfo.Feature = feature;
            return ReturnInfo.CreateSuccess();
        }

        public ReturnInfo DoVivoDetection(FaceCheckInfo faceCheckInfo)
        {
            LivenessInfo liveInfo = FaceUtil.LivenessInfo_RGB(faceEngine, faceCheckInfo.Image, GetMaxFaceInfo(faceCheckInfo.FaceInfo as MultiFaceInfo), out int retcode);
            if (retcode != 0)
            {
                return ReturnInfo.CreateFailed();
            }

            if (liveInfo.isLive.Length == 0)
            {
                return ReturnInfo.CreateFailed();
            }

            if (liveInfo.isLive[0] != 1)
            {
                return ReturnInfo.CreateFailed();
            }

            return ReturnInfo.CreateSuccess();
        }

        public ReturnInfo DoFaceFeature(FaceCheckInfo faceCheckInfo)
        {
            if (faceCheckInfo.Image.Width % 4 != 0)
            {
                faceCheckInfo.Image = ImageUtil.ScaleImage(faceCheckInfo.Image, faceCheckInfo.Image.Width - (faceCheckInfo.Image.Width % 4), faceCheckInfo.Image.Height);
            }

            int retcode = faceEngine.ASFDetectFacesEx(faceCheckInfo.Image, out MultiFaceInfo faceInfo);
            if (retcode != 0 || faceInfo.faceNum < 1)
            {
                return ReturnInfo.CreateFailed("人脸检测失败");
            }

            retcode = faceEngine.ASFFaceFeatureExtractEx(faceCheckInfo.Image, faceInfo, ASF_RegisterOrNot.ASF_REGISTER, out FaceFeature feature, GetMaxFaceIndex(faceInfo));
            if (retcode != 0 || feature == null)
            {
                return ReturnInfo.CreateFailed("提取人脸特征失败");
            }

            faceCheckInfo.Feature = feature;
            return ReturnInfo.CreateSuccess();
        }

        private SingleFaceInfo GetMaxFaceInfo(MultiFaceInfo multiFaceInfo)
        {
            int maxIndex = GetMaxFaceIndex(multiFaceInfo);
            SingleFaceInfo singleFaceInfo = new SingleFaceInfo();
            singleFaceInfo.faceOrient = multiFaceInfo.faceOrients[maxIndex];
            singleFaceInfo.faceRect = multiFaceInfo.faceRects[maxIndex];
            singleFaceInfo.faceDataInfo = multiFaceInfo.faceDataInfoList[maxIndex];
            return singleFaceInfo;
        }

        private int GetMaxFaceIndex(MultiFaceInfo multiFaceInfo)
        {
            int dataSize = 0;
            int maxIndex = 0;
            for (int i = 0; i < multiFaceInfo.faceNum; i++)
            {
                int tempSize = multiFaceInfo.faceDataInfoList[i].dataSize;
                if (tempSize > dataSize)
                {
                    dataSize = tempSize;
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        private int CheckMask(Image image, MultiFaceInfo faceInfo)
        {
            int retcode = faceEngine.ASFProcessEx(image, faceInfo, FaceEngineMask.ASF_MASKDETECT);
            Log.Instance.WriteWarn($"人脸信息检测-口罩：{retcode}");
            if (retcode == 0)
            {
                retcode = faceEngine.ASFGetMask(out MaskInfo maskInfo);
                Log.Instance.WriteWarn($"口罩检测结果：{retcode}");
                if (retcode == 0 && maskInfo != null && maskInfo.maskArray != null && maskInfo.maskArray.Length > 0 && maskInfo.maskArray[0].Equals(1))
                {
                    Log.Instance.WriteWarn($"检测到带口罩");
                    return 1;
                }
            }
            return retcode;
        }
    }
}
