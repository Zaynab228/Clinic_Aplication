using clinic_api_project.DTO;
using clinic_api_project.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
// dont write old verion
namespace clinic_api_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApointmentController : ControllerBase
    {
        private Context _context;
        private readonly UserManager<UserApplication> usermanger;
        private readonly RoleManager<IdentityRole> userRole;

        public ApointmentController(Context context, UserManager<UserApplication> usermanger, RoleManager<IdentityRole> userRole)
        {
            _context = context;
            this.usermanger = usermanger;
            this.userRole = userRole;
        }
        [HttpPost("Book")]
        public async Task<IActionResult> Bookapointment([FromBody] BookDTO apoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dateIsValid = await _context.apointments.
            Where(a => a.DoctorID == apoint.DoctorId && a.DateApointment == apoint.date).FirstOrDefaultAsync();
            if (dateIsValid != null)
            {
                return Conflict("the Doctor All ready booked At this time");
            }
            else
            {
                apointment apointment = new apointment();
                apointment.Id = apoint.id;
                apointment.DateApointment = apoint.date;
                apointment.patientID = apoint.patientId;
                apointment.DoctorID = apoint.DoctorId;
                apointment.Status = "pending";
                apointment.Notes = apoint.notes;
                _context.apointments.Add(apointment);
                _context.SaveChanges();
                return Ok(new { mess = $"{apointment} added suceefully" });
            }

        }
        [HttpGet("getAll")]
        public async Task<IActionResult> Allapointment()
        {
            var list = await _context.apointments.Select(a => new
            {
                a.Id,
                a.DateApointment,
                a.Status,
                DoctorName = a.Doctor.UserName,  // Fetch doctor's name
                PatientName = a.patient.UserName // Fetch patient's name
            })
                .ToListAsync();
            if (list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> GetByApointmentID(int id)
        {
            var appointment = await _context.apointments
    .Include(a => a.Doctor)   // Include the Doctor entity
    .Include(a => a.patient)  // Include the Patient entity
    .Where(a => a.Id == id)
    .Select(a => new
    {
        a.Id,
        a.DateApointment,
        a.Status,
        DoctorName = a.Doctor.UserName,  // Fetch doctor's name
        PatientName = a.patient.UserName // Fetch patient's name
    })
    .FirstOrDefaultAsync();


            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
        [HttpGet("getApointmentWithDoctor/{name:alpha}")]
        public async Task<IActionResult> GetApointmentWithDoctor(string name, [FromQuery] DateTime date)
        {
            // Find the doctor (user) by name
            var user = await usermanger.FindByNameAsync(name);

            if (user == null)
            {
                return NotFound("Doctor not found.");
            }

            var appointments = await _context.apointments
                .Where(a => (a.DoctorID == user.Id || a.patientID == user.Id) && a.DateApointment.Date == date.Date)
                .Select(a=> new
                {
                    a.Id,
                    a.DateApointment,
                    a.Status,
                    DoctorName = a.Doctor.UserName,  // Fetch doctor's name
                    PatientName = a.patient.UserName // Fetch patient's name
                })
                .ToListAsync();

            if (!appointments.Any())
            {
                return NotFound("No appointments found for the given doctor and date.");
            }

            return Ok(appointments);
        }
        [HttpPut("UpdateApointmentStatus/{id:int}")]
        public async Task<IActionResult> UpdateAppointment(int id, UpdateAppointment updateDTO)
        {
            var oldOne = await _context.apointments.FindAsync(id);

            if (oldOne != null)
            {
               // oldOne.patientID = updateDTO.patientId;
               // oldOne.DoctorID = updateDTO.DoctorId;
                oldOne.Status = updateDTO.Status;
                oldOne.Notes = updateDTO.notes;
                oldOne.DateApointment = updateDTO.date;
                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return Ok("Updated successfully");
            }

            return BadRequest("Appointment not found");
        }


        [HttpDelete("Canell")]
        public async Task<IActionResult> Cancell(int id)
        {
            apointment apointment = await _context.apointments.FirstOrDefaultAsync(a => a.Id == id);
            if (apointment != null)
            {
                await _context.SaveChangesAsync();
                return Ok(new { mass = $"{apointment} was deleted sucessfully" });
            }
            return NotFound("appiontment not found");
        }
        [HttpGet("theMostDoctor")]
        public async Task<IActionResult> theMostDoctor()
        {
            var result = _context.apointments
                .GroupBy(a => a.DoctorID) 
                .Select(group => new
                {
                    DoctorID = group.Key,
                    Count = group.Count() 
                })
                .OrderByDescending(g => g.Count) 
                .FirstOrDefault();
            var Doctor = await usermanger.FindByIdAsync(result.DoctorID);
            if (result == null)
            {
                return NotFound("NotFound");
            }
            else
            {
                return Ok(new { Result= result,theNameOfDoctor=Doctor.UserName });
            }
        }
        
    }
}
