using Data;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Z3_F.DialogForms
{
    public partial class AddPatient_Authorized : Form
    {
        public BindingList<AuthorizedPersonModel> AuthorizedPeople { get; set; }

        public AddPatient_Authorized()
        {
            InitializeComponent();
            AuthorizedPeople = new BindingList<AuthorizedPersonModel>();
            authorizedPersonModelBindingSource.DataSource = AuthorizedPeople;
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            AuthorizedPeople.AddNew();
            BoxEnabler();
        }

        private void BoxEnabler()
        {
            if (AuthorizedPeople.Count > 0)
            {
                label1.Enabled = true;
                label2.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
            }
            else
            {
                label1.Enabled = false;
                label2.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (AuthorizedPeople.Count > 0)
            {
                AuthorizedPeople.Remove((AuthorizedPersonModel)dataGridView1.SelectedRows[0].DataBoundItem);
                BoxEnabler();
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            AuthorizedPeople.Clear();//this sucks cuz its not triggering on closing with x
            this.Close();
        }
    }
}