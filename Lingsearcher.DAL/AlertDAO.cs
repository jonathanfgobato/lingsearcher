using Lingsearcher.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.DAL
{
    public class AlertDAO : BaseDAO<Alert>
    {
        public void UpdateAlertNotification(Alert entity)
        {
            ExecuteProcedure($"Spr_Alterar_{Name}_Notification",new { Id = entity.Id, NumberNotificationsSend = entity.NumberNotificationsSend});
        }
    }
}
