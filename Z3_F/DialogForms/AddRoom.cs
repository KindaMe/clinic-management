using Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddRoom : Form
    {
        private BindingList<RoomModel> TakenRoomNumbers;

        public AddRoom()
        {
            InitializeComponent();
            TakenRoomNumbers = DataAccess.LoadRooms();

            int FreeNumber = 1;

            while (true)
            {
                if (!TakenRoomNumbers.ToList().Exists(x => x.Number == FreeNumber))
                {
                    numericUpDown1.Value = FreeNumber;
                    break;
                }
                else
                {
                    FreeNumber++;
                }
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (!TakenRoomNumbers.ToList().Exists(x => x.Number == (int)numericUpDown1.Value))
            {
                RoomModel NewRoom = new RoomModel
                {
                    Number = (int)numericUpDown1.Value
                };
                DataAccess.InsertRoom(NewRoom);
                MessageBox.Show("Dodano nowy gabinet!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Wybrany numer pokoju już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}