using Dapper;
using System;
using System.Collections.Generic;
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

        #region Select Data

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

        #region Appointment View

        public static BindingList<AppointmentModel> LoadAppointmentsView(int DoctorIndex, DateTime Date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { Doc_ID = DoctorIndex, Day = Date };

                var output = cnn.Query<AppointmentModel>("SELECT * FROM Appointments " +
                    "where Doctor_ID = @Doc_ID and DATE(DateAndTime) = DATE(@Day);", parameters);

                return new BindingList<AppointmentModel>(output.ToList());
            }
        }

        public static string GetSpecializationName(int SpecializationID)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { ID = SpecializationID };
                var output = cnn.QueryFirst<string>("select Name from Specializations where ID=@ID", parameters);

                return output;
            }
        }

        public static string GetPatientName(int PatientID)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { ID = PatientID };
                var output = cnn.QueryFirst<string>("select LastName ||' '|| FirstName from Patients where ID=@ID", parameters);

                return output;
            }
        }

        public static string GetDoctorName(int DoctorID)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { ID = DoctorID };
                var output = cnn.QueryFirst<string>("select LastName ||' '|| FirstName from Doctors where ID=@ID", parameters);

                return output;
            }
        }

        public static int GetRoomName(int RoomID)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var parameters = new { ID = RoomID };
                var output = cnn.QueryFirst<int>("select Number from Rooms where ID=@ID", parameters);

                return output;
            }
        }

        #endregion Appointment View

        #endregion Select Data

        #region Insert Data

        public static void InsertSchedule(WorkScheduleModel NewSchedule)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Schedule (Doctor_ID,Date,TimeStart,TimeEnd,Room_ID) values (@Doctor_ID,@Date,@TimeStart,@TimeEnd,@Room_ID)", NewSchedule);
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
                cnn.Execute("insert into Appointments (Specialization_ID,Patient_ID,Doctor_ID,DateAndTime,Room_ID) values (@Specialization_ID,@Patient_ID,@Doctor_ID,@DateAndTime,@Room_ID)", NewAppointment);
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

        #endregion Insert Data

        #region Update Data

        public static void UpdateRoom(RoomModel RoomToUpdate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update Rooms set Number = @Number where ID = @ID", RoomToUpdate);
            }
        }

        public static void UpdateSpecialization(SpecializationModel SpecializationToUpdate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update Specializations set Name = @Name where ID = @ID", SpecializationToUpdate);
            }
        }

        public static void UpdateDoctor(DoctorModel DoctorToUpdate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update Doctors set FirstName = @FirstName, LastName = @LastName, BeganWorkYear = @BeganWorkYear where ID = @ID", DoctorToUpdate);

                BindingList<SpecializationModel> AllSpecializations = LoadSpecializations();

                var AllDoctorSpecializations = cnn.Query<SpecializationModel>(
                    "SELECT ID, Name " +
                    "FROM Specializations " +
                    "INNER JOIN " +
                    "Doctors_Specializations ON Specializations.ID = Doctors_Specializations.Specialization_ID " +
                    "WHERE Doctor_ID = @ID; ", DoctorToUpdate);

                foreach (SpecializationModel Spec in AllSpecializations)
                {
                    var parameters = new { Doctor_ID = DoctorToUpdate.ID, Specialization_ID = Spec.ID };

                    if (AllDoctorSpecializations.ToList().Exists(x => x.ID == Spec.ID) == true && DoctorToUpdate.Specializations.ToList().Exists(x => x.ID == Spec.ID) == false)
                    {
                        cnn.Execute("delete from Doctors_Specializations where Doctor_ID = @Doctor_ID and Specialization_ID = @Specialization_ID", parameters);
                        cnn.Execute("delete from Appointments where Doctor_ID = @Doctor_ID and Specialization_ID = @Specialization_ID", parameters);
                    }
                    else if (AllDoctorSpecializations.ToList().Exists(x => x.ID == Spec.ID) == false && DoctorToUpdate.Specializations.ToList().Exists(x => x.ID == Spec.ID) == true)
                    {
                        cnn.Execute("insert into Doctors_Specializations (Doctor_ID,Specialization_ID) values (@Doctor_ID,@Specialization_ID)", parameters);
                    }
                }
            }
        }

        public static void UpdatePatient(PatientModel PatientToUpdate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update Patients set FirstName = @FirstName, LastName = @LastName, NumberID = @NumberID, BornYear = @BornYear, Address = @Address, NumberPhone = @NumberPhone, Email = @Email where ID=@ID", PatientToUpdate);

                cnn.Execute("DELETE FROM AuthorizedPeople " +
                    "WHERE AuthorizedPeople.ID IN(" +
                    "SELECT Patients_AuthorizedPeople.AuthorizedPerson_ID " +
                    "FROM Patients_AuthorizedPeople " +
                    "WHERE Patients_AuthorizedPeople.Patient_ID IN(" +
                    "SELECT Patients.ID " +
                    "FROM Patients " +
                    "WHERE Patients.ID = @ID)); ", PatientToUpdate);

                foreach (AuthorizedPersonModel AuthPerson in PatientToUpdate.AuthorizedPeople)
                {
                    var parameters = new { Patient_ID = PatientToUpdate.ID, AuthorizedPerson_ID = InsertAuthorizedPerson(AuthPerson) };
                    cnn.Execute("insert into Patients_AuthorizedPeople (Patient_ID,AuthorizedPerson_ID) values (@Patient_ID,@AuthorizedPerson_ID)", parameters);
                }
            }
        }

        public static void UpdateAppointment(AppointmentModel AppointmentToUpdate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update Appointments set Specialization_ID = @Specialization_ID, Patient_ID = @Patient_ID, Doctor_ID = @Doctor_ID, DateAndTime = @DateAndTime, Room_ID = @Room_ID where ID = @ID", AppointmentToUpdate);
            }
        }

        #endregion Update Data

        #region Delete Data

        public static void DeleteSchedule(WorkScheduleModel ScheduleToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("DELETE FROM Appointments " +
                    "WHERE Doctor_ID = @Doctor_ID AND " +
                    "Room_ID = @Room_ID AND " +
                    "DateAndTime >= @TimeStart AND " +
                    "DateAndTime < datetime(@TimeStart, '+1 hour')", ScheduleToDelete);

                cnn.Execute("delete from Schedule where ID = @ID", ScheduleToDelete);
            }
        }

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
                cnn.Execute("delete from Appointments where Specialization_ID = @ID;", SpecializationToDelete);
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
                cnn.Execute("DELETE FROM AuthorizedPeople " +
                    "WHERE AuthorizedPeople.ID IN( " +
                    "SELECT Patients_AuthorizedPeople.AuthorizedPerson_ID " +
                    "FROM Patients_AuthorizedPeople " +
                    "WHERE Patients_AuthorizedPeople.Patient_ID IN( " +
                    "SELECT Patients.ID " +
                    "FROM Patients " +
                    "WHERE Patients.ID = @ID)); ", PatientToDelete);
                cnn.Execute("delete from Patients_AuthorizedPeople where Patient_ID = @ID", PatientToDelete);
                cnn.Execute("delete from Appointments where Patient_ID = @ID", PatientToDelete);

                cnn.Execute("delete from Patients where ID = @ID", PatientToDelete);
            }
        }

        public static void DeleteAppointment(AppointmentModel AppointmentToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Appointments where ID = @ID", AppointmentToDelete);
            }
        }

        #endregion Delete Data
    }
}