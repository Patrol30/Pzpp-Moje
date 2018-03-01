using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pzpp
{
    class PingDataContext: DbContext
    {
        public DbSet<Devices> Devices { get; set; }
        public DbSet<Responses> Responses { get; set; }
    }
}
