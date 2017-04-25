using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class AppointmentStatusRepository : DbConnection
    {
        public AppointmentStatusRepository()
            : base()
        { }

        public IEnumerable<AppointmentStatu> GetAppointmentStatuses()
        {
            return Entity.AppointmentStatus.AsEnumerable();
        }

        public AppointmentStatu GetAppointmentStatusId(String status)
        {
            return Entity.AppointmentStatus.SingleOrDefault(s => s.State.Equals(status));
        }

        public AppointmentStatu GetAppointmentStatusById(int id)
        {
            return Entity.AppointmentStatus.SingleOrDefault(s => s.Id.Equals(id));
        }
    }
}
