using clinic_api_project.models;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinic_api_project.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; } 
        public string PatientId { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
       
        public int apointmentid { get; set; }
    }
}
