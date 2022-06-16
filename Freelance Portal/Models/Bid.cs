using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance_Portal.Models
{
    public class Bid
    {
        public string bid { get; set; }
        public string orderId { get; set; }
        public string suggPrice { get; set; }
        public string deliveryDate { get; set; }
        public string status { get; set; }
        public string freeLancerId { get; set; }
        public string desc { get; set; }
    }
}
