using Freedom.Hardware.VideoLibrary;
using HSZH.Common;
using HSZH.Config;
using HSZH.Hardware.FaceCheck;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HSZH.FKTerminal.ViewModels
{
    public class FaceCheckViewModel : ObservableObject
    {
        #region 私有变量
        private string zzzpBase64Str = string.Empty;
        private Thread FaceCheckThread = null;
        private Visibility _BtnNextVisibility;
        private BitmapSource _TakePhototImage;
        private readonly ICameraHelper cameraHelper;
        private string _FooterMessageInfo;
        private string xcrxPath = FileHelper.GetLocalPath() + "\\FaceCheckPhoto\\xcrx.jpg";
        private string zjzpPath = FileHelper.GetLocalPath() + "\\FaceCheckPhoto\\zjzp.jpg";
        #endregion

        #region 通知属性
        public BitmapSource TakePhototImage
        {
            get { return _TakePhototImage; }
            set { _TakePhototImage = value; base.RaisePropertyChanged("TakePhototImage"); }
        }

        public Visibility BtnNextVisibility
        {
            get { return _BtnNextVisibility; }
            set { _BtnNextVisibility = value; base.RaisePropertyChanged("BtnNextVisibility"); }
        }

        public string FooterMessageInfo
        {
            get => _FooterMessageInfo;
            set { _FooterMessageInfo = value; base.RaisePropertyChanged("FooterMessageInfo"); }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FaceCheckViewModel()
        {
            switch (AppConfig.Instance.SysConfig.CameraConnType)
            {
                case CameraConnType.Accord:
                    cameraHelper = new AccordCameraImpl();
                    break;
                case CameraConnType.Emgucv:
                    cameraHelper = new EmgucvCameraImpl();
                    break;
                default:
                    cameraHelper = new AccordCameraImpl();
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 初始化事件
        /// </summary>
        /// <param name="obj"></param>
        public override void DoInitFunction(object obj)
        {
            try
            {
                Log.Instance.WriteInfo("进入人像比对界面");
                //zzzpBase64Str = CommandTools.ImgToBase64(zjzpPath);
                //if (zzzpBase64Str.IsEmpty())
                //{
                //    Log.Instance.WriteInfo("驾驶证制证照片数据为空！");
                //    MainWindowViewModel.Instance.SetFrameMessagePage("抱歉，获取制证照片失败，请联系工作人员！", true, false);
                //    return;
                //}
                if (!cameraHelper.OpenCamera(AppConfig.Instance.SysConfig.CameraIndex).IsSucceed)
                {
                    Log.Instance.WriteInfo("打开摄像头失败, 返回主界面!");
                    MainWindowViewModel.Instance.SetFrameMessagePage("抱歉，打开摄像头失败，请联系工作人员！", true, false);
                    return;
                }

                cameraHelper.VideoCaptureEvent += CameraHelper_VideoCaptureEvent;
                BtnNextVisibility = Visibility.Collapsed;
                TTS.PlaySound("人像比对");
                MainWindowViewModel.Instance.TimeOutEventAction += Instance_TimeOutEventAction;
                MainWindowViewModel.Instance.OpenTimeOut(120);
                FaceCheckThread = new Thread(new ThreadStart(delegate { SwitchFaceCheck(FaceCheckImg); }));
                FaceCheckThread.IsBackground = true;
                FaceCheckThread.Start();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("初始化摄像头界面异常，异常信息:" + ex.Message);
                CloseCamera();
                MainWindowViewModel.Instance.SetFrameMessagePage("抱歉，操作摄像头异常，请联系工作人员！", true, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void DoExitFunction(object obj)
        {
            Log.Instance.WriteInfo("申请人点击了取消按钮，流程结束！");
            MainWindowViewModel.Instance.StopTimeOut();
            CloseFaceCheckThread();
            CloseCamera();
            MainWindowViewModel.Instance.ReturnHome();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void DoNextFunction(object obj)
        {
            Log.Instance.WriteInfo("跳转到下一个界面！");
            MainWindowViewModel.Instance.StopTimeOut();
            CloseFaceCheckThread();
            CloseCamera();
            //GlovalToIntersection.Instance.QueryInfo();
        }

        /// <summary>
        /// 超时回调函数
        /// </summary>
        private void Instance_TimeOutEventAction()
        {
            Log.Instance.WriteInfo("比对超时");
            CloseFaceCheckThread();
            CloseCamera();
            MainWindowViewModel.Instance.SetFrameMessagePage("抱歉，人像比对超时，请联系工作人员！", true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funFaceCheck"></param>
        private void SwitchFaceCheck(Func<bool> funFaceCheck)
        {
            try
            {
                Log.Instance.WriteInfo("开始比对制证照片");
                bool checkIsOK = funFaceCheck();
                Log.Instance.WriteInfo("制证照片比对结果：" + checkIsOK.ToString());
                MainWindowViewModel.Instance.StopTimeOut();

                if (checkIsOK)
                {
                    TTS.PlaySound("比对成功");
                    Log.Instance.WriteInfo("比对成功");
                    CloseCamera();
                    MainWindowViewModel.Instance.SetFrameTraget("CardStatus");
                    return;
                }

                Log.Instance.WriteInfo("比对失败");
                CloseCamera();
                MainWindowViewModel.Instance.SetFrameMessagePage("抱歉，人像比对失败，请联系工作人员！", true);
                return;
            }
            catch (ThreadAbortException abortEx)
            {
                Log.Instance.WriteInfo("释放比对线程，描述信息：" + abortEx.Message);
                MainWindowViewModel.Instance.StopTimeOut();
                CloseCamera();
                return;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("人像比对异常，异常信息：" + ex.Message);
                MainWindowViewModel.Instance.StopTimeOut();
                CloseCamera();
                MainWindowViewModel.Instance.SetFrameMessagePage("抱歉，人像比对异常，请联系工作人员！", true);
            }
        }

        private bool FaceCheckImgTest()
        {
            try
            {
                FaceCheckInfo xcrxFaceCheckInfo = new FaceCheckInfo();
                int times = 0;
                while (true)
                {
                    if(cameraHelper.FrameBitmap == null)
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    xcrxFaceCheckInfo.Image = CommandTools.CopyBitmap(cameraHelper.FrameBitmap);
                    if (xcrxFaceCheckInfo.Image == null)
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    if (++times < 50)
                    {
                        continue;
                    }
                    break;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("比对发生异常，异常信息：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 人像比对
        /// </summary>
        /// <param name="bdzpPath"></param>
        /// <returns></returns>
        private bool FaceCheckImg()
        {
            try
            {
                if (!AppConfig.Instance.SysConfig.IsOpenFaceCheck)
                {
                    return FaceCheckImgTest();
                }

                int currCheckNumber = 0;
                Log.Instance.WriteInfo("人像比对照片路径：" + zjzpPath);
                FaceCheckInfo zzzpFaceCheckInfo = new FaceCheckInfo() { Image = Image.FromFile(zjzpPath) };

                ReturnInfo rinfo;
                rinfo = FaceCheckHelper.Instance.DoFaceFeature(zzzpFaceCheckInfo);
                if (!rinfo.IsSucceed)
                {
                    Log.Instance.WriteInfo("提取证件照片人脸特征失败");
                    return false;
                }

                bool nflag = false;
                FaceCheckInfo xcrxFaceCheckInfo = new FaceCheckInfo();
                while (true)
                {
                    xcrxFaceCheckInfo.Image = CommandTools.CopyBitmap(cameraHelper.FrameBitmap);
                    if (xcrxFaceCheckInfo.Image == null)
                    {
                        FooterMessageInfo = "检测不到现场照片...";
                        Thread.Sleep(100);
                        continue;
                    }

                    rinfo = FaceCheckHelper.Instance.DoFaceDetection(xcrxFaceCheckInfo);
                    if (!rinfo.IsSucceed)
                    {
                        FooterMessageInfo = "检测不到本人...";
                        Thread.Sleep(500);
                        continue;
                    }

                    rinfo = FaceCheckHelper.Instance.DoVivoDetection(xcrxFaceCheckInfo);
                    if (!rinfo.IsSucceed)
                    {
                        FooterMessageInfo = "检测不到本人...";
                        Thread.Sleep(500);
                        continue;
                    }

                    FooterMessageInfo = "正在进行人脸比对...";
                    rinfo = FaceCheckHelper.Instance.DoFaceAlignment(xcrxFaceCheckInfo, zzzpFaceCheckInfo, AppConfig.Instance.SysConfig.FaceCheckFZ);
                    if (!rinfo.IsSucceed)
                    {
                        Log.Instance.WriteInfo("证件照片比对失败");
                        FooterMessageInfo = "人脸比对失败";
                        if (++currCheckNumber > AppConfig.Instance.SysConfig.FaceCheckNumber)
                        {
                            Log.Instance.WriteInfo("人脸比对失败次数超过上限，比对失败！");
                            break;
                        }
                        Thread.Sleep(1000);
                        continue;
                    }

                    nflag = true;
                    Log.Instance.WriteInfo("比对成功");
                    FooterMessageInfo = "人脸比对成功";
                    TTS.PlaySound("比对成功");
                    try
                    {
                        byte[] xcbuf = CommandTools.GetImageByte(xcrxFaceCheckInfo.Image);
                        MemoryStream ms = new MemoryStream(xcbuf);
                        FileStream fs = new FileStream(xcrxPath, FileMode.Create);
                        ms.WriteTo(fs);
                        ms.Close();
                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.WriteError("保存现场人像异常：" + ex.Message);
                    }
                    Thread.Sleep(500);
                    BtnNextVisibility = Visibility.Visible;
                    break;
                }

                return nflag;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("比对发生异常，异常信息：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 摄像头回调函数
        /// </summary>
        /// <param name="bitmap"></param>
        private void CameraHelper_VideoCaptureEvent(Bitmap bitmap)
        {
            App.Current.Dispatcher.Invoke(new Action(() => { TakePhototImage = CommandTools.ImageToBitmapSource(bitmap); }));
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        private void CloseCamera()
        {
            App.Current.Dispatcher.Invoke(new Action(() => { cameraHelper.CloseCamera(); }));
        }

        /// <summary>
        /// 释放比对线程
        /// </summary>
        private void CloseFaceCheckThread()
        {
            try
            {
                if (FaceCheckThread != null) FaceCheckThread.Abort(0);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteError("释放人像比对线程异常，异常信息：" + ex.Message);
            }
        }
    }
}
