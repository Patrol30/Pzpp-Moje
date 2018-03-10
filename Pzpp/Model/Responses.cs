using Pzpp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pzpp
{
    public class Responses :IEntity
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public long Time { get; set; }

        [ForeignKey("Device")]
        public int Device_Id { get; set; }
        public virtual Devices Device { get; set; }
    }
}
