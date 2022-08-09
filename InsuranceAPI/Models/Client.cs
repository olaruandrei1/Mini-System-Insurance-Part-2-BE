using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceAPI.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string InsuranceType { get; set; }
        public string DateOfStart { get; set; }
        public string PhotoCI { get; set; }
    }
}
