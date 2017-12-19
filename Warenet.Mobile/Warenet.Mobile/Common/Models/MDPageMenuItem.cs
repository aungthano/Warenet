using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warenet.Mobile.Models
{
    public class MDPageMenuItem
    {
        public MDPageMenuItem() { }
        public int Id { get; set; }
        public string Title { get; set; }
        public Type TargetType { get; set; }
        public bool Visible { get; set; }
    }
}