using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using RagAppGuide.DAL.Models;

namespace RagAppGuide.DAL.Mappings
{
    public class DWXF712Map : ClassMap<DWXF712>
    {
        public DWXF712Map()
        {
            //Id(x => x.ID);
            Id(x => x.ID).Column("ID").GeneratedBy.Increment();

            //Map(x => x.ID);
            Map(x => x.RCDID);
            Map(x => x.CHNGDESC);
            Map(x => x.CREATEBY);
            Map(x => x.APPROVBY);
            Map(x => x.MDFYDATE);
            Map(x => x.CREATDTE);
        }

    }
}
