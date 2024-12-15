using clinic_api_project.DTO;
using clinic_api_project.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace clinic_api_project.Controllers
{
    public class MidicalRecordController : ControllerBase
    {
        private readonly Context _context;
        public MidicalRecordController(Context context)
        {
            this._context = context;
        }
        [HttpPost("addNewRecord")]
        public async Task<IActionResult> insertRecord(MedicalRecordDTO medicalRecord)
        {
            if (ModelState.IsValid)
            {
                MedicalRecord MR = new MedicalRecord();

                MR.DoctorID = medicalRecord.DoctorID;
                MR.patientID = medicalRecord.patientID;
                MR.Diagnosis = medicalRecord.Diagnosis;
                MR.DateCreated = DateTime.Now;
                MR.Prescription = medicalRecord.Prescription;
                _context.medicicals.Add(MR);
                await _context.SaveChangesAsync();
                return Ok(MR);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("GetMedicalRecord")]
        public async Task<IActionResult> GetById(int id)
        {
            var medicalRecord = await _context.medicicals
                             .Where(m => m.Id == id)
                            .Select(x => new
                            {
                                DoctorID = x.DoctorID,
                                DoctorName = x.Doctor.UserName,
                                Diagnosis = x.Diagnosis,
                                patientName = x.patient.UserName,
                                date = x.DateCreated,
                            })
                           .FirstOrDefaultAsync();
            return Ok(medicalRecord);
        }
        [HttpGet("GetAllMedicalRecord")]
        public async Task<IActionResult> GetAll()
        {
            var medicalRecord = await _context.medicicals

              .Select(x => new
                   {
                       DoctorID = x.DoctorID,
                       DoctorName = x.Doctor.UserName,
                       Diagnosis = x.Diagnosis,
                       patientName = x.patient.UserName,
                       date = x.DateCreated,
             })
             .ToListAsync();
            return Ok(medicalRecord);
          }
        [HttpPut("UpdateMedicalRecord/{id:int}")]
        public async Task<IActionResult> Update(int id ,UpDateMedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                MedicalRecord Oldone = await _context.medicicals.FindAsync(id);
                if (Oldone == null)
                {
                    return NotFound($"No medical record found with ID {id}"); 
                }
                Oldone.Diagnosis = medicalRecord.Diagnosis;
                Oldone.DateCreated = DateTime.Now;
                Oldone.Prescription = medicalRecord.Prescription;
                await _context.SaveChangesAsync();
                return Ok("updated successefully");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("GetRecenteMedicalrecord")]
        public async Task<IActionResult> GetRecenteMedicalrecord(int NumberOfday)
        {
            var cutoffDate = DateTime.Now.AddDays(-NumberOfday);
            var medicalRecord = await _context.medicicals
                             .Where(a=>a.DateCreated>= cutoffDate)
                             .Select(x => new
                             {
                                 DoctorID = x.DoctorID,
                                 DoctorName = x.Doctor.UserName,
                                 Diagnosis = x.Diagnosis,
                                 patientName = x.patient.UserName,
                                 Prescription=  x.Prescription,
                                 date = x.DateCreated,
                             })
                           .ToListAsync();
            return Ok(medicalRecord);
        }
        [HttpGet("GetByPatientId")]
        public async Task<IActionResult> GetByPatientId(string id)
        {
            var medicalRecord = await _context.medicicals
                             .Where(a => a.patientID ==id)
                             .Select(x => new
                             {
                                 DoctorID = x.DoctorID,
                                 DoctorName = x.Doctor.UserName,
                                 Diagnosis = x.Diagnosis,
                                 patientName = x.patient.UserName,
                                 Prescription = x.Prescription,
                                 date = x.DateCreated,
                             })
                           .ToListAsync();
            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            else
            {
                return NotFound("NotFound");
            }
        }
        [HttpGet("GetByDiagnosis/{Diagnosis:alpha}")]
        public async Task<IActionResult> GetByDiagnosis(string Diagnosis)
        {
            var medicalRecord = await _context.medicicals
                             .Where(a => a.Diagnosis == Diagnosis)
                             .Select(x => new
                             {
                                 DoctorID = x.DoctorID,
                                 DoctorName = x.Doctor.UserName,
                                 Diagnosis = x.Diagnosis,
                                 patientName = x.patient.UserName,
                                 Prescription = x.Prescription,
                                 date = x.DateCreated,
                             })
                           .ToListAsync();
            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            else
            {
                return NotFound("NotFound");
            }
        }
        [HttpGet("GetByDoctorId")]
        public async Task<IActionResult> GetByDoctorId(string id)
        {
            var medicalRecord = await _context.medicicals
                             .Where(a => a.DoctorID == id)
                             .Select(x => new
                             {
                                 DoctorID = x.DoctorID,
                                 DoctorName = x.Doctor.UserName,
                                 Diagnosis = x.Diagnosis,
                                 patientName = x.patient.UserName,
                                 Prescription = x.Prescription,
                                 date = x.DateCreated,
                             })
                           .ToListAsync();
            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            else
            {
                return NotFound("NotFound");
            }
        }
        [HttpDelete("RemoveMedicalRecord")]
        public async Task<IActionResult> Remove(int id)
        {
            var medicalRecord = await _context.medicicals.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (medicalRecord != null)
            {
                _context.medicicals.Remove(medicalRecord);
                _context.SaveChanges();
                return Ok("Deleted Sucsessfully");
            }
            return NotFound("medical record not found");
        }
    }
    

}