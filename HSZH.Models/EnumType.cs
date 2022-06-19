using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Models
{
    /// <summary>
    /// 身份证阅读器类型
    /// </summary>
    public enum ReadIdCardType
    {
        USB = 0,
        COM = 1,
    }

    public enum CameraConnType
    {
        Accord,
        Emgucv
    }

    public enum FaceCheckType
    {
        HongRuan,
        BaiDu,
        YiSheng
    }
}
