using Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddSchedule_EnterRoom : Form
    {
        public int ChosenRoom { get; set; }

        private int LocationX;
        private int LocationY;
        private List<RoomModel> FreeRooms;

        public AddSchedule_EnterRoom(int CursorX, int CursorY, List<RoomModel> FreeRooms)
        {
            InitializeComponent();

            LocationX = CursorX;
            LocationY = CursorY;
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
            ChosenRoom = ((RoomModel)comboBox1.SelectedItem).ID;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}