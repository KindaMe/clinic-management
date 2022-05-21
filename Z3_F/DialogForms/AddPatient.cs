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
    public partial class AddPatient : Form
    {
        public PatientModel NewPatient { get; set; }

        public AddPatient()
        {
            InitializeComponent();
            NewPatient = new PatientModel
            {
                BornYear = (int)numericUpDown1.Value
            };

            PatientModelBindingSource.DataSource = NewPatient;
        }

        public AddPatient(string NumberID) : this()
        {
            NewPatient.NumberID = NumberID;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                NewPatient.ID = DataAccess.InsertPatient(NewPatient);
                MessageBox.Show("Dodano nowego pacjenta!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Dane są niepoprawne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button_AddAuthorized_Click(object sender, EventArgs e)
        {
            using (AddPatient_Authorized AddAuthorizedDialog = new AddPatient_Authorized())
            {
                AddAuthorizedDialog.ShowDialog();
                NewPatient.AuthorizedPeople = AddAuthorizedDialog.AuthorizedPeople;
                PatientModelBindingSource.ResetBindings(true);
            }
        }
    }
}