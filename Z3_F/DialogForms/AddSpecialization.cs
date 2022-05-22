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

        public AddSpecialization()
        {
            InitializeComponent();
            TakenSpecializations = DataAccess.LoadSpecializations();
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (TakenSpecializations.ToList().Exists(x => x.Name == textBox1.Text) || textBox1.Text == "")
            {
                MessageBox.Show("Wybrana nazwa jest błędna lub zajęta!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                SpecializationModel NewSpecialization = new SpecializationModel
                {
                    Name = textBox1.Text
                };

                DataAccess.InsertSpecialization(NewSpecialization);
                MessageBox.Show("Specjalizacja została dodana pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}