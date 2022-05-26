using Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddSpecialization : Form
    {
        private BindingList<SpecializationModel> TakenSpecializations;
        private SpecializationModel SpecializationToEdit;

        public AddSpecialization()
        {
            InitializeComponent();
            TakenSpecializations = DataAccess.LoadSpecializations();
        }

        public AddSpecialization(SpecializationModel SpecializationToEdit)
        {
            InitializeComponent();
            TakenSpecializations = DataAccess.LoadSpecializations();

            this.Text = "Edytuj Specializacje";
            button_Add.Text = "Zapisz";

            this.SpecializationToEdit = SpecializationToEdit;
            textBox1.Text = SpecializationToEdit.Name;
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (SpecializationToEdit != null)
            {
                if ((!TakenSpecializations.ToList().Exists(x => x.Name == textBox1.Text) || textBox1.Text == SpecializationToEdit.Name) && !(textBox1.Text == ""))
                {
                    SpecializationModel EditedSpecialization = new SpecializationModel
                    {
                        ID = SpecializationToEdit.ID,
                        Name = textBox1.Text
                    };
                    DataAccess.UpdateSpecialization(EditedSpecialization);
                    MessageBox.Show("Specjalizacja została zaktualizowana pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wybrana nazwa jest błędna lub zajęta!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                if (!TakenSpecializations.ToList().Exists(x => x.Name == textBox1.Text) && !(textBox1.Text == ""))
                {
                    SpecializationModel NewSpecialization = new SpecializationModel
                    {
                        Name = textBox1.Text
                    };

                    DataAccess.InsertSpecialization(NewSpecialization);
                    MessageBox.Show("Specjalizacja została dodana pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wybrana nazwa jest błędna lub zajęta!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}