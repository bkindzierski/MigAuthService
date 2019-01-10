﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RagAppGuide.DAL.Models
{
    public class DWXF710
    {
        private string desc;
        [DataMember]
        public virtual string PRMSTE { get; set; }
        [DataMember]
        public virtual string DESC {
            get {
                if (desc == null || !string.IsNullOrEmpty(desc))
                {
                   return desc.Trim();
                }
                else
                {
                    return desc = "";
                }
            }
            set
            {
                if (value != null || !string.IsNullOrEmpty(value))
                {
                    desc = value.Trim();
                }
                else
                {
                    desc = "";
                }
            }
        }
        [DataMember]
        public virtual string CLASX { get; set; }
        //public virtual string CLSSEQ { get; set; }
        //public virtual Int32 EFFDTE { get; set; }
        //public virtual Int32 ENDDTE { get; set; }
        //public virtual Int32 RNWEFF { get; set; }
        //public virtual Int32 RNWEXP { get; set; }
        //public virtual string WEBCLS { get; set; }
        //public virtual string REFDSC { get; set; }
        //public virtual string REFCLS { get; set; }
        //public virtual string REFSEQ { get; set; }
        [DataMember]
        public virtual string MAPCLS { get; set; }
        [DataMember]
        public virtual string AUTCLS { get; set; }
        [DataMember]
        public virtual string COVCLS { get; set; }
        //public virtual string SICCDE { get; set; }
        [DataMember]
        public virtual string MAPMS { get; set; }
        public virtual string AUTMS { get; set; }
        public virtual string COVMS { get; set; }
        [DataMember]
        public virtual string MAPDSR { get; set; }
        [DataMember]
        public virtual string AUTDSR { get; set; }
        [DataMember]
        public virtual string PROPDSR { get; set; }
        [DataMember]
        public virtual string GLDSR { get; set; }
        [DataMember]
        public virtual string WCDSR { get; set; }
        [DataMember]
        public virtual string CAUTDSR { get; set; }
        [DataMember]
        public virtual string COVDSR { get; set; }
        [DataMember]
        public virtual string CUPDSR { get; set; }
       
        //public virtual string MAPCMT { get; set; }
        //public virtual string AUTCMT { get; set; }
        //public virtual string PROPCMT { get; set; }
        //public virtual string GLCMT { get; set; }
        //public virtual string WCCMT { get; set; }
        //public virtual string CAUTCMT { get; set; }
        //public virtual string COVCMT { get; set; }
        //public virtual string CUPCMT { get; set; }
        //public virtual string SPCTYP { get; set; }
        //public virtual Int32 CLSDED { get; set; }
        //public virtual Int32 RCDID { get; set; }

        
    }
}
