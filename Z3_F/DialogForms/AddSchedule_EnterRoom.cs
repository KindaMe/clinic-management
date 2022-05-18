using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddSchedule_EnterRoom : Form
    {
        private int LocationX;
        private int LocationY;
        private WorkScheduleModel NewSchedule;
        private List<RoomModel> FreeRooms;

        public AddSchedule_EnterRoom(int CursorX, int CursorY, WorkScheduleModel NewSchedule, List<RoomModel> FreeRooms)
        {
            InitializeComponent();

            LocationX = CursorX;
            LocationY = CursorY;
            this.NewSchedule = NewSchedule;
            this.FreeRooms = FreeRooms;
            comboBox1.DataSource = FreeRooms;
        }

        private void AddSchedule_EnterRoom_Load(object sender, EventArgs e)
        {
            SetDesktopLocation(LocationX, LocationY);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewSchedule.Room_ID = ((RoomModel)comboBox1.SelectedItem).ID;

            //prolly should just delete all schedules for a doctor in a currently chosen day and run a loop through rows to make new schedules for connected hours
            DataAccess.AddSchedule(NewSchedule);

            this.Close();
        }
    }
}