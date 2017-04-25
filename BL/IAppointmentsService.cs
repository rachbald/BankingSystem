using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAppointmentsService" in both code and config file together.
    [ServiceContract]
    public interface IAppointmentsService
    {
        [OperationContract]
        IEnumerable<Appointment> GetAppointments();

        [OperationContract]
        Appointment GetUserAppointment(int userId);

        [OperationContract]
        bool AppointmentExists(Appointment a);

        [OperationContract]
        Appointment GetAppointment(Appointment a);

        [OperationContract]
        Appointment GetAppointmentById(int Id);

        [OperationContract]
        bool UpdateAppointment(Appointment a);

        [OperationContract]
        bool DeleteAppointment(Appointment a);

        [OperationContract]
        bool AppointmentRequest(Appointment a);

        [OperationContract]
        Appointment UserExistsCheck(Appointment a);
    }
}
