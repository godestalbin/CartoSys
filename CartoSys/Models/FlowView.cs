using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartoSys.Models
{
    public class FlowView
    {
        public int ID { get; set; }
        public String SourceName { get; set; }
        public String TargetName { get; set; }
        public string Status { get; set; }
        public string FlowType { get; set; }
        public string SendingMode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}