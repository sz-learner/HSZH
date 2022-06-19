using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HSZH.FKTerminal.Views
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        private DispatcherTimer timer = null;
        private int ExitTimeOut = 3;

        public MessageWindow(bool IsClose = false, int timeout = 10)
        {
            InitializeComponent();
            if (IsClose)
            {
                TimeOutPanel.Visibility = Visibility.Visible;
                ExitTimeOut = timeout;
                txtTimeOutMessage.Text = ExitTimeOut.ToString();
                timer = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 0, 0, 1000)
                };
                timer.Tick += Timer_Tick; ;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (ExitTimeOut <= 0)
            {
                timer.Stop();
                this.DialogResult = true;
            }
            else
            {
                ExitTimeOut--;
                txtTimeOutMessage.Text = ExitTimeOut.ToString();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
                timer.Stop();
            this.DialogResult = false;
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
            }
            this.DialogResult = true;
            this.Close();
        }
    }

    public partial class Msg
    {
        public static void Show(string messageInfo, bool IsClose = true)
        {
            MessageWindow window = new MessageWindow(IsClose);
            window.txtMessageInfo.Text = messageInfo;
            window.ShowDialog();
        }

        public static bool ShowDialog(string MsgInfo, bool IsClose = true)
        {
            bool result = false;
            MessageWindow msg = new MessageWindow(IsClose);
            msg.btnCancel.Visibility = System.Windows.Visibility.Visible;
            msg.txtMessageInfo.Text = MsgInfo;
            result = msg.ShowDialog() == true ? true : false;
            return result;
        }
    }
}
