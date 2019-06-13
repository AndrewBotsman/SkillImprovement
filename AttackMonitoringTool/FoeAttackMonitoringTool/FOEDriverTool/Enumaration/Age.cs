using FOEDriverTool.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOEDriverTool.Enumaration
{
    public enum Age
    {
        [StringValue("IA")]
        IA = 1,
        [StringValue("EMA")]
        EMA,
        [StringValue("HMA")]
        HMA,
        [StringValue("CA")]
        CA,
        [StringValue("IndA")]
        IndA,
        [StringValue("PE")]
        PE,
        [StringValue("ME")]
        ME,
        [StringValue("PME")]
        PME,
        [StringValue("CE")]
        CE,
        [StringValue("T")]
        T,
        [StringValue("F")]
        F,
        [StringValue("AA")]
        AA
    }
}
