using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using RagAppGuide.DAL.Models;

namespace RagAppGuide.DAL.Mappings
{
    public class DWXF710Map : ClassMap<DWXF710>
    {
        public DWXF710Map()
        {

            Id(x => x.PRMSTE);
            Id(x => x.DESC);
            Id(x => x.CLASX);
            Id(x => x.CLSSEQ);
            Id(x => x.EFFDTE);
            Id(x => x.RCDID);

            Map(x => x.PRMSTE);
            Map(x => x.DESC);
            Map(x => x.CLASX);
            Map(x => x.CLSSEQ);
            Map(x => x.EFFDTE);
            Map(x => x.ENDDTE);
            Map(x => x.RNWEFF);
            Map(x => x.RNWEXP);
            Map(x => x.WEBCLS);
            Map(x => x.REFDSC);
            Map(x => x.REFCLS);
            Map(x => x.REFSEQ);
            Map(x => x.MAPCLS);
            Map(x => x.AUTCLS);
            Map(x => x.COVCLS);
            Map(x => x.SICCDE);
            Map(x => x.MAPMS);
            Map(x => x.AUTMS);
            Map(x => x.COVMS);
            Map(x => x.MAPDSR);
            Map(x => x.AUTDSR);
            Map(x => x.PROPDSR);
            Map(x => x.GLDSR);
            Map(x => x.WCDSR);
            Map(x => x.CAUTDSR);
            Map(x => x.COVDSR);
            Map(x => x.CUPDSR);
            Map(x => x.MAPCMT);
            Map(x => x.AUTCMT);
            Map(x => x.PROPCMT);
            Map(x => x.GLCMT);
            Map(x => x.WCCMT);
            Map(x => x.CAUTCMT);
            Map(x => x.COVCMT);
            Map(x => x.CUPCMT);
            Map(x => x.SPCTYP);
            Map(x => x.CLSDED);
            Map(x => x.RCDID);
        }
    }
}
