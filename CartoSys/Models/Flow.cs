using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartoSys.Models
{
    public class Flow
    {
        public int ID { get; set; }
        public int SourceId { get; set; }
        public int TargetId { get; set; }
        public string Status { get; set; }
        public int? FlowType { get; set; }
        public string SendingMode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}