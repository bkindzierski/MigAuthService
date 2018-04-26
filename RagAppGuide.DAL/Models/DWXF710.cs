using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagAppGuide.DAL.Models
{
    public class DWXF710
    {

        public virtual string PRMSTE { get; set; }
        public virtual string DESC { get; set; }
        public virtual string CLASX { get; set; }
        public virtual string CLSSEQ { get; set; }
        public virtual Int32 EFFDTE { get; set; }
        public virtual Int32 ENDDTE { get; set; }
        public virtual Int32 RNWEFF { get; set; }
        public virtual Int32 RNWEXP { get; set; }
        public virtual string WEBCLS { get; set; }
        public virtual string REFDSC { get; set; }
        public virtual string REFCLS { get; set; }
        public virtual string REFSEQ { get; set; }
        public virtual string MAPCLS { get; set; }
        public virtual string AUTCLS { get; set; }
        public virtual string COVCLS { get; set; }
        public virtual string SICCDE { get; set; }
        public virtual string MAPMS { get; set; }
        public virtual string AUTMS { get; set; }
        public virtual string COVMS { get; set; }
        public virtual string MAPDSR { get; set; }
        public virtual string AUTDSR { get; set; }
        public virtual string PROPDSR { get; set; }
        public virtual string GLDSR { get; set; }
        public virtual string WCDSR { get; set; }
        public virtual string CAUTDSR { get; set; }
        public virtual string COVDSR { get; set; }
        public virtual string CUPDSR { get; set; }
        public virtual string MAPCMT { get; set; }
        public virtual string AUTCMT { get; set; }
        public virtual string PROPCMT { get; set; }
        public virtual string GLCMT { get; set; }
        public virtual string WCCMT { get; set; }
        public virtual string CAUTCMT { get; set; }
        public virtual string COVCMT { get; set; }
        public virtual string CUPCMT { get; set; }
        public virtual string SPCTYP { get; set; }
        public virtual Int32 CLSDED { get; set; }
        public virtual Int32 RCDID { get; set; }

        
    }
}
