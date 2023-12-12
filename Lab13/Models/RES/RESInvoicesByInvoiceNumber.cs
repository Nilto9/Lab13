namespace Lab13.Models.RES
{
    public class RESInvoicesByInvoiceNumber
    {
        //Customer
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerDocumentNumber { get; set; }

        //Invoices
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceTotal { get; set; }

    }
}
