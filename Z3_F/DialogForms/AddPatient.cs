using Data;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddPatient : Form
    {
        public PatientModel NewPatient { get; set; }
        private bool IsEditing = false;

        public AddPatient()
        {
            InitializeComponent();
            NewPatient = new PatientModel
            {
                BornYear = (int)numericUpDown1.Value,
                AuthorizedPeople = new BindingList<AuthorizedPersonModel>()
            };

            PatientModelBindingSource.DataSource = NewPatient;
        }

        public AddPatient(PatientModel PatientToEdit)
        {
            IsEditing = true;

            InitializeComponent();

            BindingList<AuthorizedPersonModel> CopyAuthorizedPeople = new BindingList<AuthorizedPersonModel>();
            foreach (AuthorizedPersonModel authorizedPerson in PatientToEdit.AuthorizedPeople)
            {
                CopyAuthorizedPeople.Add(new AuthorizedPersonModel
                {
                    ID = authorizedPerson.ID,
                    FirstName = authorizedPerson.FirstName,
                    LastName = authorizedPerson.LastName,
                    HealthState = authorizedPerson.HealthState,
                    MedicalDocumentation = authorizedPerson.MedicalDocumentation,
                });
            }

            NewPatient = new PatientModel
            {
                ID = PatientToEdit.ID,
                LastName = PatientToEdit.LastName,
                FirstName = PatientToEdit.FirstName,
                NumberPhone = PatientToEdit.NumberPhone,
                NumberID = PatientToEdit.NumberID,
                BornYear = PatientToEdit.BornYear,
                Address = PatientToEdit.Address,
                Email = PatientToEdit.Email,
                AuthorizedPeople = CopyAuthorizedPeople
            };

            PatientModelBindingSource.DataSource = NewPatient;
        }

        public AddPatient(string NumberID) : this()
        {
            NewPatient.NumberID = NumberID;
        }

        #region Patient

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                if (IsEditing == false)
                {
                    NewPatient.ID = DataAccess.InsertPatient(NewPatient);
                    MessageBox.Show("Pacjent został dodany pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    DataAccess.UpdatePatient(NewPatient);
                    MessageBox.Show("Pacjent został zaktualizowany pomyślnie!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Dane nie są kompletne!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion Patient

        #region Authorized People

        private void button_Add_Click(object sender, EventArgs e)
        {
            NewPatient.AuthorizedPeople.Insert(0, new AuthorizedPersonModel());
            authorizedPeopleBindingSource.Position = 0;
            BoxEnabler();
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (NewPatient.AuthorizedPeople.Count > 0)
            {
                NewPatient.AuthorizedPeople.Remove((AuthorizedPersonModel)dataGridView1.SelectedRows[0].DataBoundItem);
                dataGridView1.ClearSelection();

                if (dataGridView1.Rows.Count > 0)
                    dataGridView1.Rows[authorizedPeopleBindingSource.Position].Selected = true;

                BoxEnabler();
            }
        }

        private void BoxEnabler()
        {
            if (NewPatient.AuthorizedPeople.Count > 0)
            {
                label8.Enabled = true;
                label9.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
            }
            else
            {
                label8.Enabled = false;
                label9.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
            }
        }

        #endregion Authorized People
    }
}