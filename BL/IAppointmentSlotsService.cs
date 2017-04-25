﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAppointmentSlotsService" in both code and config file together.
    [ServiceContract]
    public interface IAppointmentSlotsService
    {
        [OperationContract]
        IEnumerable<AppointmentSlot> GetAppointmentSlots();
    }
}
