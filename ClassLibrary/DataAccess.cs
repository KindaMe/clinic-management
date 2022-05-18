using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

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
                    var output2 = cnn.Query<SpecializationModel>("select Specializations.Name from Doctors_Specializations " +
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
                    var output2 = cnn.Query<AuthorizedPersonModel>("SELECT AuthorizedPeople.FirstName,AuthorizedPeople.LastName,AuthorizedPeople.HealthState,AuthorizedPeople.MedicalDocumentation " +
                        "FROM Patients_AuthorizedPeople " +
                        "INNER JOIN AuthorizedPeople ON Patients_AuthorizedPeople.AuthorizedPerson_ID = AuthorizedPeople.ID " +
                        "WHERE AuthorizedPeople.ID = @ID ", patient);
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

        #endregion Read Data

        #region Write Data

        //

        public static void AddSchedule(WorkScheduleModel NewSchedule)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<RoomModel>("select * from Rooms order by Number", new DynamicParameters());
                cnn.Execute("insert into Schedule (Doctor_ID,Date,TimeStart,TimeEnd,Room_ID) values (@Doctor_ID,@Date,@TimeStart,@TimeEnd,@Room_ID)", NewSchedule);
            }
        }

        #endregion Write Data
    }
}