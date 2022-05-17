using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SpecializationModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class DoctorModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BeganWorkYear { get; set; }
        public BindingList<SpecializationModel> Specializations { get; set; }

        public override string ToString()
        {
            return LastName + " " + FirstName;
        }

        public string ToString_ScheduleHeader()
        {
            return "";
        }
    }

    public class RoomModel
    {
        public int ID { get; set; }
        public int Number { get; set; }
    }

    public class PatientModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NumberID { get; set; }
        public int BornYear { get; set; }
        public string Address { get; set; }
        public string NumberPhone { get; set; }

        public string Email { get; set; }
        public BindingList<AuthorizedPersonModel> AuthorizedPeople { get; set; }
    }

    public class AuthorizedPersonModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool HealthState { get; set; }
        public bool MedicalDocumentation { get; set; }
    }

    public class WorkScheduleModel
    {
        public int Doctor_ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int Room_ID { get; set; }
    }

    public class AppointmentModel
    {
        private PatientModel Patient { get; set; }
        private DoctorModel Doctor { get; set; }
        private DateTime DateAndTime { get; set; }
        private RoomModel Room { get; set; }
    }
}