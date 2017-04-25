using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAppointmentStatusService" in both code and config file together.
    [ServiceContract]
    public interface IAppointmentStatusService
    {
        [OperationContract]
        IEnumerable<AppointmentStatu> GetAppointmentStatuses();

        [OperationContract]
        AppointmentStatu GetAppointmentStatusId(string word);

        [OperationContract]
        AppointmentStatu GetAppointmentStatusById(int AppointmentStatuId);
    }
}
