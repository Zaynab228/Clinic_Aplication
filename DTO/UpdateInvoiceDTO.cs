namespace clinic_api_project.DTO
{
    public class UpdateInvoiceDTO
    {
        public DateTime DateIssued { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
    }
}
