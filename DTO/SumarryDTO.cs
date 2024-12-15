namespace clinic_api_project.DTO
{
    public class SumarryDTO
    {
        public int TotalInvoices { get; set; }
        public int TotalPaidInvoices { get; set; }
        public int TotalUnpaidInvoices { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
