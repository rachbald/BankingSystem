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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppointmentSlotsService" in code, svc and config file together.
    public class AppointmentSlotsService : IAppointmentSlotsService
    {
        public IEnumerable<AppointmentSlot> GetAppointmentSlots()
        {
            return new AppointmentSlotsRepository().GetAppointmentSlots();
        }
    }
}
