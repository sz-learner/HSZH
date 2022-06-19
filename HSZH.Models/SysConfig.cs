using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Models
{
    [Serializable]
    public class SysConfig
    {
        public string TT_K7X0_ComPortName { get; set; }
        public string AppTitle { get; set; }
        public string AppExitPassWord { get; set; }
        public int FaceCheckNumber { get; set; }
        public int FaceCheckFZ { get; set; }
        public CameraConnType CameraConnType { get; set; }
        public int CameraIndex { get; set; }

        public bool IsConnReadIDcardDev { get; set; }
        public bool IsOpenFaceCheck { get; set; }

        public ReadIdCardType ConnIDCardType { get; set; }

        public int ReadIDCardPortName { get; set; }

        public string ServerPhoneNumber1 { get; set; }

        public string ServerPhoneNumber2 { get; set; }
    }
}
