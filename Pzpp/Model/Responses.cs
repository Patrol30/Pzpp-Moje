using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pzpp
{
    class Responses
    {
        public Guid ID { get; set; }
        public bool Success { get; set; }
        public long Time { get; set; }
        public virtual Devices Device { get; set; }
    }
}
