using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace clinic_api_project.models
{
    public class UserApplication:IdentityUser
    {
        //[Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        //public int ID { set;get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
       public virtual Address address { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        //Relations 
        public virtual ICollection<MedicalRecord> DOCRecords { get; set; }//for doctors and MedicalRecord
        public virtual ICollection<apointment> Apointments{ get; set; }
        public virtual ICollection<apointment> PationApointments { get; set; }
        public virtual MedicalRecord PatientRecord { get; set; }//for doctors and MedicalRecord

        public virtual ICollection<UserApplication> patient { get; set; }//for doctors and MedicalRecord
        public virtual ICollection<UserApplication> Doctors { get; set; }
    }

    public class Address
    {
        [Key]
        public string Id { get; set; }    
        public string City { get; set; }
        public string Country { get; set; }
        public string zipcode { get; set; }

        [ForeignKey("UserApplication")]  
        public string UserApplicationID { get; set; }
        public virtual UserApplication UserApplication { get; set; }

    }
}
