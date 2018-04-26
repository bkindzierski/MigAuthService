using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using RagAppGuide.DAL.Models;

namespace RagAppGuide.DAL.Mappings
{
    public class DWXF711Map : ClassMap<DWXF711>
    {
        public DWXF711Map() {

            Id(x => x.RCDID).Column("RCDID").GeneratedBy.Increment();
            
            Map(x => x.RCDID);
            Map(x => x.PROD);
            Map(x => x.HAZGRD);
            Map(x => x.ACTIVE);
            Map(x => x.IDADDBY);
            Map(x => x.DTEADD);
            Map(x => x.IDCHGBY);
            Map(x => x.DTECHG);

        }
    }
}
