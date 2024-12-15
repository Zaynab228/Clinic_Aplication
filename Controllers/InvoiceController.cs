using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using clinic_api_project.DTO;
using clinic_api_project.models;
using Microsoft.EntityFrameworkCore;

namespace clinic_api_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly Context _context;

        public InvoiceController(Context _context)
        {
            this._context = _context;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddInvoice(InvoiceDTO invoiceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Invoice invoice = _context.invoices.FirstOrDefault(a => a.Id == invoiceDTO.Id);
            if (invoice != null)
            {
                return BadRequest("InVoice is already esxit");
            }
            else
            {
                Invoice inv = new Invoice();
                inv.Id = invoiceDTO.Id;
                inv.DateIssued = invoiceDTO.DateIssued;
                inv.PatientId = invoiceDTO.PatientId;
                inv.IsPaid = invoiceDTO.IsPaid;
                inv.Amount = invoiceDTO.Amount;
                inv.apointmentid = invoiceDTO.apointmentid;
                _context.invoices.Add(inv);
                _context.SaveChanges();
                return Created(string.Empty, new { message = "Done ya 7abib a5ook" });
            }
        }
        [HttpGet("allInvoice")]
        public async Task<IActionResult> allInvoice()
        {
            var ListOfInvoice = await _context.invoices.Include(a => a.Patient).Select(a => new { a.DateIssued, a.IsPaid, a.Amount, a.Patient.UserName }).ToListAsync();
            if (ListOfInvoice.Any())
            {
                return Ok(ListOfInvoice);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("summary")]
        public async Task<IActionResult> getSummary()
        {
            var numberOfInvoice = await _context.invoices.CountAsync();
            var numberOfInvIsPaid = await _context.invoices.CountAsync(a => a.IsPaid);
            var numOfUnpaid = numberOfInvoice - numberOfInvIsPaid;
            //revenue =>الاربااح
            var Totalrevenue = await _context.invoices.Where(i => i.IsPaid).SumAsync(a => a.Amount);
            SumarryDTO sumarry = new SumarryDTO();
            sumarry.TotalInvoices = numberOfInvoice;
            sumarry.TotalPaidInvoices = numberOfInvIsPaid;
            sumarry.TotalUnpaidInvoices = numOfUnpaid;
            sumarry.TotalRevenue = Totalrevenue;

            return Ok(sumarry);
        }
        [HttpPut("UpdateInvoice/{id:int}")]
        public async Task<IActionResult> UpdateInvoice(int id, UpdateInvoiceDTO invoice)
        {
            if (ModelState.IsValid)
            {
                Invoice oldone = await _context.invoices.Where(i => i.Id == id).FirstOrDefaultAsync();
                if (oldone == null)
                {
                    return NotFound(new { mass = "the invoice is not fund" });
                }
                else
                {
                    oldone.DateIssued = DateTime.Now;
                    oldone.Amount = invoice.Amount;
                    oldone.IsPaid = invoice.IsPaid;
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Invoice updated successfully" });
                }
            }
            return BadRequest(ModelState);
        }
        [HttpGet("GetByRange")]
        public async Task<IActionResult> GetByRange(DateTime? Start, DateTime? End)
        {
            if (Start.HasValue && End.HasValue)
            {
                var invoicesInRange = await _context.invoices
        .Where(i => i.DateIssued == Start.Value)
        .Select(a => new { a.DateIssued, a.IsPaid, a.Amount, a.Patient.UserName }).ToListAsync();
                return Ok(invoicesInRange);
            }
            else
            {
                return BadRequest(new { message = "Both startDate and endDate are required." });
            }

        }
        [HttpGet("GetByDay")]
        public async Task<IActionResult> GetByDay(DateTime? Day)
        {
            if (Day.HasValue)
            {
                var invoicesInRange = await _context.invoices
              .Where(i => i.DateIssued == Day)
              .Select(a => new { a.DateIssued, a.IsPaid, a.Amount, a.Patient.UserName }).ToListAsync();
                return Ok(invoicesInRange);
            }
            else
            {
                return BadRequest(new { message = "The Day is required." });
            }

        }

        [HttpGet("GetUnPaid")]
        public async Task<IActionResult> GetUnPaid()
        {
            var invoicesInRange = await _context.invoices
          .Where(i => i.IsPaid==false)
          .Select(a => new { a.DateIssued, a.IsPaid, a.Amount, a.Patient.UserName }).ToListAsync();
            if (invoicesInRange.Count > 0) { return Ok(invoicesInRange); }
            else
            {
                return BadRequest("there is no UnPaid");
            }
        }
        [HttpGet("GetPaid")]
        public async Task<IActionResult> GetPaid()
        {
            var invoicesInRange = await _context.invoices
          .Where(i => i.IsPaid == true)
          .Select(a => new { a.DateIssued, a.IsPaid, a.Amount, a.Patient.UserName }).ToListAsync();
            if (invoicesInRange.Count > 0) { return Ok(invoicesInRange); }
            else
            {
                return BadRequest("there is no Paid");
            }
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteInvoice (int id)
        {
            Invoice invoice=await _context.invoices.FirstAsync(i => i.Id == id);
            if(invoice == null)
            {
                return NotFound("the invoice not found");
            }
            else
            {
               _context.invoices.Remove(invoice);
               await _context.SaveChangesAsync();
                return Ok("removed sucessfully");
            }

        }
    }
}
