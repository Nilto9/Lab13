namespace Lab13.Models.RES
{
    public class RESInvoicesDetailsByDate
    {
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal InvoiceTotal { get; set; }

        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerDocumentNumber { get; set; }

        // Propiedades del detalle
        public int DetailId { get; set; }
        public string DetailProduct { get; set; }
        public string DetailDescription { get; set; }
        public decimal DetailAmount { get; set; }
    }

}
