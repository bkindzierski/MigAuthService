using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagAppGuide.DAL.Models
{
    public class DWXF711
    {

        public virtual Int32 RCDID { get; set; }
        public virtual string PROD { get; set; }
        public virtual string HAZGRD { get; set; }
        public virtual string ACTIVE { get; set; }
        public virtual string IDADDBY { get; set; }
        public virtual string DTEADD { get; set; }
        public virtual string IDCHGBY { get; set; }
        public virtual string DTECHG { get; set; }

    }

}
