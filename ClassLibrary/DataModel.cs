using System;
using System.ComponentModel;

namespace Data
{
    public class SpecializationModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
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
    }

    public class RoomModel
    {
        public int ID { get; set; }
        public int Number { get; set; }

        public override string ToString()
        {
            return Number.ToString();
        }
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

        public override string ToString()
        {
            return LastName + " " + FirstName;
        }
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
        public int ID { get; set; }
        public int Doctor_ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int Room_ID { get; set; }
    }

    public class AppointmentModel
    {
        public int ID { get; set; }

        public int Specialization_ID { get; set; }

        public string Specialization
        { get { return DataAccess.GetSpecializationName(Specialization_ID); } }

        public int Patient_ID { get; set; }

        public string Patient
        { get { return DataAccess.GetPatientName(Patient_ID); } }

        public int Doctor_ID { get; set; }

        public string Doctor
        { get { return DataAccess.GetDoctorName(Doctor_ID); } }

        public DateTime DateAndTime { get; set; }

        public int Room_ID { get; set; }

        public int Room
        { get { return DataAccess.GetRoomName(Room_ID); } }
    }

    public class FreeAppointments
    {
        public DateTime time { get; set; }
        public int Room_ID { get; set; }
    }
}