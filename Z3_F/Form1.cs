using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data;

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
                    }
                }
            }
        }

        #endregion Local Data Storage

        #region Tab_Schedule

        private bool ChangeDisabled = false;

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

            foreach (string text in Hours)
                dataGridView_schedule.Rows.Add(text);

            AddMissingColumns();
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
                for (int i = 1; i < row.Cells.Count; i++)
                    row.Cells[i].Style.BackColor = Color.White;

            ReadSchedule();

            dataGridView_schedule.ClearSelection();
            ChangeDisabled = false;
        }

        private void dataGridView_schedule_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell.ColumnIndex != 0 && e.Cell.Selected == true && ChangeDisabled == false)
            {
                if (e.Cell.Style.BackColor == Color.Green)
                {
                    e.Cell.Style.BackColor = Color.White;
                    e.Cell.Style.SelectionBackColor = Color.White;
                }
                else
                {
                    e.Cell.Style.BackColor = Color.Green;
                    e.Cell.Style.SelectionBackColor = Color.Green;
                }
            }
        }

        private void dataGridView_schedule_MouseLeave(object sender, EventArgs e)
        {
            dataGridView_schedule.ClearSelection();
        }

        private void dataGridView_schedule_MouseUp(object sender, MouseEventArgs e)
        {
            dataGridView_schedule.ClearSelection();
        }

        #endregion Tab_Schedule
    }
}