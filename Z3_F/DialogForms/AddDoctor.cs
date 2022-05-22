using Data;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddDoctor : Form
    {
        private BindingList<SpecializationModel> AllSpecializations;
        private BindingList<SpecializationModel> SelectedSpecializations;

        public AddDoctor()
        {
            InitializeComponent();

            AllSpecializations = DataAccess.LoadSpecializations();
            specializationModelBindingSource.DataSource = AllSpecializations;

            SelectedSpecializations = new BindingList<SpecializationModel>();
            specializationModelBindingSource1.DataSource = SelectedSpecializations;

            numericUpDown_Year.Value = DateTime.Now.Year;
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (!SelectedSpecializations.Contains((SpecializationModel)listBox_Available.SelectedItem))
            {
                SelectedSpecializations.Add((SpecializationModel)listBox_Available.SelectedItem);
                AllSpecializations.Remove((SpecializationModel)listBox_Available.SelectedItem);
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (listBox_Selected.SelectedItems.Count > 0)
            {
                AllSpecializations.Add((SpecializationModel)listBox_Selected.SelectedItem);
                SelectedSpecializations.Remove((SpecializationModel)listBox_Selected.SelectedItem);
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (textBox_FirstName.Text != "" && textBox_LastName.Text != "" && listBox_Selected.Items.Count > 0)
            {
                DoctorModel NewDoctor = new DoctorModel
                {
                    FirstName = textBox_FirstName.Text,
                    LastName = textBox_LastName.Text,
                    BeganWorkYear = (int)numericUpDown_Year.Value,
                    Specializations = SelectedSpecializations
                };

                DataAccess.InsertDoctor(NewDoctor);
                MessageBox.Show("Pomyślnie dodano pracownika!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Dane są niekompletne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}