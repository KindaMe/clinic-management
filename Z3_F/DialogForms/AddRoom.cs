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
        private RoomModel RoomToEdit;

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

        public AddRoom(RoomModel RoomToEdit)
        {
            InitializeComponent();
            TakenRoomNumbers = DataAccess.LoadRooms();

            this.Text = "Edytuj Gabinet";
            button_Add.Text = "Zapisz";

            this.RoomToEdit = RoomToEdit;
            numericUpDown1.Value = RoomToEdit.Number;
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (RoomToEdit != null)
            {
                if (numericUpDown1.Value == RoomToEdit.Number || !TakenRoomNumbers.ToList().Exists(x => x.Number == (int)numericUpDown1.Value))
                {
                    RoomModel EditedRoom = new RoomModel
                    {
                        ID = RoomToEdit.ID,
                        Number = (int)numericUpDown1.Value
                    };
                    DataAccess.UpdateRoom(EditedRoom);
                    MessageBox.Show("Gabinet został zaktualizowany pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wybrany numer pokoju już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                if (!TakenRoomNumbers.ToList().Exists(x => x.Number == (int)numericUpDown1.Value))
                {
                    RoomModel NewRoom = new RoomModel
                    {
                        Number = (int)numericUpDown1.Value
                    };
                    DataAccess.InsertRoom(NewRoom);
                    MessageBox.Show("Gabinet został dodany pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wybrany numer pokoju już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}