using clinic_api_project.models;

namespace clinic_api_project.DTO
{
    public class registerDTO
    {
      
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
