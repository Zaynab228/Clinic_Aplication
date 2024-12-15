using System.ComponentModel.DataAnnotations;

namespace clinic_api_project.DTO
{
    public class UpdateAppointment
    {
        //public string DoctorId { get; set; }
        [Required]
        //public string patientId { get; set; }
       // [Required]
        public DateTime date { get; set; }
        [Required]
        public string Status { get; set; }
        public string notes { get; set; }
    }
}
