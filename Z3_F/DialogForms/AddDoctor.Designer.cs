namespace Z3_F.DialogForms
{
    partial class AddDoctor
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_FirstName = new System.Windows.Forms.TextBox();
            this.textBox_LastName = new System.Windows.Forms.TextBox();
            this.numericUpDown_Year = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox_Selected = new System.Windows.Forms.ListBox();
            this.specializationModelBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.button_Add = new System.Windows.Forms.Button();
            this.listBox_Available = new System.Windows.Forms.ListBox();
            this.specializationModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button_Delete = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Year)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.specializationModelBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.specializationModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_FirstName);
            this.groupBox2.Controls.Add(this.textBox_LastName);
            this.groupBox2.Controls.Add(this.numericUpDown_Year);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(351, 111);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dane osobowe";
            // 
            // textBox_FirstName
            // 
            this.textBox_FirstName.Location = new System.Drawing.Point(68, 22);
            this.textBox_FirstName.Name = "textBox_FirstName";
            this.textBox_FirstName.Size = new System.Drawing.Size(158, 20);
            this.textBox_FirstName.TabIndex = 0;
            // 
            // textBox_LastName
            // 
            this.textBox_LastName.Location = new System.Drawing.Point(68, 48);
            this.textBox_LastName.Name = "textBox_LastName";
            this.textBox_LastName.Size = new System.Drawing.Size(158, 20);
            this.textBox_LastName.TabIndex = 1;
            // 
            // numericUpDown_Year
            // 
            this.numericUpDown_Year.Location = new System.Drawing.Point(131, 74);
            this.numericUpDown_Year.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDown_Year.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numericUpDown_Year.Name = "numericUpDown_Year";
            this.numericUpDown_Year.Size = new System.Drawing.Size(95, 20);
            this.numericUpDown_Year.TabIndex = 2;
            this.numericUpDown_Year.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Imię:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Rok rozpoczęcia pracy:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Nazwisko:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox_Selected);
            this.groupBox1.Controls.Add(this.button_Add);
            this.groupBox1.Controls.Add(this.listBox_Available);
            this.groupBox1.Controls.Add(this.button_Delete);
            this.groupBox1.Location = new System.Drawing.Point(12, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 135);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Specjalizacje";
            // 
            // listBox_Selected
            // 
            this.listBox_Selected.DataSource = this.specializationModelBindingSource1;
            this.listBox_Selected.DisplayMember = "Name";
            this.listBox_Selected.FormattingEnabled = true;
            this.listBox_Selected.Location = new System.Drawing.Point(225, 19);
            this.listBox_Selected.Name = "listBox_Selected";
            this.listBox_Selected.Size = new System.Drawing.Size(120, 108);
            this.listBox_Selected.TabIndex = 2;
            // 
            // specializationModelBindingSource1
            // 
            this.specializationModelBindingSource1.DataSource = typeof(Data.SpecializationModel);
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(132, 48);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(87, 23);
            this.button_Add.TabIndex = 1;
            this.button_Add.Text = ">>";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // listBox_Available
            // 
            this.listBox_Available.DataSource = this.specializationModelBindingSource;
            this.listBox_Available.DisplayMember = "Name";
            this.listBox_Available.FormattingEnabled = true;
            this.listBox_Available.Location = new System.Drawing.Point(6, 19);
            this.listBox_Available.Name = "listBox_Available";
            this.listBox_Available.Size = new System.Drawing.Size(120, 108);
            this.listBox_Available.TabIndex = 0;
            // 
            // specializationModelBindingSource
            // 
            this.specializationModelBindingSource.DataSource = typeof(Data.SpecializationModel);
            // 
            // button_Delete
            // 
            this.button_Delete.Location = new System.Drawing.Point(132, 77);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(87, 23);
            this.button_Delete.TabIndex = 3;
            this.button_Delete.Text = "<<";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(253, 268);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(110, 23);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "Anuluj";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(12, 268);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(235, 23);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "Zapisz";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // AddDoctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 302);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddDoctor";
            this.Text = "Nowy Lekarz";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Year)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.specializationModelBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.specializationModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_FirstName;
        private System.Windows.Forms.TextBox textBox_LastName;
        private System.Windows.Forms.NumericUpDown numericUpDown_Year;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox_Selected;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.ListBox listBox_Available;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.BindingSource specializationModelBindingSource;
        private System.Windows.Forms.BindingSource specializationModelBindingSource1;
    }
}