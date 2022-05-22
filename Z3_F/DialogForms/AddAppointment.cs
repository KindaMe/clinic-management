using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddAppointment : Form
    {
        private PatientModel ChosenPatient;

        private bool ConstructorFinished = false;

        private BindingList<PatientModel> Patients;
        private BindingList<SpecializationModel> Specializations;
        private BindingList<DoctorModel> Doctors;
        private List<DoctorModel> DoctorsToDisplay;
        private BindingList<WorkScheduleModel> Schedule;
        private BindingList<RoomModel> Rooms;
        private BindingList<AppointmentModel> Appointments;
        private BindingList<FreeAppointments> AppointmentsToDisplay;

        public AddAppointment(BindingList<SpecializationModel> _Specializations, BindingList<DoctorModel> _Doctors, BindingList<WorkScheduleModel> _Schedule, BindingList<PatientModel> _Patients, BindingList<RoomModel> _Rooms)
        {
            InitializeComponent();

            Patients = _Patients;

            Rooms = _Rooms;

            Specializations = _Specializations;
            specializationModelBindingSource.DataSource = Specializations;

            Doctors = _Doctors;
            Schedule = _Schedule;

            Appointments = DataAccess.LoadAppointments();

            AppointmentsToDisplay = new BindingList<FreeAppointments>();
            freeAppointmentsBindingSource.DataSource = AppointmentsToDisplay;

            UpdateDoctors();
            UpdateCalendar();
            UpdateAppointments();

            ConstructorFinished = true;
        }

        private void AddAppointment_FormClosing(object sender, FormClosingEventArgs e) //disables triggers on form closing
        {
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged -= comboBox2_SelectedIndexChanged;
            monthCalendar1.DateChanged -= monthCalendar1_DateChanged;
        }

        #region Changing Value Triggers

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDoctors();
            UpdateCalendar();
            UpdateAppointments();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCalendar();
            UpdateAppointments();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            UpdateAppointments();
        }

        #endregion Changing Value Triggers

        #region Update Fields

        private void UpdateDoctors()
        {
            DoctorsToDisplay = Doctors.ToList().FindAll(x => x.Specializations.ToList().Any(y => y.ID == ((SpecializationModel)comboBox1.SelectedItem).ID));
            doctorModelBindingSource.DataSource = DoctorsToDisplay;
        }

        private void UpdateCalendar()
        {
            DateTime DateChecker = DateTime.MaxValue;

            foreach (WorkScheduleModel schedule in Schedule)
            {
                if (schedule.Doctor_ID == ((DoctorModel)comboBox2.SelectedItem).ID)
                {
                    if (schedule.Date < DateChecker && AnyAppointmentsAvailableInSchedule(schedule) == true)
                    {
                        DateChecker = schedule.Date;
                    }
                }
            }
            if (DateChecker != DateTime.MaxValue)
            {
                monthCalendar1.SelectionStart = DateChecker;
            }
            else
            {
                if (ConstructorFinished == true)
                {
                    monthCalendar1.SelectionStart = DateTime.Now;
                    MessageBox.Show("Brak wolnych wizyt!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool AnyAppointmentsAvailableInSchedule(WorkScheduleModel schedule)
        {
            DateTime PivotTime = schedule.TimeStart;

            while (PivotTime < schedule.TimeEnd)
            {
                if (!Appointments.ToList().Exists(x => x.DateAndTime.ToShortTimeString() == PivotTime.ToShortTimeString()))
                {
                    return true;
                }

                PivotTime = PivotTime.AddMinutes(10);
            }

            return false;
        }

        private void UpdateAppointments()
        {
            AppointmentsToDisplay.Clear();
            foreach (WorkScheduleModel schedule in Schedule)
            {
                if (schedule.Doctor_ID == ((DoctorModel)comboBox2.SelectedItem).ID && schedule.Date.ToShortDateString() == monthCalendar1.SelectionStart.ToShortDateString())
                {
                    DateTime PivotTime = schedule.TimeStart;

                    while (PivotTime < schedule.TimeEnd)
                    {
                        if (!Appointments.ToList().Exists(x => x.DateAndTime.ToShortTimeString() == PivotTime.ToShortTimeString()))
                        {
                            FreeAppointments appointment = new FreeAppointments
                            {
                                time = PivotTime,
                                Room_ID = schedule.Room_ID
                            };
                            AppointmentsToDisplay.Add(appointment);
                        }

                        PivotTime = PivotTime.AddMinutes(10);
                    }
                }
            }
        }

        #endregion Update Fields

        #region Utility

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        #endregion Utility

        #region Buttons

        private void button_FindNumberID_Click(object sender, EventArgs e)
        {
            textBox_DisplayPatient.Text = "";

            if (IsDigitsOnly(textBox_NumberID.Text) == false || textBox_NumberID.Text.Length != 11)
            {
                MessageBox.Show("Niepoprawny numer pesel!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Patients.ToList().Exists(x => x.NumberID == textBox_NumberID.Text))
            {
                ChosenPatient = Patients.ToList().Find(x => x.NumberID == textBox_NumberID.Text);
                textBox_DisplayPatient.Text = ChosenPatient.ToString();
            }
            else
            {
                DialogResult result = MessageBox.Show("Pacjent nie znajduje się w bazie.\nChcesz dodać pacjenta?", "Informacja", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    using (AddPatient AddPatientDialog = new AddPatient(textBox_NumberID.Text))
                    {
                        var Result = AddPatientDialog.ShowDialog();

                        Patients = DataAccess.LoadPatients();
                        textBox_DisplayPatient.Text = AddPatientDialog.NewPatient.ToString();
                        ChosenPatient = AddPatientDialog.NewPatient;
                    }
                }
            }
        }

        private void button_AddAppointment_Click(object sender, EventArgs e)
        {
            if (ChosenPatient != null && listBox1.SelectedIndices.Count > 0)
            {
                AppointmentModel NewAppointment = new AppointmentModel
                {
                    Patient_ID = ChosenPatient.ID,
                    Doctor_ID = ((DoctorModel)comboBox2.SelectedItem).ID,
                    DateAndTime = ((FreeAppointments)listBox1.SelectedItem).time,
                    Room_ID = ((FreeAppointments)listBox1.SelectedItem).Room_ID
                };

                DataAccess.InsertAppointment(NewAppointment);
                MessageBox.Show("Wizyta została dodana pomyślnie!\nNumer gabinetu - " + Rooms.ToList().Find(x => x.ID == NewAppointment.Room_ID).Number + ".", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Dane nie są kompletne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Buttons
    }
}