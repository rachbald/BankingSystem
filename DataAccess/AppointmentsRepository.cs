using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class AppointmentsRepository : DbConnection
    {
        public AppointmentsRepository()
            : base()
        { }

        public IEnumerable<Appointment> GetAppointments()
        {
            return Entity.Appointments.AsEnumerable();
        }



        public Appointment GetUserAppointment(int userId)
        {
            return Entity.Appointments.SingleOrDefault(a => a.User.Equals(userId));
        }

        public bool AppointmentExists(Appointment a)
        {
            try
            {
                Appointment saved = Entity.Appointments.SingleOrDefault(ap => ap.User.Equals(a.User) && ap.Date.Equals(a.Date) && ap.Date.Equals(a.Date));

                if (saved != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Appointment UserExistsCheck(Appointment a)
        {
            try
            {
                return Entity.Appointments.SingleOrDefault(ap => ap.User.Equals(a.User) && ap.Date.Equals(a.Date) && ap.Date.Equals(a.Date));

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Appointment GetAppointment(Appointment a)
        {
            return Entity.Appointments.SingleOrDefault(ap => ap.User.Equals(a.User) && ap.Date.Equals(a.Date));
        }

        public Appointment GetAppointmentById(int Id)
        {
            return Entity.Appointments.SingleOrDefault(ap => ap.Id.Equals(Id));
        }

        public bool UpdateAppointment(Appointment a)
        {
            try
            {

                Entity.Appointments.Attach(GetAppointment(a));
                Entity.Appointments.ApplyCurrentValues(a);
                Entity.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool DeleteAppointment(Appointment a)
        {
            try
            {
                Entity.Appointments.DeleteObject(GetAppointment(a));
                Entity.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AppointmentRequest(Appointment a)
        {
            try
            {
                Entity.AddToAppointments(a);
                Entity.SaveChanges();

                return true;
                            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
