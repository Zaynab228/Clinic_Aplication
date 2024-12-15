using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinic_api_project.models
{
    public class apointment
    {
        [Key]
        public int Id { set; get; }
        [ForeignKey("Doctor")]
        public string DoctorID { set; get; }
        public virtual UserApplication Doctor { set; get; }
        [ForeignKey("patient")]
        public string patientID { set; get; }
       
        public virtual UserApplication patient { set; get; }
        public DateTime DateApointment { set; get; }
        public string Status { set; get; }
        public string Notes { set; get; }
        public virtual Invoice invoice { set; get; }


    }
}
