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
            NewPatient = new PatientModel();
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
                DataAccess.InsertPatient(NewPatient);
                MessageBox.Show("Dodano nowego pacjenta!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Dane są niepoprawne!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}