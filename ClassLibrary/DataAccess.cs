using Dapper;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Data
{
    public class DataAccess
    {
        #region Connection String

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        #endregion Connection String

        #region Read Data

        public static BindingList<DoctorModel> LoadDoctors()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<DoctorModel>("select * from Doctors order by LastName", new DynamicParameters());
                BindingList<DoctorModel> temp = new BindingList<DoctorModel>(output.ToList());

                foreach (DoctorModel doc in temp)
                {
                    var output2 = cnn.Query<SpecializationModel>("select Specializations.ID,Specializations.Name from Doctors_Specializations " +
                            "inner join Specializations on Doctors_Specializations.Specialization_ID = Specializations.ID " +
                            "where Doctor_ID = @ID", doc);
                    doc.Specializations = new BindingList<SpecializationModel>(output2.ToList());
                }

                return temp;
            }
        }

        public static BindingList<SpecializationModel> LoadSpecializations()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<SpecializationModel>("select * from Specializations order by Name", new DynamicParameters());

                return new BindingList<SpecializationModel>(output.ToList());
            }
        }

        public static BindingList<PatientModel> LoadPatients()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PatientModel>("select * from Patients order by LastName", new DynamicParameters());
                BindingList<PatientModel> temp = new BindingList<PatientModel>(output.ToList());

                foreach (PatientModel patient in temp)
                {
                    var output2 = cnn.Query<AuthorizedPersonModel>("SELECT AuthorizedPeople.FirstName, " +
                        "AuthorizedPeople.LastName, " +
                        "AuthorizedPeople.HealthState, " +
                        "AuthorizedPeople.MedicalDocumentation " +
                        "FROM Patients_AuthorizedPeople " +
                        "INNER JOIN " +
                        "Patients ON Patients_AuthorizedPeople.Patient_ID = Patients.ID " +
                        "INNER JOIN " +
                        "AuthorizedPeople ON Patients_AuthorizedPeople.AuthorizedPerson_ID = AuthorizedPeople.ID " +
                        "WHERE Patients.ID = @ID;", patient);
                    patient.AuthorizedPeople = new BindingList<AuthorizedPersonModel>(output2.ToList());
                }

                return temp;
            }
        }

        public static BindingList<RoomModel> LoadRooms()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<RoomModel>("select * from Rooms order by Number", new DynamicParameters());

                return new BindingList<RoomModel>(output.ToList());
            }
        }

        public static BindingList<WorkScheduleModel> LoadSchedule()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<WorkScheduleModel>("select * from Schedule order by Date,TimeStart", new DynamicParameters());

                return new BindingList<WorkScheduleModel>(output.ToList());
            }
        }

        public static BindingList<AppointmentModel> LoadAppointments()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<AppointmentModel>("select * from Appointments", new DynamicParameters());

                return new BindingList<AppointmentModel>(output.ToList());
            }
        }

        public static BindingList<AppointmentView> LoadAppointmentView(int DoctorIndex, DateTime Date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { Doc_ID = DoctorIndex, Day = Date };

                var output = cnn.Query<AppointmentView>("SELECT Appointments.ID, " +
                    "Patients.LastName || ' ' || Patients.FirstName AS Patient, " +
                    "Doctors.LastName || ' ' || Doctors.FirstName AS Doctor, " +
                    "Appointments.DateAndTime, " +
                    "Rooms.Number AS Room " +
                    "FROM Appointments " +
                    "INNER JOIN " +
                    "Patients ON Appointments.Patient_ID = Patients.ID " +
                    "INNER JOIN " +
                    "Doctors ON Appointments.Doctor_ID = Doctors.ID " +
                    "INNER JOIN " +
                    "Rooms ON Appointments.Room_ID = Rooms.ID " +
                    "where Doctors.ID = @Doc_ID and DATE(Appointments.DateAndTime) = DATE(@Day);", parameters);

                return new BindingList<AppointmentView>(output.ToList());
            }
        }

        #endregion Read Data

        #region Write Data

        public static void InsertSchedule(WorkScheduleModel NewSchedule)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<RoomModel>("select * from Rooms order by Number", new DynamicParameters());
                cnn.Execute("insert into Schedule (Doctor_ID,Date,TimeStart,TimeEnd,Room_ID) values (@Doctor_ID,@Date,@TimeStart,@TimeEnd,@Room_ID)", NewSchedule);
            }
        }

        public static void UpdateSchedule(WorkScheduleModel ScheduleToUpdate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                if (ScheduleToUpdate.TimeStart != ScheduleToUpdate.TimeEnd)
                {
                    cnn.Execute("update Schedule SET TimeStart = @TimeStart,TimeEnd = @TimeEnd WHERE ID = @ID ", ScheduleToUpdate);
                }
                else
                {
                    cnn.Execute("delete from Schedule where ID=@ID", ScheduleToUpdate);
                }
            }
        }

        public static int InsertPatient(PatientModel NewPatient)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var identity = cnn.ExecuteScalar<int>("insert into Patients (FirstName,LastName,NumberID,BornYear,Address,NumberPhone,Email) values (@FirstName,@LastName,@NumberID,@BornYear,@Address,@NumberPhone,@Email); SELECT last_insert_rowid();", NewPatient);

                foreach (AuthorizedPersonModel AuthPerson in NewPatient.AuthorizedPeople)
                {
                    var parameters = new { Patient_ID = identity, AuthorizedPerson_ID = InsertAuthorizedPerson(AuthPerson) };
                    cnn.Execute("insert into Patients_AuthorizedPeople (Patient_ID,AuthorizedPerson_ID) values (@Patient_ID,@AuthorizedPerson_ID)", parameters);
                }
                return identity;
            }
        }

        public static int InsertAuthorizedPerson(AuthorizedPersonModel NewAuthorizedPerson)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var identity = cnn.ExecuteScalar<int>("INSERT INTO AuthorizedPeople " +
                    "(FirstName, LastName, HealthState,MedicalDocumentation) " +
                    "VALUES " +
                    "(@FirstName, @LastName, @HealthState,@MedicalDocumentation); SELECT last_insert_rowid();", NewAuthorizedPerson);

                return identity;
            }
        }

        public static void InsertAppointment(AppointmentModel NewAppointment)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Appointments (Patient_ID,Doctor_ID,DateAndTime,Room_ID) values (@Patient_ID,@Doctor_ID,@DateAndTime,@Room_ID)", NewAppointment);
            }
        }

        public static void InsertDoctor(DoctorModel NewDoctor)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var identity = cnn.ExecuteScalar<int>("insert into Doctors (FirstName,LastName,BeganWorkYear) values (@FirstName,@LastName,@BeganWorkYear); select last_insert_rowid();", NewDoctor);
                foreach (SpecializationModel Spec in NewDoctor.Specializations)
                {
                    var parameters = new { Doctor_ID = identity, Specialization_ID = Spec.ID };
                    cnn.Execute("insert into Doctors_Specializations (Doctor_ID,Specialization_ID) values (@Doctor_ID,@Specialization_ID)", parameters);
                }
            }
        }

        public static void InsertRoom(RoomModel NewRoom)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Rooms (Number) values (@Number)", NewRoom);
            }
        }

        public static void InsertSpecialization(SpecializationModel NewSpecialization)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Specializations (Name) values (@Name)", NewSpecialization);
            }
        }

        #endregion Write Data

        #region Delete Data

        public static void DeleteRoom(RoomModel RoomToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Appointments where Room_ID = @ID", RoomToDelete);
                cnn.Execute("delete from Schedule where Room_ID = @ID", RoomToDelete);

                cnn.Execute("delete from Rooms where ID = @ID", RoomToDelete);
            }
        }

        public static void DeleteSpecialization(SpecializationModel SpecializationToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("DELETE FROM Appointments " +
                    "WHERE Appointments.Doctor_ID IN(" +
                    "SELECT Doctors.ID " +
                    "FROM Doctors " +
                    "WHERE Doctors.ID IN(" +
                    "SELECT Doctors_Specializations.Doctor_ID " +
                    "FROM Doctors_Specializations " +
                    "WHERE Doctors_Specializations.Specialization_ID IN(" +
                    "SELECT Specializations.ID " +
                    "FROM Specializations " +
                    "WHERE Specializations.ID = @ID)));", SpecializationToDelete);
                cnn.Execute("delete from Doctors_Specializations where Specialization_ID = @ID", SpecializationToDelete);

                cnn.Execute("delete from Specializations where ID = @ID", SpecializationToDelete);
            }
        }

        public static void DeleteDoctor(DoctorModel DoctorToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Doctors_Specializations where Doctor_ID = @ID", DoctorToDelete);
                cnn.Execute("delete from Appointments where Doctor_ID = @ID", DoctorToDelete);
                cnn.Execute("delete from Schedule where Doctor_ID = @ID", DoctorToDelete);

                cnn.Execute("delete from Doctors where ID = @ID", DoctorToDelete);
            }
        }

        public static void DeletePatient(PatientModel PatientToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Patients_AuthorizedPeople where Patient_ID = @ID", PatientToDelete);
                cnn.Execute("delete from Appointments where Patient_ID = @ID", PatientToDelete);

                cnn.Execute("delete from Patients where ID = @ID", PatientToDelete);
            }
        }

        public static void DeleteAppointment(AppointmentView AppointmentToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Appointments where ID = @ID", AppointmentToDelete);
            }
        }

        #endregion Delete Data
    }
}