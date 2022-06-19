using HSZH.Common;
using HSZH.Interface;
using HSZH.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware.IDCard
{
    public class CVR_IDCard : IIDCardReader
    {
        private bool IsInit;
        private string path = FileHelper.GetLocalPath() + "\\EDCDMM\\";
        private IdCardInitParam param = null;

        public ReturnInfo Initialize(IdCardInitParam param)
        {
            this.param = param;
            return DoReadIDCardInit();
        }

        public ReturnInfo DoReadIDCardInit()
        {
            try
            {
                int iPort, iRetUSB = 0, iRetCOM = 0;
                if (param.ConnType == ReadIdCardType.COM)
                {
                    iRetCOM = IDCardDLLImport.CVR_InitComm(param.ComPort);
                    if (iRetCOM == 1)
                    {
                        IsInit = true;
                        Log.Instance.WriteDebug("串口读卡器初始化成功");
                    }
                }
                else if (param.ConnType == ReadIdCardType.USB)
                {
                    for (iPort = 1001; iPort <= 1010; iPort++)
                    {
                        iRetUSB = IDCardDLLImport.CVR_InitComm(iPort);
                        if (iRetUSB == 1)
                        {
                            IsInit = true;
                            Log.Instance.WriteDebug("USB读卡器初始化成功");
                            break;
                        }
                    }
                }
                if ((iRetCOM == 1) || (iRetUSB == 1))
                {
                    return ReturnInfo.CreateSuccess("初始化身份证读卡器成功");
                }
                else
                {
                    return ReturnInfo.CreateError("初始化身份证读卡器失败");
                }
            }
            catch (Exception ex)
            {
                return ReturnInfo.CreateException(ex);
            }
        }

        public ReturnInfo DoReadIDCardInfo(out IdCardInfo model)
        {
            try
            {
                if (!IsInit)
                {
                    var rinfo = DoReadIDCardInit();
                    if (!rinfo.IsSucceed)
                    {
                        model = null;
                        return rinfo;
                    }
                }
                model = null;
                if (IDCardDLLImport.CVR_Authenticate() == 1)
                {
                    Log.Instance.WriteInfo("身份证认证成功，开始读卡");
                    int readContent = IDCardDLLImport.CVR_Read_FPContent();
                    if (readContent == 1)
                    {
                        Log.Instance.WriteInfo("读卡成功！");
                        model = new IdCardInfo();
                        byte[] imgData = new byte[40960];
                        int length = 40960;

                        try
                        {
                            //update add wl 20190521
                            IDCardDLLImport.GetJpgData(ref imgData[0], ref length);
                            MemoryStream myStream = new MemoryStream();
                            for (int i = 0; i < length; i++)
                            {
                                myStream.WriteByte(imgData[i]);
                            }
                            Image myImage = Image.FromStream(myStream);
                            //将图片保存根目录
                            Log.Instance.WriteInfo("开始保存图片！");
                            string filename = path + "zp." + System.Drawing.Imaging.ImageFormat.Jpeg;
                            Log.Instance.WriteInfo("图片路径！" + filename);
                            myImage.Save(filename);
                            myStream.Close();
                            model.ImageUrl = filename;
                        }
                        catch (Exception)
                        {
                            model.ImageUrl = Path.Combine(FileHelper.GetLocalPath(), @"EDCDMM\zp.bmp");
                        }
                        //update add wl 20190521
                        // 
                        byte[] name = new byte[30];
                        //update add wl 20190521
                        // int length = 30;
                        length = 30;
                        IDCardDLLImport.GetPeopleName(ref name[0], ref length);
                        byte[] number = new byte[36];
                        length = 36;
                        IDCardDLLImport.GetPeopleIDCode(ref number[0], ref length);
                        byte[] people = new byte[4];
                        length = 4;
                        IDCardDLLImport.GetPeopleNation(ref people[0], ref length);
                        byte[] validtermOfStart = new byte[30];
                        length = 16;
                        IDCardDLLImport.GetStartDate(ref validtermOfStart[0], ref length);
                        byte[] birthday = new byte[30];
                        length = 16;
                        IDCardDLLImport.GetPeopleBirthday(ref birthday[0], ref length);
                        byte[] address = new byte[70];
                        length = 70;
                        IDCardDLLImport.GetPeopleAddress(ref address[0], ref length);
                        byte[] validtermOfEnd = new byte[30];
                        length = 16;
                        IDCardDLLImport.GetEndDate(ref validtermOfEnd[0], ref length);
                        byte[] signdate = new byte[30];
                        length = 30;
                        IDCardDLLImport.GetDepartment(ref signdate[0], ref length);
                        byte[] sex = new byte[30];
                        length = 3;
                        IDCardDLLImport.GetPeopleSex(ref sex[0], ref length);
                        byte[] samid = new byte[32];
                        IDCardDLLImport.CVR_GetSAMID(ref samid[0]);
                        byte[] ICFeatureData = new byte[1024];
                        //提取身份证中的指纹信息(1024 字节，两个指纹)
                        IDCardDLLImport.GetFPDate(ref ICFeatureData[0], ref length);
                        model.FullName = System.Text.Encoding.GetEncoding("GB2312").GetString(name).Replace("\0", "").Trim();
                        model.Gender = System.Text.Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "");
                        //这里赋值错误 将民族的赋值给派出所更正一下update wl 20190520 
                        model.Nation = System.Text.Encoding.GetEncoding("GB2312").GetString(signdate).Replace("\0", "");
                        model.Birthday = System.Text.Encoding.GetEncoding("GB2312").GetString(birthday).Replace("\0", "");
                        try
                        {
                            if (model.Birthday.Length != 8)
                            {
                                model.Birthday = model.Birthday.Replace("年", "").Replace("月", "").Replace("日", "").Replace("-", "");
                            }
                        }
                        catch
                        {

                        }
                        model.Address = System.Text.Encoding.GetEncoding("GB2312").GetString(address).Replace("\0", "");
                        model.IDCardNo = System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "");
                        model.IsUnder16Age = CheckeAge(model.IDCardNo);
                        //这里赋值错误 将派出所的赋值给民族更正一下update wl 20190520 
                        model.pNational = System.Text.Encoding.GetEncoding("GB2312").GetString(people).Replace("\0", "");
                        model.Zjyxq = System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfEnd).Replace("\0", "");
                        model.Zjqfrq = System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfStart).Replace("\0", "");
                        try
                        {
                            if (length == 0)
                            {
                                model.IsFingerPrintData = false;
                                model.FingerData = null;
                            }
                            else
                            {
                                if (ICFeatureData[4] == 1 || ICFeatureData[516] == 1)
                                {
                                    model.IsFingerPrintData = true;
                                    model.FingerData = new List<FingerPrintData>();
                                    FingerPrintData mode = null;
                                    if (ICFeatureData[4] == 1)
                                    {
                                        mode = new FingerPrintData();
                                        mode.FingerPrintNumber = ICFeatureData[5];
                                        mode.FingerPrintName = getFPcode(ICFeatureData[5]);
                                        mode.FingerPrintQuality = ICFeatureData[6];
                                        mode.FingerPrintdata = new byte[512];
                                        Array.Copy(ICFeatureData, 0, mode.FingerPrintdata, 0, 512);
                                        model.FingerData.Add(mode);
                                    }
                                    if (ICFeatureData[516] == 1)
                                    {
                                        mode = new FingerPrintData();
                                        mode.FingerPrintNumber = ICFeatureData[517];
                                        mode.FingerPrintName = getFPcode(ICFeatureData[517]);
                                        mode.FingerPrintQuality = ICFeatureData[518];
                                        mode.FingerPrintdata = new byte[512];
                                        Array.Copy(ICFeatureData, 512, mode.FingerPrintdata, 0, 512);
                                        model.FingerData.Add(mode);
                                    }
                                }
                                else
                                {
                                    model.IsFingerPrintData = false;
                                    model.FingerData = null;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Instance.WriteError(ex.ToString());
                            model.IsFingerPrintData = false;
                            model.FingerData = null;
                        }
                        Log.Instance.WriteInfo(string.Format("身份证读取成功，姓名：{0} 身份证号：{1}", model.FullName, model.IDCardNo));
                        return ReturnInfo.CreateSuccess("读取身份证信息成功");
                    }
                    else
                    {
                        Log.Instance.WriteInfo("身份证读取内容失败！");
                        return ReturnInfo.CreateError("读取内容失败");
                    }
                }
                else
                {
                    Log.Instance.WriteInfo("身份证认证失败！");
                    return ReturnInfo.CreateError("身份证认证失败");
                }
            }
            catch (Exception ex)
            {
                model = null;
                return ReturnInfo.CreateException(ex);
            }
        }

        #region 获取指位名称
        /// <summary>
        /// 获取指位名称
        /// </summary>
        /// <param name="FPcode"></param>
        /// <returns></returns>
        private string getFPcode(int FPcode)
        {
            switch (FPcode)
            {
                case 11: return "右手拇指";
                case 12: return "右手食指";
                case 13: return "右手中指";
                case 14: return "右手无名指";
                case 15: return "右手小拇指";
                case 16: return "左手拇指";
                case 17: return "左手食指";
                case 18: return "左手中指";
                case 19: return "左手无名指";
                case 20: return "左手小拇指";
                case 97: return "右手不确定指位";
                case 98: return "左手不确定指位";
                case 99: return "其他不确定指位";
                default: return "未知";
            }
        }

        public void Dispose()
        {
            IDCardDLLImport.CVR_CloseComm();
        }
        #endregion

        #region 关闭身份证阅读器

        /// <summary>
        /// 关闭身份证阅读器
        /// </summary>

        public ReturnInfo DoCloseIDCard()
        {
            try
            {
                IDCardDLLImport.CVR_CloseComm();
                IsInit = false;
            }
            catch (Exception ex)
            {
                return ReturnInfo.CreateException(ex);
            }
            return ReturnInfo.CreateSuccess();
        }

        #endregion

        /// <summary>
        /// 根据身份证号检测年龄
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        private bool CheckeAge(string cardNo)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(cardNo))
            {
                string str = cardNo.Substring(6, 8);
                DateTime time = DateTime.Parse(str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2));
                num = ((int)DateTime.Now.Subtract(time).TotalDays) / 0x16d;
                if ((num >= 0x10) || (num < 0))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
