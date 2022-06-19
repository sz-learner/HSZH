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

namespace HSZH.FKTerminal.Views
{
    /// <summary>
    /// PauseServiceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PauseServiceWindow : Window
    {
        public Action<string> SelectPauseServiceCallBack = null;
        private Dictionary<string, string> pauseServiceReason = new Dictionary<string, string>();
        public PauseServiceWindow()
        {
            InitializeComponent();
            pauseServiceReason.Add("设备故障", "设备故障，暂无法受理，请稍候，不便之处敬请谅解！");
            pauseServiceReason.Add("暂停服务", "设备暂停服务，暂无法受理！");
            pauseServiceReason.Add("网络故障", "网络故障，暂无法受理，请稍候，不便之处敬请谅解！");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                Button button = sender as Button;
                var key = button.Content.ToString();
                SelectPauseServiceCallBack?.Invoke(pauseServiceReason[key]);
                this.DialogResult = true;
            }
        }
    }
}
