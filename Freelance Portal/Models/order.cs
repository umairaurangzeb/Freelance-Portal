using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance_Portal.Models
{
    public class order
    {
        public string id { get; set; }
        public string JobName { get; set; }
        public string Description { get; set; }
        public string Skill { get; set; }
        public string completionTime { get; set; }
        public string proposedRate { get; set; }
        public string postedBy { get; set; }
        public string postedDate { get; set; }
        public string status { get; set; }
        public string deliveryType { get; set; }
    }
}
