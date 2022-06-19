using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Hardware
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SC_CALIBDATA
    {
        public int bCalibStatus;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2576)]
        public ushort[] R_Gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2576)]
        public ushort[] G_Gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2576)]
        public ushort[] B_Gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2576)]
        public ushort[] IR_Gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2576)]
        public ushort[] offset;
    }
    public partial class IDCardDLLImport
    {
        private const string fprdllPaht = @"EDCDMM\300E\ID_Fpr.dll";
        private const string fprcapPaht = @"EDCDMM\300E\ID_FprCap.dll";

        #region 二代证接口 update wl  20190522

        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "CVR_InitComm", CharSet = CharSet.Auto, SetLastError = false)]
        //public static extern int CVR_InitComm(int Port);//声明外部的标准动态库, 跟Win32API是一样的
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "CVR_Authenticate", CharSet = CharSet.Auto, SetLastError = false)]
        //public static extern int CVR_Authenticate();
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "CVR_Read_Content", CharSet = CharSet.Auto, SetLastError = false)]
        //public static extern int CVR_Read_Content(int Active);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "CVR_Read_FPContent", CharSet = CharSet.Auto, SetLastError = false)]
        //public static extern int CVR_Read_FPContent();
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "CVR_CloseComm", CharSet = CharSet.Auto, SetLastError = false)]
        //public static extern int CVR_CloseComm();
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetPeopleName", CharSet = CharSet.Ansi, SetLastError = false)]
        //public static extern int GetPeopleName(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetPeopleNation", CharSet = CharSet.Ansi, SetLastError = false)]
        //public static extern int GetPeopleNation(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetPeopleBirthday", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetPeopleBirthday(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetPeopleAddress", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetPeopleAddress(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetPeopleIDCode", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetPeopleIDCode(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetDepartment", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetDepartment(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetStartDate", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetStartDate(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetEndDate", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetEndDate(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetPeopleSex", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetPeopleSex(ref byte strTmp, ref int strLen);
        [DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetFPDate", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetFPDate(ref byte strTmp, ref int strLen);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "CVR_GetSAMID", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int CVR_GetSAMID(ref byte strTmp);
        //[DllImport(@"EDCDMM\termb.dll", EntryPoint = "GetManuID", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        //public static extern int GetManuID(ref byte strTmp);

        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_InitComm", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_InitComm(int Port);//声明外部的标准动态库, 跟Win32API是一样的


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_Authenticate", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_Authenticate();


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_Read_Content", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_Read_Content(int Active);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_Read_FPContent", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_Read_FPContent();

        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_FindCard", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_FindCard();

        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_SelectCard", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_SelectCard();


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_CloseComm", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_CloseComm();

        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetCertType", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int GetCertType(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetPeopleName", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int GetPeopleName(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetPeopleChineseName", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int GetPeopleChineseName(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetPeopleNation", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int GetPeopleNation(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetNationCode", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int GetNationCode(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetPeopleBirthday", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleBirthday(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetPeopleAddress", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleAddress(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetPeopleIDCode", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleIDCode(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetDepartment", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetDepartment(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetStartDate", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetStartDate(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetEndDate", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetEndDate(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetPeopleSex", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleSex(ref byte strTmp, ref int strLen);


        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "CVR_GetSAMID", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CVR_GetSAMID(ref byte strTmp);

        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetBMPData", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetBMPData(ref byte btBmp, ref int nLen);

        [DllImport(@"EDCDMM\Termb.dll", EntryPoint = "GetJpgData", CharSet = CharSet.Ansi, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetJpgData(ref byte btBmp, ref int nLen);

        #endregion

        #region 指纹仪函数接口

        #region 鸿达指纹仪新增加接口
        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_SetVideoWindow")]
        public static extern int SetVideoWindow(IntPtr nPtr);
        [DllImport(@"EDCDMM\ImageQuality.dll", EntryPoint = "GetFingerImgQuality")]
        public static extern int GetFingerImgQuality(byte[] fingerImage, int width, int height);
        [DllImport(@"EDCDMM\FPR.dll")]
        public static extern int FeatureExtractMem(byte[] pcImageData, int nDataLen, byte[] pcTemplateData);
        [DllImport(@"EDCDMM\FPR.dll")]
        public static extern int FeatureMatchMem(byte[] pcTemplateDataA, byte[] pcTemplateDataB, ref float pfSim);
        [DllImport(@"EDCDMM\FirWSQLibDll.dll")]
        public static extern int Bmp2Wsq40KMem(byte[] data, int dataLen, byte[] outData, int outLen, out int result);
        [DllImport(@"EDCDMM\FirWSQLibDll.dll")]
        public static extern int Bmp2WsqMem(byte[] data, int dataLen, byte[] outData, int outLen, out int result);
        [DllImport(@"EDCDMM\MLFDLL.dll", EntryPoint = "_FingerDataDecryption@24")]
        internal static extern int DecryptionData(byte[] cKeySeed, uint ulKeySeedLen, byte[] cDataIn, uint ulDataLen, byte[] cOut, out uint ulOutLen);
        [DllImport(@"EDCDMM\MLFDLL.dll", EntryPoint = "_FingerDataEncryption@24")]
        internal static extern int EncryptionData(byte[] cKeySeed, uint ulKeySeedLen, byte[] cDataIn, uint ulDataLen, byte[] cOut, out uint ulOutLen);
        #endregion

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_Init", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_Init();

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_InitEx", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_InitEx(int index);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_Close", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_Close();

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetChannelCount", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetChannelCount();

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetMaxImageSize", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetMaxImageSize(int nChannel, ref int pnWidth, ref int pnHeight);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetBright", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetBright(int nChannel, ref int pnBright);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_SetBright", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_SetBright(int nChannel, int nBright);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetContrast", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetContrast(int nChannel, ref int pnContrast);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_SetContrast", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_SetContrast(int nChannel, int nContrast);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetCaptWindow", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetCaptWindow(int nChannel, ref int pnOriginX, ref int pnOriginY, ref int pnWidth, ref int pnHeight);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_SetCaptWindow", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_SetCaptWindow(int nChannel, int nOriginX, int nOriginY, int nWidth, int nHeight);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_BeginCapture", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_BeginCapture(int nChannel);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_EndCapture", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_EndCapture(int nChannel);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetFPRawData", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetFPRawData(int nChannel, System.IntPtr pRawData);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetFPBmpData", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetFPBmpData(int nChannel, byte[] pBmpData);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_Setup", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_Setup();

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_IsSupportSetup", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_IsSupportSetup();

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetVersion", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetVersion();

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetDesc", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetDesc(System.IntPtr pszDesc);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_SetBufferEmpty", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_SetBufferEmpty(System.IntPtr pImageData, long imageLength);

        [DllImport(fprcapPaht, EntryPoint = "LIVESCAN_GetErrorInfo", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int LIVESCAN_GetErrorInfo(int nErrorNo, System.IntPtr pszErrorInfo);

        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(ref long count);

        #endregion

        #region 指纹特征提取函数接口

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FP_Begin();

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_GetVersion(System.IntPtr code);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_FeatureExtract(byte cScannerType, byte cFingerCode, System.IntPtr pFingerImgBuf, System.IntPtr pFeatureData);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_FeatureMatch(System.IntPtr pFeatureData1, System.IntPtr pFeatureData2, ref float pfSimilarity);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_ImageMatch(System.IntPtr pFingerImgBuf, System.IntPtr pFeatureData, ref float pfSimilarity);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_Compress(byte cScannerType, byte cEnrolResult, byte cFingerCode, System.IntPtr pFingerImgBuf,
                                             int nCompressRatio, System.IntPtr pCompressedImgBuf, byte[] strBuf);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_Decompress(System.IntPtr pCompressedImgBuf, System.IntPtr pFingerImgBuf, byte[] strBuf);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_GetQualityScore(System.IntPtr pFingerImgBuf, ref int pnScore);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_GenFeatureFromEmpty1(byte cScannerType, byte cFingerCode, System.IntPtr pFeatureData);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_Decompress(byte cFingerCode, System.IntPtr pFeatureData);

        [DllImport(fprdllPaht, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int FP_End();

        #endregion

        #region 身份证收卡
        [DllImport(@"EDCDMM\CVRScand.dll", EntryPoint = "CVR_InitDevice", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_InitDevice();
        [DllImport(@"EDCDMM\CVRScand.dll", EntryPoint = "CVR_GetDeviceStatus", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_GetDeviceStatus();
        [DllImport(@"EDCDMM\CVRScand.dll", EntryPoint = "CVR_Getcardstatue", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_Getcardstatue();
        [DllImport(@"EDCDMM\CVRScand.dll", EntryPoint = "CVR_ScanAndOCR", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_ScanAndOCR(ref byte strTmp);
        [DllImport(@"EDCDMM\CVRScand.dll", EntryPoint = "CVR_EjectCard", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_EjectCard();
        [DllImport(@"EDCDMM\CVRScand.dll", EntryPoint = "CVR_CloseDevice", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int CVR_CloseDevice();
        #endregion

        #region 收卡模块接口
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="IVS_600DS"></param>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern Boolean IO_HasScanner(String IVS_600DS);
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern Boolean IO_CloseDevice();
        //校准
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_StartCalib();
        /// <summary>
        /// 使其工作
        /// </summary>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_StartSuckCard();
        /// <summary>
        /// 禁止工作
        /// </summary>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_CancelSuckCard();

        /// <summary>
        /// 设置扫描完成后，退卡或卡还是留在设备内。如果未定义默认退卡。0 = 退卡；1 = 卡留在设备内,其它 = 未定义。
        /// </summary>
        /// <param name="lstatus"></param>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_SetAfterScan(int lstatus);

        /// <summary>
        /// 获取卡状态
        /// </summary>
        /// <param name="status"></param>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_GetCardStatus(ref int status);

        /// <summary>
        ///  报告设备当前状态
        ///  0：准备好
        ///  1：繁忙
        ///  2：正在校准
        ///  3：反向转动
        ///  4：正在扫描
        ///  5：正在测试（调试）马达
        ///  6：内部通读错误，如没有得到扫描图像。
        ///  7：卡在设备内.
        ///  8：马达正在转动
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_GetDeviceStatus(ref int status);

        /// <summary>
        /// 读取校准数据。从下位机读取校准数据到上位机或从上位机校准文件中读取校准数据，校准数据是扫描图像必须的；返回参数说明下位机内是否已有校准数据，该函数在每次打开设备后或扫描前是必须调用的，详见推荐流程。
        /// </summary>
        /// <param name="pCalibData"></param>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_GetCalibData(ref SC_CALIBDATA pCalibData);

        /// <summary>
        /// 吞卡
        /// </summary>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_ConfiscateCard();

        /// <summary>
        /// 退卡
        /// </summary>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_EjectCard();

        /// <summary>
        /// 扫描并OCR
        /// </summary>
        /// <param name="m_cFileName1"></param>
        /// <param name="m_cFileName2"></param>
        /// <param name="scanMode"></param>
        /// <param name="pName"></param>
        /// <param name="pSex"></param>
        /// <param name="pNational"></param>
        /// <param name="pBorn"></param>
        /// <param name="pHome"></param>
        /// <param name="pCode"></param>
        /// <param name="pRegOrg"></param>
        /// <param name="pSigndate"></param>
        /// <param name="pValidate"></param>
        /// <returns></returns>
        [DllImport(@"EDCDMM\a8_dblNew.dll")]
        public static extern int IO_ScanAndOCR(String m_cFileName1, String m_cFileName2, int scanMode, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pName, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pSex, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pNational, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pBorn, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pHome, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pCode, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pRegOrg, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pSigndate, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pValidate);
        #endregion

        #region 切换版
        [DllImport(@"EDCDMM\HIDSwitchSDT.dll", EntryPoint = "Switch", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int Switch(int nDevIndex);  //0为第一个指纹仪，1为第二个指纹仪
        #endregion
    }
}
