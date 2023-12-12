namespace Lab13.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentNumber { get; set; }

        // Propiedades de navegación
        //public List<Invoice> Invoices { get; set; }

        public bool Active { get; set; }
    }
}
