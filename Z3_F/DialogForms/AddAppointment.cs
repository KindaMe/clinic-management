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
        private bool IsEditing = false;
        private AppointmentModel AppointmentToEdit;

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

        public AddAppointment()
        {
            InitializeComponent();

            InitData();

            UpdateDoctors();
            UpdateCalendar();
            UpdateAppointments();

            ConstructorFinished = true;
        }

        public AddAppointment(AppointmentModel AppointmentToEdit)
        {
            InitializeComponent();

            IsEditing = true;
            this.AppointmentToEdit = AppointmentToEdit;

            InitData();

            ChosenPatient = Patients.ToList().Find(x => x.ID == AppointmentToEdit.ID);
            textBox_NumberID.Text = ChosenPatient.NumberID;
            textBox_DisplayPatient.Text = ChosenPatient.ToString();

            comboBox1.SelectedIndex = Specializations.ToList().FindIndex(x => x.ID == AppointmentToEdit.Specialization_ID);
            UpdateDoctors();

            comboBox2.SelectedIndex = DoctorsToDisplay.ToList().FindIndex(x => x.ID == AppointmentToEdit.Doctor_ID);

            monthCalendar1.SelectionStart = AppointmentToEdit.DateAndTime;
            UpdateAppointments();
        }

        public void InitData()
        {
            Patients = DataAccess.LoadPatients();

            Rooms = DataAccess.LoadRooms();

            Specializations = DataAccess.LoadSpecializations();
            specializationModelBindingSource.DataSource = Specializations;

            Doctors = DataAccess.LoadDoctors();

            Schedule = DataAccess.LoadSchedule();

            Appointments = DataAccess.LoadAppointments();

            AppointmentsToDisplay = new BindingList<FreeAppointments>();
            freeAppointmentsBindingSource.DataSource = AppointmentsToDisplay;
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
                if (schedule.Date > DateTime.Now)
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
                    //MessageBox.Show("Brak wolnych wizyt!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool AnyAppointmentsAvailableInSchedule(WorkScheduleModel schedule)
        {
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

        private void UpdateAppointments()
        {
            AppointmentsToDisplay.Clear();
            foreach (WorkScheduleModel schedule in Schedule)
            {
                if (schedule.Doctor_ID == ((DoctorModel)comboBox2.SelectedItem).ID && schedule.Date.ToShortDateString() == monthCalendar1.SelectionStart.ToShortDateString())
                {
                    DateTime PivotTime = schedule.TimeStart;

                    while (PivotTime < schedule.TimeStart.AddHours(1))//fix that later
                    {
                        if (!Appointments.ToList().Exists(x => x.DateAndTime.ToString() == PivotTime.ToString() && x.Doctor_ID == ((DoctorModel)comboBox2.SelectedItem).ID))
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
            if (((FreeAppointments)listBox1.SelectedItem).time > DateTime.Now)
            {
                if (ChosenPatient != null && listBox1.SelectedIndices.Count > 0)
                {
                    if (IsEditing == false)
                    {
                        AppointmentModel NewAppointment = new AppointmentModel
                        {
                            Specialization_ID = ((SpecializationModel)comboBox1.SelectedItem).ID,
                            Patient_ID = ChosenPatient.ID,
                            Doctor_ID = ((DoctorModel)comboBox2.SelectedItem).ID,
                            DateAndTime = ((FreeAppointments)listBox1.SelectedItem).time,
                            Room_ID = ((FreeAppointments)listBox1.SelectedItem).Room_ID
                        };

                        DataAccess.InsertAppointment(NewAppointment);
                        MessageBox.Show("Wizyta została dodana pomyślnie!\nNumer gabinetu - " + NewAppointment.Room + ".", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        AppointmentToEdit.Specialization_ID = ((SpecializationModel)comboBox1.SelectedItem).ID;
                        AppointmentToEdit.Patient_ID = ChosenPatient.ID;
                        AppointmentToEdit.Doctor_ID = ((DoctorModel)comboBox2.SelectedItem).ID;
                        AppointmentToEdit.DateAndTime = ((FreeAppointments)listBox1.SelectedItem).time;
                        AppointmentToEdit.Room_ID = ((FreeAppointments)listBox1.SelectedItem).Room_ID;

                        DataAccess.UpdateAppointment(AppointmentToEdit);
                        MessageBox.Show("Wizyta została zaktualizowana pomyślnie!\nNumer gabinetu - " + AppointmentToEdit.Room + ".", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Dane nie są kompletne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Wybrana wizyta już się odbyła!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                UpdateCalendar();
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Buttons
    }
}