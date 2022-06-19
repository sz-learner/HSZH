using HSZH.Config;
using HSZH.FKTerminal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HSZH.FKTerminal.ViewModels
{
    public class ExitPageViewModel : ObservableObject
    {
        private PasswordBox passwordBox = null;

        public override void DoInitFunction(object obj)
        {
            MainWindowViewModel.Instance.OpenTimeOut();
            passwordBox = (PasswordBox)obj;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            passwordBox.Focus();
        }

        public override void DoExitFunction(object obj)
        {
            MainWindowViewModel.Instance.StopTimeOut();
            MainWindowViewModel.Instance.ReturnHome();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                return;
            }
            if (passwordBox.Password.Equals(AppConfig.Instance.SysConfig.AppExitPassWord))
            {
                MainWindowViewModel.Instance.StopTimeOut();
                MainWindowViewModel.Instance.SetFrameTraget("AdminManager");
                return;
            }
            if (passwordBox.Password.Length > AppConfig.Instance.SysConfig.AppExitPassWord.Length)
            {
                Msg.Show("密码错误，请重新输入！");
                passwordBox.Password = string.Empty;
            }
        }
    }
}
