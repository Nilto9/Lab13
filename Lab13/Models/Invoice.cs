namespace Lab13.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Total { get; set; }

        public Customer? Customer { get; set; }

        public List<Detail>? Detail { get; set; }
    }

}
