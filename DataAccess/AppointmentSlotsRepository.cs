using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class AppointmentSlotsRepository : DbConnection
    {
        public AppointmentSlotsRepository()
            : base()
        {
        }

        public IEnumerable<AppointmentSlot> GetAppointmentSlots()
        {
            return Entity.AppointmentSlots.AsEnumerable();
        }
        
    }
}