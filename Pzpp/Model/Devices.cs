using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pzpp
{
    class Devices
    {
        public string IP { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Responses> Responses { get; set; }
        public Devices()
        {
            Responses = new HashSet<Responses>();
        }
    }
}
