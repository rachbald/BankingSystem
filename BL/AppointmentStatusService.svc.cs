using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;
using DataAccess;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppointmentStatusService" in code, svc and config file together.
    public class AppointmentStatusService : IAppointmentStatusService
    {
        public IEnumerable<AppointmentStatu> GetAppointmentStatuses()
        {
            return new AppointmentStatusRepository().GetAppointmentStatuses();
        }

        public AppointmentStatu GetAppointmentStatusId(String status)
        {
            return new AppointmentStatusRepository().GetAppointmentStatusId(status);
        }

        public AppointmentStatu GetAppointmentStatusById(int id)
        {
            return new AppointmentStatusRepository().GetAppointmentStatusById(id);
        }
    }
}
