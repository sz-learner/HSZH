using Emgu.CV;
using HSZH.Common;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freedom.Hardware.VideoLibrary
{
    public class EmgucvCameraImpl : ICameraHelper
    {
        public event CameraCaptureDelegate VideoCaptureEvent;

        private Capture capture = null;
        private Mat _frame;
        private Mat _TakeFrame;

        public Bitmap FrameBitmap
        {
            get
            {
                if (_TakeFrame == null)
                    _TakeFrame = new Mat();
                capture.Retrieve(_TakeFrame, 0);
                return _TakeFrame.Bitmap;
            }
        }

        /// <summary>
        /// 打开摄像头
        /// </summary>
        /// <param name="CameraIndex"></param>
        /// <returns></returns>
        public ReturnInfo OpenCamera(int CameraIndex = 0)
        {
            try
            {
                if (capture == null)
                {
                    capture = new Capture(CameraIndex);
                }
                capture.ImageGrabbed += Capture_ImageGrabbed;
                _frame = new Mat();
                capture.Start();
                if (capture.Ptr != IntPtr.Zero)
                    return ReturnInfo.CreateSuccess();
                else
                    return ReturnInfo.CreateError();
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
                if (capture != null)
                {
                    if (VideoCaptureEvent != null)
                        VideoCaptureEvent = null;
                    capture.ImageGrabbed -= Capture_ImageGrabbed;
                    capture.Stop();
                    capture.Dispose();
                    capture = null;
                }
                return ReturnInfo.CreateSuccess();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("关闭摄像头异常，异常信息：" + ex.Message);
                return ReturnInfo.CreateException(ex);
            }
        }

        /// <summary>
        /// 摄像头回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            var NewCapture = sender as Capture;
            if (NewCapture.Ptr != IntPtr.Zero)
            {
                NewCapture.Retrieve(_frame, 0);
                if (_frame != null)
                {
                    if (VideoCaptureEvent != null)
                        VideoCaptureEvent.Invoke(_frame.Bitmap);
                }
            }
        }

        public ReturnInfo StartVideoRecord(int cameraIndex, int capabilitieIndex, string saveFileName)
        {
            throw new NotImplementedException();
        }

        public ReturnInfo StopVideoRecord()
        {
            throw new NotImplementedException();
        }

        public List<CameraDeviceInfo> GetCameras()
        {
            throw new NotImplementedException();
        }

        public List<DeviceCapabilityInfo> GetDeviceCapability(CameraDeviceInfo deviceInfo)
        {
            throw new NotImplementedException();
        }
    }
}
