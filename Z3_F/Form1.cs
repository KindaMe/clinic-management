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

            //improves datagridview performance
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView_schedule, new object[] { true });
            InitSchedule();
            ReadSchedule();
        }

        #region Local Data Storage

        private BindingList<DoctorModel> Doctors;
        private BindingList<SpecializationModel> Specializations;
        private BindingList<PatientModel> Patients;
        private BindingList<RoomModel> Rooms;
        private BindingList<WorkScheduleModel> Schedule;

        private void ReadDoctors()
        {
            Doctors = DataAccess.LoadDoctors();
            doctorModelBindingSource.DataSource = Doctors;
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

        private void ReadSchedule()
        {
            Schedule = DataAccess.LoadSchedule();

            AddMissingColumns();

            foreach (WorkScheduleModel schedule in Schedule)
            {
                if (monthCalendar_schedule.SelectionStart.Date.ToShortDateString() == schedule.Date.ToShortDateString())
                {
                    DoctorModel tempDoc = Doctors.ToList().Find(x => x.ID == schedule.Doctor_ID);
                    int colIndex = dataGridView_schedule.Columns[tempDoc.ID.ToString()].Index;

                    int StartingHour = schedule.TimeStart.Hour;
                    int EndingHour = schedule.TimeEnd.Hour;

                    for (int i = StartingHour; i < EndingHour; i++)
                    {
                        dataGridView_schedule.Rows[i].Cells[colIndex].Style.BackColor = Color.Green;
                        dataGridView_schedule.Rows[i].Cells[colIndex].Style.ForeColor = Color.White;
                        dataGridView_schedule.Rows[i].Cells[colIndex].Value = Rooms.ToList().Find(x => x.ID == schedule.Room_ID).Number;
                    }
                }
            }
        }

        #endregion Local Data Storage

        #region Tab_Schedule

        private bool ChangeDisabled = false;
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

            AddMissingColumns();

            dataGridView_schedule.Rows.Add(Hours.Count);

            for (int i = 0; i < dataGridView_schedule.Rows.Count; i++)
            {
                dataGridView_schedule.Rows[i].HeaderCell.Value = Hours[i];
            }
        }

        private void AddMissingColumns()
        {
            foreach (DoctorModel doc in Doctors)
            {
                if (dataGridView_schedule.Columns.Contains(doc.ID.ToString()))
                    continue;

                int tempColIndex = dataGridView_schedule.Columns.Add(doc.ID.ToString(), doc.ToString() + " (ID:" + doc.ID + ")");
                dataGridView_schedule.Columns[tempColIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void monthCalendar_schedule_DateChanged(object sender, DateRangeEventArgs e)
        {
            ChangeDisabled = true;

            foreach (DataGridViewRow row in dataGridView_schedule.Rows)
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    row.Cells[i].Style.BackColor = Color.White;
                    row.Cells[i].Value = null;
                }

            ReadSchedule();

            dataGridView_schedule.ClearSelection();
            ChangeDisabled = false;
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

        private void dataGridView_schedule_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

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
                int newDoctor_ID = int.Parse(Doctors[dataGridView_schedule.SelectedCells[0].ColumnIndex].ID.ToString());

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
                DateTime newTimeEnd = new DateTime(
                monthCalendar_schedule.SelectionStart.Date.Year,
                monthCalendar_schedule.SelectionStart.Date.Month,
                monthCalendar_schedule.SelectionStart.Date.Day,
                EndingHour + 1,
                0,
                0);

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
            else
            {
                return;
            }
        }

        #endregion Tab_Schedule

        #region Tab_Appointment

        //

        #endregion Tab_Appointment
    }
}