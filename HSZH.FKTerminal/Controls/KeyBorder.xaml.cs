using HSZH.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HSZH.FKTerminal.Controls
{
    /// <summary>
    /// KeyBorder.xaml 的交互逻辑
    /// </summary>
    public partial class KeyBorder : UserControl
    {
        public KeyBorder()
        {
            InitializeComponent();
        }

        private static bool _ShiftFlag;

        protected static bool ShiftFlag
        {
            get { return _ShiftFlag; }
            set { _ShiftFlag = value; }
        }
        private static bool _CtrlFlag;
        protected static bool CtrlFlag
        {
            get { return _CtrlFlag; }
            set { _CtrlFlag = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button keybtn = sender as System.Windows.Controls.Button;
            #region//First Row
            if (keybtn.Name == "CmdTlide")
            {
                addNumkeyINput(0xc0);
            }
            else if (keybtn.Name == "cmd1" || keybtn.Name == "Scmd1")
            {
                addNumkeyINput(0x31);
            }
            else if (keybtn.Name == "cmd2" || keybtn.Name == "Scmd2")
            {
                addNumkeyINput(0x32);
            }
            else if (keybtn.Name == "cmd3" || keybtn.Name == "Scmd3")
            {
                addNumkeyINput(0x33);
            }
            else if (keybtn.Name == "cmd4" || keybtn.Name == "Scmd4")
            {
                addNumkeyINput(0x34);
            }
            else if (keybtn.Name == "cmd5" || keybtn.Name == "Scmd5")
            {
                addNumkeyINput(0x35);
            }
            else if (keybtn.Name == "cmd6" || keybtn.Name == "Scmd6")
            {
                addNumkeyINput(0x36);

            }
            else if (keybtn.Name == "cmd7" || keybtn.Name == "Scmd7")
            {
                addNumkeyINput(0x37);
            }
            else if (keybtn.Name == "cmd8" || keybtn.Name == "Scmd8")
            {
                addNumkeyINput(0x38);
            }
            else if (keybtn.Name == "cmd9" || keybtn.Name == "Scmd9")
            {
                addNumkeyINput(0x39);
            }
            else if (keybtn.Name == "cmd0" || keybtn.Name == "Scmd0")
            {
                addNumkeyINput(0x30);

            }
            else if (keybtn.Name == "cmdminus")//-_
            {
                addNumkeyINput(0xbd);
            }
            else if (keybtn.Name == "cmdplus")//+=
            {
                addNumkeyINput(0xbb);
            }
            else if (keybtn.Name == "cmdBackspace" || keybtn.Name == "btnBack")//backspace
            {
                AddKeyBoardINput(0x08);
            }
            #endregion
            #region//Second Row
            else if (keybtn.Name == "CmdTab")
            {
                AddKeyBoardINput(0x09);
            }
            else if (keybtn.Name == "CmdQ")
            {
                AddKeyBoardINput(0x51);
            }
            else if (keybtn.Name == "Cmdw")
            {
                AddKeyBoardINput(0x57);

            }
            else if (keybtn.Name == "CmdE")
            {
                AddKeyBoardINput(0X45);

            }
            else if (keybtn.Name == "CmdR")
            {
                AddKeyBoardINput(0X52);

            }
            else if (keybtn.Name == "CmdT")
            {
                AddKeyBoardINput(0X54);

            }
            else if (keybtn.Name == "CmdY")
            {
                AddKeyBoardINput(0X59);

            }
            else if (keybtn.Name == "CmdU")
            {
                AddKeyBoardINput(0X55);

            }
            else if (keybtn.Name == "CmdI")
            {
                AddKeyBoardINput(0X49);

            }
            else if (keybtn.Name == "CmdO")
            {
                AddKeyBoardINput(0X4F);
            }
            else if (keybtn.Name == "CmdP")
            {
                AddKeyBoardINput(0X50);
            }
            else if (keybtn.Name == "CmdOpenCrulyBrace")
            {
                addNumkeyINput(0xdb);
            }
            else if (keybtn.Name == "CmdEndCrultBrace")
            {
                addNumkeyINput(0xdd);
            }
            else if (keybtn.Name == "CmdOR")
            {
                addNumkeyINput(0xdc);
            }
            #endregion
            #region///Third ROw

            else if (keybtn.Name == "CmdCapsLock")
            {
                AddKeyBoardINput(0x14);
                if (checkImage.Visibility != Visibility.Visible)
                {
                    checkImage.Visibility = Visibility.Visible;
                }
                else
                {
                    checkImage.Visibility = Visibility.Hidden;
                }
            }
            else if (keybtn.Name == "CmdA")
            {
                AddKeyBoardINput(0x41);
            }
            else if (keybtn.Name == "CmdS")
            {
                AddKeyBoardINput(0x53);
            }
            else if (keybtn.Name == "CmdD")
            {
                AddKeyBoardINput(0x44);
            }
            else if (keybtn.Name == "CmdF")
            {
                AddKeyBoardINput(0x46);
            }
            else if (keybtn.Name == "CmdG")
            {
                AddKeyBoardINput(0x47);
            }
            else if (keybtn.Name == "CmdH")
            {
                AddKeyBoardINput(0x48);
            }
            else if (keybtn.Name == "CmdJ")
            {
                AddKeyBoardINput(0x4A);
            }
            else if (keybtn.Name == "CmdK")
            {
                AddKeyBoardINput(0X4B);
            }
            else if (keybtn.Name == "CmdL")
            {
                AddKeyBoardINput(0X4C);

            }
            else if (keybtn.Name == "CmdColon")//;:
            {
                addNumkeyINput(0xba);
            }
            else if (keybtn.Name == "CmdDoubleInvertedComma")//'"
            {
                addNumkeyINput(0xde);
            }
            else if (keybtn.Name == "CmdEnter")
            {
                AddKeyBoardINput(0x0d);
            }
            #endregion
            #region//Fourth Row
            else if (keybtn.Name == "CmdShift" || keybtn.Name == "CmdlShift")
            {
                if (CtrlFlag)
                {
                    CtrlFlag = false;
                    ShiftFlag = false;
                    changeInput();
                }
                else
                {
                    ShiftFlag = true;
                }
            }
            else if (keybtn.Name == "CmdZ")
            {

                AddKeyBoardINput(0X5A);

            }
            else if (keybtn.Name == "CmdX")
            {
                AddKeyBoardINput(0X58);

            }
            else if (keybtn.Name == "CmdC")
            {
                AddKeyBoardINput(0X43);

            }
            else if (keybtn.Name == "CmdV")
            {
                AddKeyBoardINput(0X56);

            }
            else if (keybtn.Name == "CmdB")
            {
                AddKeyBoardINput(0X42);

            }
            else if (keybtn.Name == "CmdN")
            {
                AddKeyBoardINput(0x4E);

            }
            else if (keybtn.Name == "CmdM")
            {
                AddKeyBoardINput(0x4D);
            }
            else if (keybtn.Name == "CmdLessThan")//<,
            {
                addNumkeyINput(0xbc);
            }
            else if (keybtn.Name == "CmdGreaterThan")//>.
            {
                addNumkeyINput(0xbe);
            }
            else if (keybtn.Name == "CmdQuestion")//?/
            {
                addNumkeyINput(0xbf);
            }

            else if (keybtn.Name == "CmdSpaceBar")
            {
                AddKeyBoardINput(0x20);
            }
            #endregion
            #region//Last row
            else if (keybtn.Name == "CmdCtrl" || keybtn.Name == "CmdlCtrl")//ctrl
            {
                if (ShiftFlag)
                {
                    ShiftFlag = false;
                    CtrlFlag = false;
                }
                else
                {
                    CtrlFlag = true;
                }
            }
            else if (keybtn.Name == "CmdpageUp")
            {
                AddKeyBoardINput(0x21);
            }
            else if (keybtn.Name == "CmdpageDown")
            {
                AddKeyBoardINput(0x22);
            }
            else if (keybtn.Name == "CmdClose")//关闭键盘
            {
                KeyBorderPanel.Visibility = Visibility.Hidden;
                //this.Close();
            }
            #endregion
        }

        private void changeInput()
        {
            Win32API.keybd_event(0x11, 0, 0, 0);
            Win32API.keybd_event(0x10, 0, 0, 0);
            Win32API.keybd_event(0x11, 0, 0x02, 0);
            Win32API.keybd_event(0x10, 0, 0x02, 0);
        }

        private static void addNumkeyINput(byte input)
        {
            if (CtrlFlag)
            {
                CtrlFlag = false;
                ShiftFlag = false;
                Win32API.keybd_event(input, 0, 0, 0);
                Win32API.keybd_event(input, 0, 0x02, 0);
            }
            else
            {
                if (!ShiftFlag)
                {
                    Win32API.keybd_event(input, 0, 0, 0);
                    Win32API.keybd_event(input, 0, 0x02, 0);
                }
                else
                {
                    Win32API.keybd_event(0x10, 0, 0, 0);//shift
                    Win32API.keybd_event(input, 0, 0, 0);
                    Win32API.keybd_event(input, 0, 0x02, 0);
                    Win32API.keybd_event(0x10, 0, 0x02, 0);
                    ShiftFlag = false;
                }
            }
        }
        private static void AddKeyBoardINput(byte input)
        {

            if (CtrlFlag)
            {
                CtrlFlag = false;
                ShiftFlag = false;
                Win32API.keybd_event(input, 0, 0, 0);
                Win32API.keybd_event(input, 0, 0x02, 0);
            }
            else
            {
                if (ShiftFlag)
                {
                    Win32API.keybd_event(input, 0, 0, 0);
                    Win32API.keybd_event(input, 0, 0x02, 0);
                    ShiftFlag = false;
                }
                else
                {
                    Win32API.keybd_event(input, 0, 0, 0);
                    Win32API.keybd_event(input, 0, 0x02, 0);
                }
            }

        }

        public bool CapsLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                Win32API.GetKeyboardState(bs);
                return (bs[0x14] == 1);
            }
        }
    }
}
