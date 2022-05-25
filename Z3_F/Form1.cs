using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Z3_F.DialogForms;

namespace Z3_F
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ReadDoctors();

            ReadSpecializations();

            ReadPatients();

            ReadRooms();

            ReadAppointment();

            InitSchedule();
            ReadSchedule();

            //improves datagridview performance
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView_schedule, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView_Appointments, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView_Doctors, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView_DoctorsSpecializations, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView2, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView3, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView_Specializations, new object[] { true });
        }

        #region Main Form

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            listBox_Appointment_Doctors.SelectedIndexChanged -= listBox_Appointment_Doctors_SelectedIndexChanged;
            monthCalendar_Appointment.DateChanged -= monthCalendar_Appointment_DateChanged;
        }

        #endregion Main Form

        #region Local Data Storage

        private BindingList<DoctorModel> Doctors;
        private BindingList<SpecializationModel> Specializations;
        private BindingList<PatientModel> Patients;
        private BindingList<RoomModel> Rooms;
        private BindingList<WorkScheduleModel> Schedule;
        private BindingList<AppointmentView> AppointmentView;

        private void ReadDoctors()
        {
            Doctors = DataAccess.LoadDoctors();
            doctorModelBindingSource.DataSource = Doctors;
            doctorModelBindingSource1.DataSource = Doctors;
        }

        private void ReadSpecializations()
        {
            Specializations = DataAccess.LoadSpecializations();
            specializationModelBindingSource.DataSource = Specializations;
        }

        private void ReadPatients()
        {
            Patients = DataAccess.LoadPatients();
            patientModelBindingSource.DataSource = Patients;
        }

        private void ReadRooms()
        {
            Rooms = DataAccess.LoadRooms();
            roomModelBindingSource.DataSource = Rooms;
        }

        private void ReadSchedule()//needs a lil remake
        {
            Schedule = DataAccess.LoadSchedule();

            foreach (DataGridViewRow row in dataGridView_schedule.Rows)
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    row.Cells[i].Style.BackColor = Color.White;
                    row.Cells[i].Value = null;
                }

            CheckMissingColumns();

            foreach (WorkScheduleModel schedule in Schedule)
            {
                if (monthCalendar_schedule.SelectionStart.Date.ToShortDateString() == schedule.Date.ToShortDateString())
                {
                    DoctorModel tempDoc = Doctors.ToList().Find(x => x.ID == schedule.Doctor_ID);
                    int colIndex = dataGridView_schedule.Columns[tempDoc.ID.ToString()].Index;

                    int StartingHour = schedule.TimeStart.Hour;
                    int EndingHour;
                    if (schedule.TimeEnd.Minute == 59)
                    {
                        EndingHour = 24;
                    }
                    else
                    {
                        EndingHour = schedule.TimeEnd.Hour;
                    }

                    for (int i = StartingHour; i < EndingHour; i++)
                    {
                        dataGridView_schedule.Rows[i].Cells[colIndex].Style.BackColor = Color.Green;
                        dataGridView_schedule.Rows[i].Cells[colIndex].Style.ForeColor = Color.White;
                        dataGridView_schedule.Rows[i].Cells[colIndex].Value = Rooms.ToList().Find(x => x.ID == schedule.Room_ID).Number;
                    }
                }
            }
        }

        private void ReadAppointment()
        {
            AppointmentView = DataAccess.LoadAppointmentView(((DoctorModel)listBox_Appointment_Doctors.SelectedItem).ID, monthCalendar_Appointment.SelectionStart);
            appointmentViewBindingSource.DataSource = AppointmentView;
        }

        #endregion Local Data Storage

        #region Tab_Schedule

        private int CurrentColumn = -1;

        private void InitSchedule()
        {
            List<string> Hours = new List<string>
            {
               "00:00 - 01:00", "01:00 - 02:00", "02:00 - 03:00", "03:00 - 04:00",
               "04:00 - 05:00", "05:00 - 06:00", "06:00 - 07:00", "07:00 - 08:00",
               "08:00 - 09:00", "09:00 - 10:00", "10:00 - 11:00", "11:00 - 12:00",
               "12:00 - 13:00", "13:00 - 14:00", "14:00 - 15:00", "15:00 - 16:00",
               "16:00 - 17:00", "17:00 - 18:00", "18:00 - 19:00", "19:00 - 20:00",
               "20:00 - 21:00", "21:00 - 22:00", "22:00 - 23:00", "23:00 - 24:00",
            };

            ReloadColumns();

            dataGridView_schedule.Rows.Add(Hours.Count);

            for (int i = 0; i < dataGridView_schedule.Rows.Count; i++)
            {
                dataGridView_schedule.Rows[i].HeaderCell.Value = Hours[i];
            }
        }

        private void DeleteScheduleColumn(DoctorModel DoctorBeingDeleted)
        {
            dataGridView_schedule.Columns.Remove(DoctorBeingDeleted.ID.ToString());
        }

        private void CheckMissingColumns()
        {
            foreach (DoctorModel doc in Doctors)
            {
                if (!dataGridView_schedule.Columns.Contains(doc.ID.ToString()))
                {
                    ReloadColumns();
                    return;
                }
            }
        }

        private void ReloadColumns()
        {
            while (dataGridView_schedule.Columns.Count > 1)
            {
                dataGridView_schedule.Columns.RemoveAt(1);
            }

            foreach (DoctorModel doc in Doctors)
            {
                int tempColIndex = dataGridView_schedule.Columns.Add(doc.ID.ToString(), doc.ToString() + " (ID:" + doc.ID + ")");
                dataGridView_schedule.Columns[tempColIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void monthCalendar_schedule_DateChanged(object sender, DateRangeEventArgs e)
        {
            ReadSchedule();

            dataGridView_schedule.ClearSelection();
        }

        private void dataGridView_schedule_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.ColumnIndex != CurrentColumn)
                e.Cell.Selected = false;
        }

        private bool IsRoomFree(RoomModel RoomToCheck, int StartingHour, int EndingHour)
        {
            for (int i = StartingHour; i <= EndingHour; i++)
            {
                foreach (DataGridViewCell cell in dataGridView_schedule.Rows[i].Cells)
                {
                    if (cell.Value != null)
                    {
                        if ((int)cell.Value == RoomToCheck.Number)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool AreAllSelectedCellsFree()
        {
            foreach (DataGridViewCell cell in dataGridView_schedule.SelectedCells)
            {
                if (cell.Value != null)
                    return false;
            }
            return true;
        }

        private void dataGridView_schedule_MouseUp(object sender, MouseEventArgs e)
        {
            if (dataGridView_schedule.SelectedCells.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (AreAllSelectedCellsFree() == false)
                    {
                        MessageBox.Show("Nie wszystkie wybrane godziny są wolne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        List<RoomModel> FreeRooms = new List<RoomModel>();
                        int StartingHour = 23;
                        foreach (DataGridViewCell cell in dataGridView_schedule.SelectedCells)
                        {
                            if (cell.RowIndex < StartingHour)
                                StartingHour = cell.RowIndex;
                        }
                        int EndingHour = StartingHour;
                        foreach (DataGridViewCell cell in dataGridView_schedule.SelectedCells)
                        {
                            if (cell.RowIndex > EndingHour)
                                EndingHour = cell.RowIndex;
                        }

                        foreach (RoomModel room in Rooms)
                        {
                            if (IsRoomFree(room, StartingHour, EndingHour) == true)
                            {
                                FreeRooms.Add(room);
                            }
                        }

                        if (FreeRooms.Count == 0)
                        {
                            MessageBox.Show("Brak wolnych gabinetów!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //int Doctor_ID
                            int newDoctor_ID = int.Parse(Doctors[dataGridView_schedule.SelectedCells[0].ColumnIndex - 1].ID.ToString());

                            //DateTime Date
                            DateTime newDate = new DateTime(
                                    monthCalendar_schedule.SelectionStart.Date.Year,
                                    monthCalendar_schedule.SelectionStart.Date.Month,
                                    monthCalendar_schedule.SelectionStart.Date.Day);

                            //DateTime TimeStart
                            DateTime newTimeStart = new DateTime(
                                    monthCalendar_schedule.SelectionStart.Date.Year,
                                    monthCalendar_schedule.SelectionStart.Date.Month,
                                    monthCalendar_schedule.SelectionStart.Date.Day,
                                    StartingHour,
                                    0,
                                    0);

                            //DateTime TimeEnd
                            DateTime newTimeEnd;
                            if (EndingHour + 1 == 24)
                            {
                                newTimeEnd = new DateTime(
                                monthCalendar_schedule.SelectionStart.Date.Year,
                                monthCalendar_schedule.SelectionStart.Date.Month,
                                monthCalendar_schedule.SelectionStart.Date.Day,
                                EndingHour,
                                59,
                                59);
                            }
                            else
                            {
                                newTimeEnd = new DateTime(
                                monthCalendar_schedule.SelectionStart.Date.Year,
                                monthCalendar_schedule.SelectionStart.Date.Month,
                                monthCalendar_schedule.SelectionStart.Date.Day,
                                EndingHour + 1,
                                0,
                                0);
                            }

                            WorkScheduleModel NewSchedule = new WorkScheduleModel
                            {
                                Doctor_ID = newDoctor_ID,
                                Date = newDate,
                                TimeStart = newTimeStart,
                                TimeEnd = newTimeEnd
                            };

                            AddSchedule_EnterRoom ChooseRoomDialog = new AddSchedule_EnterRoom(Cursor.Position.X, Cursor.Position.Y, NewSchedule, FreeRooms);
                            ChooseRoomDialog.ShowDialog();
                            ReadSchedule();
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    foreach (DataGridViewCell cell in dataGridView_schedule.SelectedCells)
                    {
                        //might need to work a lil bit more on that one xDD
                        WorkScheduleModel ScheduleToEdit = Schedule.ToList().Find(
                        x => (Doctors.ToList().FindIndex(y => y.ID == x.Doctor_ID)) == cell.ColumnIndex - 1
                        && x.Date.ToShortDateString() == monthCalendar_schedule.SelectionStart.ToShortDateString()
                        && x.TimeStart.Hour <= cell.RowIndex
                        && x.TimeEnd.Hour > cell.RowIndex);

                        if (ScheduleToEdit != null)
                        {
                            if (cell.RowIndex - ScheduleToEdit.TimeStart.Hour < ScheduleToEdit.TimeEnd.Hour - cell.RowIndex)
                            {
                                ScheduleToEdit.TimeStart = ScheduleToEdit.TimeStart.AddHours((cell.RowIndex + 1) - ScheduleToEdit.TimeStart.Hour);
                            }
                            else
                            {
                                ScheduleToEdit.TimeEnd = ScheduleToEdit.TimeEnd.AddHours(cell.RowIndex - ScheduleToEdit.TimeEnd.Hour);
                            }
                            DataAccess.UpdateSchedule(ScheduleToEdit);
                            ReadSchedule();
                        }
                    }
                }
            }

            CurrentColumn = -1;
            dataGridView_schedule.ClearSelection();
        }

        private void dataGridView_schedule_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CurrentColumn == -1)
                {
                    CurrentColumn = e.ColumnIndex;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (CurrentColumn == -1)
                {
                    CurrentColumn = e.ColumnIndex;
                }

                dataGridView_schedule.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
            else
            {
                return;
            }
        }

        #endregion Tab_Schedule

        #region Tab_Appointment

        private void button13_Click(object sender, EventArgs e)
        {
            using (AddAppointment AddAppointmentDialog = new AddAppointment(Specializations, Doctors, Schedule, Patients, Rooms))
            {
                AddAppointmentDialog.ShowDialog();
            }
            ReadAppointment();
            ReadPatients();
        }

        private void listBox_Appointment_Doctors_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadAppointment();
        }

        private void monthCalendar_Appointment_DateChanged(object sender, DateRangeEventArgs e)
        {
            ReadAppointment();
        }

        private void button_Appointment_Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView_Appointments.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Próbujesz usunąć wizytę.\nKontynuować?", "Informacja", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    DataAccess.DeleteAppointment((AppointmentView)dataGridView_Appointments.SelectedRows[0].DataBoundItem);
                    ReadAppointment();
                }
            }
        }

        #endregion Tab_Appointment

        #region Tab_Patient

        private void button_Patient_Add_Click(object sender, EventArgs e)
        {
            using (AddPatient AddPatientDialog = new AddPatient())
            {
                AddPatientDialog.ShowDialog();
            }
            ReadPatients();
        }

        private void button_Patient_Delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunięcie pacjenta spowoduje skasowanie wszystkich powiązanych osób upoważnionych oraz umówionych wizyt.\nKontynuować?", "Informacja", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                DataAccess.DeletePatient((PatientModel)dataGridView1.SelectedRows[0].DataBoundItem);
                ReadPatients();
                ReadAppointment();
            }
        }

        #endregion Tab_Patient

        #region Tab_Worker

        private void button_Worker_Add_Click(object sender, EventArgs e)
        {
            using (AddDoctor AddDoctorDialog = new AddDoctor())
            {
                AddDoctorDialog.ShowDialog();
            }
            ReadDoctors();
            ReadSchedule();
        }

        private void button_Worker_Delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunięcie pracownika spowoduje skasowanie wszystkich powiązanych dyżurów oraz umówionych wizyt.\nKontynuować?",
                "Informacja", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                DataAccess.DeleteDoctor((DoctorModel)dataGridView_Doctors.SelectedRows[0].DataBoundItem);
                DeleteScheduleColumn((DoctorModel)dataGridView_Doctors.SelectedRows[0].DataBoundItem);

                ReadDoctors();
                ReadSchedule();
                ReadAppointment();
            }
        }

        #endregion Tab_Worker

        #region Tab_Room

        private void button_Room_Add_Click(object sender, EventArgs e)
        {
            using (AddRoom AddRoomDialog = new AddRoom())
            {
                AddRoomDialog.ShowDialog();
            }
            ReadRooms();
        }

        private void button_Room_Delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunięcie gabinetu spowoduje skasowanie wszystkich powiązanych dyżurów oraz umówionych wizyt.\nKontynuować?", "Informacja", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                DataAccess.DeleteRoom((RoomModel)dataGridView3.SelectedRows[0].DataBoundItem);
                ReadRooms();
                ReadSchedule();
                ReadAppointment();
            }
        }

        #endregion Tab_Room

        #region Tab_Specialization

        private void button_Specialization_Add_Click(object sender, EventArgs e)
        {
            using (AddSpecialization AddSpecializationDialog = new AddSpecialization())
            {
                AddSpecializationDialog.ShowDialog();
            }
            ReadSpecializations();
        }

        private void button_Specialization_Delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Usunięcie specjalizacji spowoduje skasowanie wszystkich powiązanych wizyt i aktualizacje pracowników.\nKontynuować?", "Informacja", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                DataAccess.DeleteSpecialization((SpecializationModel)dataGridView_Specializations.SelectedRows[0].DataBoundItem);
                ReadSpecializations();
                ReadDoctors();
                ReadAppointment();
            }
        }

        #endregion Tab_Specialization
    }
}