using HSZH.BLL;
using HSZH.Common;
using HSZH.Config;
using HSZH.FKTerminal.Commands;
using HSZH.FKTerminal.Common;
using HSZH.Hardware;
using HSZH.Hardware.FaceCheck;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HSZH.FKTerminal.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private System.Windows.Threading.DispatcherTimer ShowTimer = null;
        private Uri _MainTraget = null;
        private string _AppTitle = "";
        private string _FooterMessageInfo1;
        private string _FooterMessageInfo2;
        private string _ServerPhoneNumber1;
        private string _ServerPhoneNumber2;
        private int ShowTimeTag = 0;
        private Thread timeoutThread = null;
        public event Action TimeOutEventAction = null;
        public int TimeOutNumber = 60;

        public Uri MainTraget
        {
            get { return _MainTraget; }
            set { _MainTraget = value; if (value != null) { base.RaisePropertyChanged("MainTraget"); } }
        }

        public string AppTitle
        {
            get { return _AppTitle; }
            set { _AppTitle = value; base.RaisePropertyChanged("AppTitle"); }
        }


        private Visibility _CountdownVisibility = Visibility.Collapsed;
        public Visibility CountdownVisibility
        {
            set { _CountdownVisibility = value; base.RaisePropertyChanged("CountdownVisibility"); }
            get { return _CountdownVisibility; }
        }

        private int _CountdownValue;
        public int CountdownValue
        {
            set { _CountdownValue = value; base.RaisePropertyChanged("CountdownValue"); }
            get { return _CountdownValue; }
        }

        public string FooterMessageInfo1
        {
            set { _FooterMessageInfo1 = value; base.RaisePropertyChanged("FooterMessageInfo1"); }
            get { return _FooterMessageInfo1; }
        }

        public string FooterMessageInfo2
        {
            set { _FooterMessageInfo2 = value; base.RaisePropertyChanged("FooterMessageInfo2"); }
            get { return _FooterMessageInfo2; }
        }

        public string ServerPhoneNumber1
        {
            set { _ServerPhoneNumber1 = value; base.RaisePropertyChanged("ServerPhoneNumber1"); }
            get { return _ServerPhoneNumber1; }
        }

        public string ServerPhoneNumber2
        {
            set { _ServerPhoneNumber2 = value; base.RaisePropertyChanged("ServerPhoneNumber2"); }
            get { return _ServerPhoneNumber2; }
        }

        public string _LoadingWaitMessage;
        public string LoadingWaitMessage
        {
            set { _LoadingWaitMessage = value; base.RaisePropertyChanged("LoadingWaitMessage"); }
            get { return _LoadingWaitMessage; }
        }

        private Visibility _LoadingWaitVisibility = Visibility.Collapsed;
        public Visibility LoadingWaitVisibility
        {
            set { _LoadingWaitVisibility = value; base.RaisePropertyChanged("LoadingWaitVisibility"); }
            get { return _LoadingWaitVisibility; }
        }

        private string _HomeCurrData;
        public string HomeCurrData
        {
            get { return _HomeCurrData; }
            set { _HomeCurrData = value; base.RaisePropertyChanged("HomeCurrData"); }
        }

        private string _HomeCurrTime;
        public string HomeCurrTime
        {
            get { return _HomeCurrTime; }
            set { _HomeCurrTime = value; base.RaisePropertyChanged("HomeCurrTime"); }
        }

        private RelayCommand<object> _DoInAdminManager;
        public RelayCommand<object> DoInAdminManager
        {
            get
            {

                if (_DoInAdminManager == null)
                    _DoInAdminManager = new RelayCommand<object>((obj) => DoInAdminManagerFun(obj));
                return _DoInAdminManager;
            }
        }

        #region 全局静态属性
        private static MainWindowViewModel _Instance;
        public static MainWindowViewModel Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MainWindowViewModel();
                return _Instance;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        {
            SetBackgroupPic();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public override void DoInitFunction(object obj)
        {
            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowTimer_Tick);
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    IsShowHiddenLoadingWait("系统正在初始化,请稍候...");

                    //加载系统配置信息
                    Log.Instance.WriteInfo("加载系统配置信息");
                    if (!AppConfig.Instance.LoadConfig().IsSucceed)
                    {
                        Log.Instance.WriteInfo("加载系统配置信息失败");
                        ExitAppFun("加载系统配置信息失败");
                        return;
                    }

                    //身份证阅读器模块
                    Log.Instance.WriteInfo("初始化身份证阅读器");
                    if (!InitIDCardReader().IsSucceed)
                    {
                        Log.Instance.WriteInfo("初始化身份证阅读器失败");
                        ExitAppFun("初始化身份证阅读器失败");
                        return;
                    }

                    //人像比对引擎模块
                    if (!InitFaceCheckModule().IsSucceed)
                    {
                        Log.Instance.WriteInfo("初始化人像比对引擎失败");
                        ExitAppFun("初始化人像比对引擎失败");
                        return;
                    }

                    //天腾设备模块

                    //获取基础配置模块
                    if (!InitBaseDataModule())
                    {
                        Log.Instance.WriteInfo("初始化基础配置失败");
                        ExitAppFun("初始化基础配置失败");
                        return;
                    }

                    //加载完成
                    IsShowHiddenLoadingWait("初始化成功...");
                    SetFrameTraget("MainPage");
                    IsShowHiddenLoadingWait("", false);
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteInfo($"系统初始化异常：{ex.Message}");
                    ExitAppFun("系统初始化异常");
                    return;
                }
            });
        }

        private ReturnInfo InitIDCardReader()
        {
            if (!AppConfig.Instance.SysConfig.IsConnReadIDcardDev)
                Log.Instance.WriteInfo("未启用身份证阅读器模块");
            return HardwareHelper.Instance.IdCardReader.Initialize(new IdCardInitParam
            {
                ConnType = AppConfig.Instance.SysConfig.ConnIDCardType,
                ComPort = AppConfig.Instance.SysConfig.ReadIDCardPortName
            });
        }

        private ReturnInfo InitFaceCheckModule()
        {
            if (!AppConfig.Instance.SysConfig.IsOpenFaceCheck)
            {
                Log.Instance.WriteInfo("未启用人脸比对模块");
                return new ReturnInfo();
            }
            return FaceCheckHelper.Instance.DoInit(FaceCheckType.HongRuan);
        }

        private bool InitBaseDataModule()
        {
            if (!AppConfig.Instance.SysConfig.ServerPhoneNumber1.IsEmpty())
                ServerPhoneNumber2 = $"咨询电话：{AppConfig.Instance.SysConfig.ServerPhoneNumber1}";
            if (!AppConfig.Instance.SysConfig.ServerPhoneNumber2.IsEmpty())
                ServerPhoneNumber2 = $"故障报修电话：{AppConfig.Instance.SysConfig.ServerPhoneNumber2}";
            AppTitle = AppConfig.Instance.SysConfig.AppTitle;
            return true;
        }

        /// <summary>
        /// 界面跳转
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="topTitle"></param>
        /// <param name="topvisable"></param>
        public void SetFrameTraget(string pageName, string topTitle = "", bool topvisable = false)
        {
            if (pageName.IsEmpty())
                return;
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                this.MainTraget = new Uri(string.Format("/HSZH.FKTerminal;component/Views/{0}.xaml", pageName), UriKind.RelativeOrAbsolute);
            }));
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="MessageInfo"></param>
        /// <param name="nflag"></param>
        public void IsShowHiddenLoadingWait(string MessageInfo = "", bool nflag = true)
        {
            if (!nflag)
            {
                LoadingWaitVisibility = Visibility.Collapsed;
                return;
            }
            if (this.LoadingWaitVisibility != Visibility.Visible)
            {
                LoadingWaitVisibility = Visibility.Visible;
            }
            LoadingWaitMessage = MessageInfo;
        }

        public void OpenTimeOut(int timeoutnumber = 60, bool IsReturnHome = true)
        {
            StopTimeOut();
            if (CountdownVisibility != Visibility.Visible)
                CountdownVisibility = Visibility.Visible;
            CountdownValue = timeoutnumber;
            TimeOutNumber = timeoutnumber;
            timeoutThread = new Thread(new ParameterizedThreadStart(TimeOutFunction));
            timeoutThread.IsBackground = true;
            timeoutThread.Start(IsReturnHome);
        }

        public void StopTimeOut(bool nflag = true)
        {
            if (timeoutThread != null)
            {
                if (nflag)
                {
                    if (TimeOutEventAction != null)
                        TimeOutEventAction = null;
                }
                if (timeoutThread != null)
                {
                    timeoutThread.Abort(0);
                    timeoutThread = null;
                }
                this.CountdownVisibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public void ExitAppFun()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { Application.Current.Shutdown(-1); }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        private void ExitAppFun(string msg, int sleepTime = 3000)
        {
            IsShowHiddenLoadingWait($"{msg}，退出程序！");
            Thread.Sleep(sleepTime);
            ExitAppFun();
        }

        private void TimeOutFunction(object IsReturnHome)
        {
            while (true)
            {
                TimeOutNumber--;
                this.CountdownValue = TimeOutNumber;
                if (TimeOutNumber == 0)
                {
                    if (TimeOutEventAction != null)
                    {
                        TimeOutEventAction();
                        TimeOutEventAction = null;
                    }
                    timeoutThread = null;
                    if (IsReturnHome.ToBool())
                        this.ReturnHome();
                    return;
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 跳转到提示消息界面
        /// </summary>
        /// <param name="MessageInfo"></param>
        /// <param name="IsColse"></param>
        /// <param name="BtnVisibility"></param>
        /// <param name="BtnContent"></param>
        public void SetFrameMessagePage(string MessageInfo, bool IsColse = false, bool BtnVisibility = true, string BtnContent = "取证")
        {
            this.IsShowHiddenLoadingWait("", false);
            MessagePageViewModel.Instance.IsClose = IsColse;
            if (!IsColse)
            {
                if (BtnVisibility)
                    MessagePageViewModel.Instance.ButtonVisibility = Visibility.Visible;
                else
                    MessagePageViewModel.Instance.ButtonVisibility = Visibility.Collapsed;
            }
            else
            {
                MessagePageViewModel.Instance.ButtonVisibility = Visibility.Collapsed;
            }
            MessagePageViewModel.Instance.ButtonContent = BtnContent;
            MessagePageViewModel.Instance.AppMessageInfo = MessageInfo;
            this.SetFrameTraget("MessagePage");
        }

        public void ReturnHome()
        {
            Task.Factory.StartNew(() =>
            {
                SetFrameTraget("BlankPage", "", true);
                //this.IsShowHiddenLoadingWait("页面加载中...", true);
                //Thread.Sleep(300);
                this.IsShowHiddenLoadingWait("", false);
                SetFrameTraget("MainPage", "", false);
            });
        }

        public void CloseShowTime()
        {
            if (ShowTimer != null) ShowTimer.Stop();
        }

        public void SetBackgroupPic(AppRunType appRunType = AppRunType.AppHome)
        {

        }

        public int ExitPageSumCount = 0;
        public int ExitPageFlag = 0;
        public DateTime ExitPageStartTime = DateTime.Now;
        /// <summary>
        /// 进入管理员界面
        /// </summary>
        /// <param name="obj"></param>
        private void DoInAdminManagerFun(object obj)
        {
            int nflag = Convert.ToInt32(obj);

            DateTime newTime = DateTime.Now;
            if (ExitPageStartTime.AddSeconds(2) < newTime)
            {
                if (nflag == 0)
                {
                    ExitPageSumCount = 1;
                    ExitPageFlag = 0;
                    ExitPageStartTime = newTime;
                }
                return;
            }

            if (nflag != ExitPageFlag)
            {
                ExitPageSumCount++;
                ExitPageFlag = nflag;
            }

            if (ExitPageSumCount > 2)
            {
                this.SetFrameTraget("ExitPage");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowTimer_Tick(object sender, EventArgs e)
        {
            HomeCurrData = DateTime.Now.ToString("yyyy年MM月dd日");
            if (ShowTimeTag == 0)
            {
                HomeCurrTime = DateTime.Now.ToString("HH:mm");
                ShowTimeTag = 1;
            }
            else
            {
                HomeCurrTime = DateTime.Now.ToString("HH mm");
                ShowTimeTag = 0;
            }
        }
    }
}
