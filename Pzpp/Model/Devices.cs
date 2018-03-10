using Pzpp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pzpp
{

    public class Devices :IEntity
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Responses> Responses { get; set; }
        public Devices()
        {
            Responses = new HashSet<Responses>();
        }
    }
}
