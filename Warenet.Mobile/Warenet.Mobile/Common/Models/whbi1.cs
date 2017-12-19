using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warenet.Mobile.Models
{
    public partial class whbi1
    {
        public virtual int TrxNo { get; set; }
        public virtual string TablePrefix { get; set; }
        public virtual string CreateBy { get; set; }
        public virtual DateTime? CreateDateTime { get; set; }
    }
}