using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartoSys.Models
{
    public class Applications
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Domain { get; set; }
        public string InChargeBu { get; set; }
        public string InChargeAdeo { get; set; }
        public string Documentation { get; set; }
    }
}