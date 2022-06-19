using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Models
{
    [Serializable]
    public class CameraDeviceInfo
    {
        public Guid Category { get; set; }
        public int Index { get; set; }
        public string MonikerString { get; set; }
        public string Name { get; set; }

        public CameraDeviceInfo()
        {

        }

        public CameraDeviceInfo(string name, string monikerString, int index) : this(name, monikerString, index, Guid.Empty)
        {
        }

        public CameraDeviceInfo(string name, string monikerString, int index, Guid category)
        {
            this.Name = name;
            this.MonikerString = monikerString;
            this.Index = index;
            this.Category = category;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
