using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data;

namespace Z3_F
{
    public partial class Form1 : Form
    {
        #region LocalDataStorage

        private BindingList<DoctorModel> Doctors;
        private BindingList<SpecializationModel> Specializations;
        private BindingList<PatientModel> Patients;
        private BindingList<WorkScheduleModel> Schedule;

        private void ReadDoctors()
        {
            Doctors = DataAccess.LoadDoctors();
            doctorModelBindingSource.DataSource = Doctors;
            doctorModelBindingSource_schedule.DataSource = Doctors;
        }

        private void ReadSpecializations()
        {
            Specializations = DataAccess.LoadSpecializations();
            specializationModelBindingSource.DataSource = Specializations;
        }

        private void ReadPatients()
        {
            Patients = DataAccess.LoadPatients();
            patientModelBindingSource.DataSource = Patients;
        }

        private void ReadSchedule()
        {
            Schedule = DataAccess.LoadSchedule();
        }

        #endregion LocalDataStorage

        public Form1()
        {
            InitializeComponent();
            ReadDoctors();
            ReadSpecializations();
            ReadPatients();
            ReadSchedule();
        }
    }
}