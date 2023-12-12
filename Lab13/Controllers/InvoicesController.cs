using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab13.Models;
using Lab13.Models.RES;

namespace Lab13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly Context _context;

        public InvoicesController(Context context)
        {
            _context = context;
        }



        [HttpGet("SearchByCustomerName")]
        public async Task<ActionResult<IEnumerable<Invoice>>> SearchInvoicesByCustomerName([FromQuery] string customerName)
        {
            var query = _context.Invoices
                .Include(i => i.Customer)
                .Where(i => EF.Functions.Like(i.Customer.FirstName, $"%{customerName}%") ||
                            EF.Functions.Like(i.Customer.LastName, $"%{customerName}%"))
                .OrderByDescending(i => i.Customer.LastName); 

            var invoices = await query.ToListAsync();

            if (invoices == null || !invoices.Any())
            {
                return NotFound();
            }

            return invoices;
        }

        [HttpGet("GetInvoiceByInvoiceNumber")]
        public async Task<ActionResult<IEnumerable<RESInvoicesByInvoiceNumber>>> GetInvoiceByInvoiceNumber([FromQuery] string invoiceNumber)
        {
            var query = _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Detail)
                    .ThenInclude(detail => detail.Product)  // Ajusta según tu modelo
                .Where(i => EF.Functions.Like(i.InvoiceNumber, $"%{invoiceNumber}%"))
                .OrderBy(i => i.Customer.LastName)
                .ThenBy(i => i.InvoiceNumber);

            var invoices = await query.ToListAsync();

            if (invoices == null || !invoices.Any())
            {
                return NotFound();
            }


            var response = invoices.Select(invoice => new RESInvoicesByInvoiceNumber
            {
                InvoiceNumber = invoice.InvoiceNumber,
                Date = invoice.Date,
                InvoiceTotal = invoice.Total,
                CustomerId = invoice.Customer.CustomerId,
                CustomerFirstName = invoice.Customer.FirstName,
                CustomerLastName = invoice.Customer.LastName,
                CustomerDocumentNumber = invoice.Customer.DocumentNumber
            }).OrderBy(res => res.CustomerLastName).ThenBy(res => res.InvoiceNumber).ToList();

            return response;
        }

        [HttpGet("DetailsByDate")]
        public async Task<ActionResult<IEnumerable<RESInvoicesDetailsByDate>>> GetInvoiceDetailsByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var query = _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Detail)
                 .ThenInclude(detail => detail.Product)
                .Where(i => i.Date >= startDate && i.Date <= endDate)
                .OrderBy(i => i.Date);
                //.ThenBy(detail => detail.Product.Name); // Ajusta según tu modelo

            var invoices = await query.ToListAsync();

            if (invoices == null || !invoices.Any())
            {
                return NotFound();
            }

            var response = invoices.Select(invoice => new RESInvoicesDetailsByDate
            {
                InvoiceNumber = invoice.InvoiceNumber,
                Date = invoice.Date,
                InvoiceTotal = invoice.Total,
                CustomerId = invoice.Customer.CustomerId,
                CustomerFirstName = invoice.Customer.FirstName,
                CustomerLastName = invoice.Customer.LastName,
                CustomerDocumentNumber = invoice.Customer.DocumentNumber
            }).OrderBy(res => res.Date).ThenBy(res => res.DetailProduct).ToList();

            return response;
        }


        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
          if (_context.Invoices == null)
          {
              return NotFound();
          }
            return await _context.Invoices.ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
          if (_context.Invoices == null)
          {
              return NotFound();
          }
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
          if (_context.Invoices == null)
          {
              return Problem("Entity set 'Context.Invoices'  is null.");
          }
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            if (_context.Invoices == null)
            {
                return NotFound();
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvoiceExists(int id)
        {
            return (_context.Invoices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
