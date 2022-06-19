using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Models
{
	[Serializable]
	public partial class IdCardInfo
	{
		public IdCardInfo()
		{

		}

		public IdCardInfo(int i)
		{
			IDCardNo = "430681199101253210";
			FullName = "张三";
			Gender = "男";
			Birthday = "1991-01-25";
			Nation = "汨罗派出所";
			Address = "广东深圳";
			Zjyxq = "2029-01-01";
			Zjqfrq = "2009-01-01";
			pNational = "汉";
			IsFingerPrintData = false;
			FingerData = new List<FingerPrintData>() { new FingerPrintData(1) };
		}

		/// <summary>
		/// 姓名
		/// </summary>
		public string FullName { get; set; }
		/// <summary>
		/// 身份证号码
		/// </summary>
		public string IDCardNo { get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		public string Gender { get; set; }
		/// <summary>
		/// 出生日期
		/// </summary>
		public string Birthday { get; set; }
		/// <summary>
		/// 派出所名称
		/// </summary>
		public string Nation { get; set; }
		/// <summary>
		/// 名族
		/// </summary>
		public string pNational { get; set; }
		/// <summary>
		/// 住址
		/// </summary>
		public string Address { get; set; }
		/// <summary>
		/// 相片地址
		/// </summary>
		public string ImageUrl { get; set; }
		/// <summary>
		/// 证件有效期
		/// </summary>
		public string Zjyxq { get; set; }
		/// <summary>
		/// 证件签发日期
		/// </summary>
		public string Zjqfrq { get; set; }
		/// <summary>
		/// 是否16周岁以下
		/// </summary>
		public bool IsUnder16Age { get; set; }
		/// <summary>
		/// 是否有指纹信息
		/// </summary>
		public bool IsFingerPrintData { get; set; }
		/// <summary>
		/// 指纹数据
		/// </summary>
		public List<FingerPrintData> FingerData { get; set; }
		public string Zwx { get; set; }
		public string Zwm { get; set; }
		public string Pyx { get; set; }
		public string Pym { get; set; }
		public string hkszd { get; set; }
		/// <summary>
		/// 预留参数
		/// </summary>
		public object Tag { get; set; }

	}
}
