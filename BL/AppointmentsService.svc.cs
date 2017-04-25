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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppointmentsService" in code, svc and config file together.
    public class AppointmentsService : IAppointmentsService
    {
        public IEnumerable<Appointment> GetAppointments()
        {
            return new AppointmentsRepository().GetAppointments();
        }

        public Appointment GetUserAppointment(int userId)
        {
            return new AppointmentsRepository().GetUserAppointment(userId);
        }

        public bool AppointmentExists(Appointment a)
        {
            return new AppointmentsRepository().AppointmentExists(a);
        }

        public Appointment GetAppointment(Appointment a)
        {
            return new AppointmentsRepository().GetAppointment(a);
        }

        public Appointment GetAppointmentById(int Id)
        {
            return new AppointmentsRepository().GetAppointmentById(Id);
        }

        public bool UpdateAppointment(Appointment a)
        {
            return new AppointmentsRepository().UpdateAppointment(a);
        }

        public bool DeleteAppointment(Appointment a)
        {
            return new AppointmentsRepository().DeleteAppointment(a);
        }

        public bool AppointmentRequest(Appointment a)
        {
            return new AppointmentsRepository().AppointmentRequest(a);
        }

         public Appointment UserExistsCheck(Appointment a)
        {
            return new AppointmentsRepository().UserExistsCheck(a);
        }
    }
}
