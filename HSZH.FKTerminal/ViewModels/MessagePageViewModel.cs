using HSZH.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HSZH.FKTerminal.ViewModels
{
    public class MessagePageViewModel : ObservableObject
    {
        public static MessagePageViewModel Instance
        {
            get
            {
                return SingletonProvider<MessagePageViewModel>.Instance;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        private string _AppMessageInfo;
        /// <summary>
        /// 是否自动关闭
        /// </summary>
        private bool _IsClose = false;
        /// <summary>
        /// 按钮文本
        /// </summary>
        private string _ButtonContent = "取卡";
        /// <summary>
        /// 按钮是否显示
        /// </summary>
        private Visibility _ButtonVisibility = Visibility.Visible;

        /// <summary>
        /// 是否自动关闭
        /// </summary>
        public bool IsClose
        {
            get { return _IsClose; }
            set { _IsClose = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string AppMessageInfo
        {
            get { return _AppMessageInfo; }
            set { _AppMessageInfo = value; base.RaisePropertyChanged("AppMessageInfo"); }
        }

        /// <summary>
        /// 按钮文本
        /// </summary>
        public string ButtonContent
        {
            get { return _ButtonContent; }
            set { _ButtonContent = value; base.RaisePropertyChanged("ButtonContent"); }
        }

        /// <summary>
        /// 按钮是否显示
        /// </summary>
        public Visibility ButtonVisibility
        {
            get { return _ButtonVisibility; }
            set { _ButtonVisibility = value; base.RaisePropertyChanged("ButtonVisibility"); }
        }

        public override void DoInitFunction(object obj)
        {
            if (this.IsClose)
            {
                MainWindowViewModel.Instance.TimeOutEventAction += Instance_TimeOutEventAction;
                MainWindowViewModel.Instance.OpenTimeOut(5);
            }
        }

        private void Instance_TimeOutEventAction()
        {

        }

        public override void DoNextFunction(object obj)
        {
            if (this.IsClose)
                MainWindowViewModel.Instance.StopTimeOut();
            MainWindowViewModel.Instance.ReturnHome();
        }
    }
}
