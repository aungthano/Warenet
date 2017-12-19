using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warenet.Mobile.Models
{
    public partial class whbi2
    {
        public virtual int TrxNo { get; set; }
        public virtual string TablePrefix { get; set; }
        public virtual int LineItemNo { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string ItemName { get; set; }
        public virtual string DimensionFlag { get; set; }
        public virtual int? Qty { get; set; }
        public virtual string UomCode { get; set; }
        public virtual decimal? Length { get; set; }
        public virtual decimal? Width { get; set; }
        public virtual decimal? Height { get; set; }
        public virtual decimal? Weight { get; set; }
        public virtual decimal? Volume { get; set; }
        public virtual decimal? SpaceArea { get; set; }
        public virtual string WorkStation { get; set; }
        public virtual string CreateBy { get; set; }
        public virtual DateTime CreateDateTime { get; set; }
    }
}