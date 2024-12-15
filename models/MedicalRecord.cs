using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinic_api_project.models
{
    public class MedicalRecord
    {
        [Key]
        public int Id { set; get; }
        [ForeignKey("Doctor")]
        public string DoctorID { set; get; }
        public virtual UserApplication Doctor { set; get; }
        [ForeignKey("patient")]
        public string patientID { set; get; }
       
        public virtual UserApplication patient { set; get; }
        //تشخيص
        public string Diagnosis { set; get; }
        //روشته
        public string Prescription { set; get; }
        public virtual DateTime DateCreated { get; set; }

    }
}
