using nat.Storage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Manager
{
    public class Help
    {
        public Guid TrainerId { get; set; }
        public Guid CenterId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer{get; set;}
    }
}
