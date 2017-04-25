using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBankingSystem.Models
{
    public class AppointmentModel
    {
        public string Username { get; set; }        
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int UserId { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
    }
}