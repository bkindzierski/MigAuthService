using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagAppGuide.DAL.Models
{
    public class DWXF712
    {
        public virtual int ID { get; set; }

        public virtual Int32 RCDID { get; set; }

        public virtual string CHNGDESC { get; set; }

        public virtual string CREATEBY { get; set; }

        public virtual string APPROVBY { get; set; }

        public virtual Int32 MDFYDATE { get; set; }

        public virtual Int32 CREATDTE { get; set; }

    }
}
