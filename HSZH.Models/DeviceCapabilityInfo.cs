using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Models
{
    [Serializable]
    public class DeviceCapabilityInfo
    {
        public Size FrameSize { get; set; }
        public int MaxFrameRate { get; set; }

        public DeviceCapabilityInfo()
        {

        }

        public DeviceCapabilityInfo(Size frameSize, int maxFrameRate)
        {
            this.FrameSize = frameSize;
            this.MaxFrameRate = maxFrameRate;
        }

        public override string ToString()
        {
            return string.Format("{0}X{1}  {2}FPS", this.FrameSize.Width, this.FrameSize.Height, this.MaxFrameRate);
        }
    }
}
