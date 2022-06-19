using Accord.Video;
using Accord.Video.DirectShow;
using HSZH.Common;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Freedom.Hardware.VideoLibrary
{
    public class AccordCameraImpl : ICameraHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private VideoCaptureDevice captureDevice;

        /// <summary>
        /// 
        /// </summary>
        public event CameraCaptureDelegate VideoCaptureEvent;

        /// <summary>
        /// 
        /// </summary>
        public Bitmap FrameBitmap { get; private set; }

        /// <summary>
        /// 打开摄像头
        /// </summary>
        /// <param name="CameraIndex"></param>
        /// <returns></returns>
        public ReturnInfo OpenCamera(int CameraIndex = 0)
        {
            try
            {
                FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (captureDevice != null && captureDevice.IsRunning)
                    captureDevice.Stop();
                captureDevice = new VideoCaptureDevice(filterInfoCollection[CameraIndex].MonikerString);
                captureDevice.NewFrame += captureDevice_NewFrame;
                captureDevice.Start();
                return ReturnInfo.CreateSuccess();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("打开摄像头异常，异常信息：" + ex.Message);
                return ReturnInfo.CreateException(ex);
            }
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        /// <returns></returns>
        public ReturnInfo CloseCamera()
        {
            try
            {
                if (captureDevice != null)
                {
                    captureDevice.NewFrame -= captureDevice_NewFrame;
                    this.FrameBitmap = null;
                    if (captureDevice.IsRunning)
                    {
                        captureDevice.SignalToStop();
                    }
                    if (VideoCaptureEvent != null)
                        VideoCaptureEvent = null;
                    captureDevice = null;
                }
                return ReturnInfo.CreateSuccess();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("关闭摄像头异常，异常信息：" + ex.Message);
                return ReturnInfo.CreateException(ex);
            }
        }

        // <summary>
        /// 摄像头回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void captureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap =(Bitmap)eventArgs.Frame.Clone();
            this.FrameBitmap = bitmap;
            this.VideoCaptureEvent?.Invoke((Bitmap)eventArgs.Frame.Clone());
        }

        /// <summary>
        /// 开始视频录像
        /// </summary>
        /// <param name="cameraIndex"></param>
        /// <param name="saveFileName"></param>
        /// <returns></returns>
        public ReturnInfo StartVideoRecord(int cameraIndex, int capabilitieIndex, string saveFileName)
        {
            return ReturnInfo.CreateError();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReturnInfo StopVideoRecord()
        {
            return ReturnInfo.CreateError();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CameraDeviceInfo> GetCameras()
        {
            List<CameraDeviceInfo> list = new List<CameraDeviceInfo>();
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in videoDevices)
            {
                list.Add(new CameraDeviceInfo(info.Name, info.MonikerString, list.Count(), FilterCategory.VideoInputDevice));
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        public List<DeviceCapabilityInfo> GetDeviceCapability(CameraDeviceInfo deviceInfo)
        {
            List<DeviceCapabilityInfo> list = new List<DeviceCapabilityInfo>();
            VideoCaptureDevice device = new VideoCaptureDevice(deviceInfo.MonikerString);
            for (int i = 0; i < device.VideoCapabilities.Length; i++)
            {
                VideoCapabilities capabilities = device.VideoCapabilities[i];
                DeviceCapabilityInfo item = new DeviceCapabilityInfo(capabilities.FrameSize, capabilities.MaximumFrameRate);
                list.Add(item);
            }
            return list;
        }
    }
}
