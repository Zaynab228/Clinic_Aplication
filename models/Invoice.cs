using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinic_api_project.models
{
    //الدفع
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public virtual UserApplication Patient { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        [ForeignKey("apointment")]//relation one to one between apointment and invoice
        public int apointmentid { get; set; }
        public virtual apointment apointment { get; set; }

    }
}
