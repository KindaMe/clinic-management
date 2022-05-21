namespace Z3_F.DialogForms
{
    partial class AddAppointment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.doctorModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.specializationModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_DisplayPatient = new System.Windows.Forms.TextBox();
            this.button_FindNumberID = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_NumberID = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.freeAppointmentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button_AddAppointment = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doctorModelBindingSource)).BeginInit();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.specializationModelBindingSource)).BeginInit();
            this.groupBox14.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.freeAppointmentsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.comboBox2);
            this.groupBox16.Location = new System.Drawing.Point(12, 179);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(306, 52);
            this.groupBox16.TabIndex = 8;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Dostępny lekarz";
            // 
            // comboBox2
            // 
            this.comboBox2.DataSource = this.doctorModelBindingSource;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(6, 19);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(294, 21);
            this.comboBox2.TabIndex = 0;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // doctorModelBindingSource
            // 
            this.doctorModelBindingSource.DataSource = typeof(Data.DoctorModel);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.comboBox1);
            this.groupBox15.Location = new System.Drawing.Point(12, 121);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(306, 52);
            this.groupBox15.TabIndex = 7;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Specjalista";
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.specializationModelBindingSource;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(294, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // specializationModelBindingSource
            // 
            this.specializationModelBindingSource.DataSource = typeof(Data.SpecializationModel);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label2);
            this.groupBox14.Controls.Add(this.textBox_DisplayPatient);
            this.groupBox14.Controls.Add(this.button_FindNumberID);
            this.groupBox14.Controls.Add(this.label1);
            this.groupBox14.Controls.Add(this.textBox_NumberID);
            this.groupBox14.Location = new System.Drawing.Point(12, 12);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(306, 103);
            this.groupBox14.TabIndex = 6;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Pacjent";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Wybrano:";
            // 
            // textBox_DisplayPatient
            // 
            this.textBox_DisplayPatient.Enabled = false;
            this.textBox_DisplayPatient.Location = new System.Drawing.Point(65, 74);
            this.textBox_DisplayPatient.Name = "textBox_DisplayPatient";
            this.textBox_DisplayPatient.Size = new System.Drawing.Size(235, 20);
            this.textBox_DisplayPatient.TabIndex = 5;
            // 
            // button_FindNumberID
            // 
            this.button_FindNumberID.Location = new System.Drawing.Point(65, 45);
            this.button_FindNumberID.Name = "button_FindNumberID";
            this.button_FindNumberID.Size = new System.Drawing.Size(235, 23);
            this.button_FindNumberID.TabIndex = 4;
            this.button_FindNumberID.Text = "Wyszukaj/Dodaj";
            this.button_FindNumberID.UseVisualStyleBackColor = true;
            this.button_FindNumberID.Click += new System.EventHandler(this.button_FindNumberID_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pesel:";
            // 
            // textBox_NumberID
            // 
            this.textBox_NumberID.Location = new System.Drawing.Point(65, 19);
            this.textBox_NumberID.MaxLength = 11;
            this.textBox_NumberID.Name = "textBox_NumberID";
            this.textBox_NumberID.Size = new System.Drawing.Size(235, 20);
            this.textBox_NumberID.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.monthCalendar1);
            this.groupBox1.Location = new System.Drawing.Point(46, 237);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 183);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kalendarz";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(6, 16);
            this.monthCalendar1.MaxSelectionCount = 1;
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Location = new System.Drawing.Point(324, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(210, 408);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wolne godziny";
            // 
            // listBox1
            // 
            this.listBox1.DataSource = this.freeAppointmentsBindingSource;
            this.listBox1.DisplayMember = "time";
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(204, 389);
            this.listBox1.TabIndex = 0;
            // 
            // freeAppointmentsBindingSource
            // 
            this.freeAppointmentsBindingSource.DataSource = typeof(Data.FreeAppointments);
            // 
            // button_AddAppointment
            // 
            this.button_AddAppointment.Location = new System.Drawing.Point(12, 427);
            this.button_AddAppointment.Name = "button_AddAppointment";
            this.button_AddAppointment.Size = new System.Drawing.Size(306, 23);
            this.button_AddAppointment.TabIndex = 11;
            this.button_AddAppointment.Text = "Zatwierdź";
            this.button_AddAppointment.UseVisualStyleBackColor = true;
            this.button_AddAppointment.Click += new System.EventHandler(this.button_AddAppointment_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(324, 427);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(210, 23);
            this.button_Cancel.TabIndex = 12;
            this.button_Cancel.Text = "Anuluj";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // AddAppointment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 455);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_AddAppointment);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox16);
            this.Controls.Add(this.groupBox15);
            this.Controls.Add(this.groupBox14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddAppointment";
            this.Text = "Nowa Wizyta";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddAppointment_FormClosing);
            this.groupBox16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.doctorModelBindingSource)).EndInit();
            this.groupBox15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.specializationModelBindingSource)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.freeAppointmentsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.TextBox textBox_NumberID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.BindingSource doctorModelBindingSource;
        private System.Windows.Forms.BindingSource specializationModelBindingSource;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button_AddAppointment;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.BindingSource freeAppointmentsBindingSource;
        private System.Windows.Forms.Button button_FindNumberID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_DisplayPatient;
    }
}