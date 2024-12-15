using clinic_api_project.models;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinic_api_project.DTO
{
    public class MedicalRecordDTO
    {
        
        public string DoctorID { set; get; }
    
        public string patientID { set; get; }
        //تشخيص
        public string Diagnosis { set; get; }
        //روشته
        public string Prescription { set; get; }
        
    }
}
