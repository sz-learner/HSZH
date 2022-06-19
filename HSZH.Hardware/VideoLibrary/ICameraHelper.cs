using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freedom.Hardware.VideoLibrary
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bitmap"></param>
    public delegate void CameraCaptureDelegate(Bitmap bitmap);

    /// <summary>
    /// 
    /// </summary>
    public interface ICameraHelper
    {
        /// <summary>
        /// 
        /// </summary>
        event CameraCaptureDelegate VideoCaptureEvent;

        /// <summary>
        /// 
        /// </summary>
        Bitmap FrameBitmap { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cameraIndex"></param>
        /// <returns></returns>
        ReturnInfo OpenCamera(int cameraIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ReturnInfo CloseCamera();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cameraIndex"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        ReturnInfo StartVideoRecord(int cameraIndex, int capabilitieIndex, string saveFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cameraIndex"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        ReturnInfo StopVideoRecord();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<CameraDeviceInfo> GetCameras();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<DeviceCapabilityInfo> GetDeviceCapability(CameraDeviceInfo deviceInfo);
    }
}
