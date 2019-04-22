using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class Alert : Domain
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserSystemId { get; set; }
        public UserSystem UserSystem{ get;set; }
        public double MinPrice { get; set; }
        public int MaxNumberNotifications { get; set; }
        public int NumberNotificationsSend { get; set; }
    }
}
