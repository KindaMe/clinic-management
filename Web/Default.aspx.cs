using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data;

namespace Web
{
    public partial class _Default : Page
    {
        private static BindingList<DoctorModel> Doctors;
        private static BindingList<SpecializationModel> Specializations;
        private static BindingList<WorkScheduleModel> Schedule;
        private static BindingList<AppointmentModel> Appointments;

        private static DoctorModel SelectedDoctor;

        protected void Page_Load(object sender, EventArgs e)
        {
            Specializations = DataAccess.LoadSpecializations();
            if (!IsPostBack)
            {
                ListBox1.DataSource = Specializations;
                ListBox1.DataTextField = "Name";
                ListBox1.DataValueField = "ID";
                ListBox1.DataBind();
            }
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedSpecializationID = int.Parse(ListBox1.SelectedItem.Value);

            SpecializationModel SelectedSpecialization = Specializations.ToList().Find(x => x.ID == SelectedSpecializationID);

            Label1.Text = "&nbsp;";

            Doctors = DataAccess.GetDoctorsWithSpecialization(SelectedSpecialization);
            ListBox2.DataSource = Doctors;
            ListBox2.DataBind();
        }

        protected void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDoctor = Doctors.ToList().Find(x => x.LastName + " " + x.FirstName == ListBox2.SelectedItem.Value);

            Label1.Text = GetAppointmentDate();
        }

        private string GetAppointmentDate()
        {
            Schedule = DataAccess.LoadSchedule();

            DateTime DateChecker = DateTime.MaxValue;

            foreach (WorkScheduleModel schedule in Schedule)
            {
                if (schedule.Date > DateTime.Now)
                    if (schedule.Doctor_ID == SelectedDoctor.ID)
                    {
                        if (schedule.Date < DateChecker && AnyAppointmentsAvailableInSchedule(schedule) == true)
                        {
                            DateChecker = schedule.Date;
                        }
                    }
            }
            if (DateChecker != DateTime.MaxValue)
            {
                DateTime TimeChecker = GetAppointmentTime(DateChecker);
                if (TimeChecker != DateTime.MaxValue)
                    return new DateTime(DateChecker.Year, DateChecker.Month, DateChecker.Day, TimeChecker.Hour, TimeChecker.Minute, TimeChecker.Second).ToString();
            }

            return "Brak Dostępnych Wizyt!";
        }

        private bool AnyAppointmentsAvailableInSchedule(WorkScheduleModel schedule)
        {
            Appointments = DataAccess.LoadAppointments();

            DateTime PivotTime = schedule.TimeStart;

            while (PivotTime < schedule.TimeStart.AddHours(1))
            {
                if (!Appointments.ToList().Exists(x => x.DateAndTime.ToShortTimeString() == PivotTime.ToShortTimeString()))
                {
                    return true;
                }

                PivotTime = PivotTime.AddMinutes(10);
            }

            return false;
        }

        private DateTime GetAppointmentTime(DateTime Date)
        {
            foreach (WorkScheduleModel schedule in Schedule)
            {
                if (schedule.Doctor_ID == SelectedDoctor.ID && schedule.Date.ToShortDateString() == Date.ToShortDateString())
                {
                    DateTime PivotTime = schedule.TimeStart;

                    while (PivotTime < schedule.TimeStart.AddHours(1))//fix that later
                    {
                        if (!Appointments.ToList().Exists(x => x.DateAndTime.ToString() == PivotTime.ToString() && x.Doctor_ID == SelectedDoctor.ID))
                        {
                            FreeAppointments appointment = new FreeAppointments
                            {
                                time = PivotTime,
                                Room_ID = schedule.Room_ID
                            };

                            return appointment.time;
                        }

                        PivotTime = PivotTime.AddMinutes(10);
                    }
                }
            }
            DateTime empty = DateTime.MaxValue;
            return empty;
        }
    }
}