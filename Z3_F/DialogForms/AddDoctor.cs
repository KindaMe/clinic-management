using Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddDoctor : Form
    {
        private BindingList<SpecializationModel> AllSpecializations;
        private BindingList<SpecializationModel> SelectedSpecializations;

        private DoctorModel DoctorToEdit;

        public AddDoctor()
        {
            InitializeComponent();

            AllSpecializations = DataAccess.LoadSpecializations();
            specializationModelBindingSource.DataSource = AllSpecializations;

            SelectedSpecializations = new BindingList<SpecializationModel>();
            specializationModelBindingSource1.DataSource = SelectedSpecializations;

            numericUpDown_Year.Value = DateTime.Now.Year;
        }

        public AddDoctor(DoctorModel DoctorToEdit)
        {
            InitializeComponent();

            this.DoctorToEdit = DoctorToEdit;

            this.Text = "Edytuj Lekarza";
            button_Save.Text = "Zapisz";

            textBox_FirstName.Text = DoctorToEdit.FirstName;
            textBox_LastName.Text = DoctorToEdit.LastName;
            numericUpDown_Year.Value = DoctorToEdit.BeganWorkYear;

            AllSpecializations = DataAccess.LoadSpecializations();
            SelectedSpecializations = new BindingList<SpecializationModel>();

            foreach (var DoctorSpecialization in DoctorToEdit.Specializations)
            {
                if (AllSpecializations.ToList().Exists(x => x.ID == DoctorSpecialization.ID))
                {
                    var temp = AllSpecializations.ToList().Find(x => x.ID == DoctorSpecialization.ID);

                    SelectedSpecializations.Add(temp);
                    AllSpecializations.Remove(temp);
                }
            }

            specializationModelBindingSource.DataSource = AllSpecializations;
            specializationModelBindingSource1.DataSource = SelectedSpecializations;
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
                if (DoctorToEdit != null)
                {
                    DoctorModel EditedDoctor = new DoctorModel
                    {
                        ID = DoctorToEdit.ID,
                        FirstName = textBox_FirstName.Text,
                        LastName = textBox_LastName.Text,
                        BeganWorkYear = (int)numericUpDown_Year.Value,
                        Specializations = SelectedSpecializations
                    };

                    DataAccess.UpdateDoctor(EditedDoctor);
                    MessageBox.Show("Pracownik został zaktualizowany pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    DoctorModel NewDoctor = new DoctorModel
                    {
                        FirstName = textBox_FirstName.Text,
                        LastName = textBox_LastName.Text,
                        BeganWorkYear = (int)numericUpDown_Year.Value,
                        Specializations = SelectedSpecializations
                    };

                    DataAccess.InsertDoctor(NewDoctor);
                    MessageBox.Show("Pracownik został dodany pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Dane nie są kompletne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}