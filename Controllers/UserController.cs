using clinic_api_project.DTO;
using clinic_api_project.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace clinic_api_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<UserApplication> _usermanger;
        private RoleManager<IdentityRole> _rolemanger;

        public UserController(UserManager<UserApplication> manager, RoleManager<IdentityRole> rolemanger)
        {
            _usermanger = manager;
            _rolemanger = rolemanger;
        }
        [HttpPost("addnewDoctor")]
        public async Task<IActionResult> addnewUser(registerDTO User)

        {
            if (ModelState.IsValid)
            {
                var user1 = await _usermanger.FindByIdAsync(User.Id);
                if (user1 != null) {
                    return BadRequest("there is user with the same name");
                }
                UserApplication user = new UserApplication();
                user.FirstName = User.FirstName;
                user.LastName = User.LastName;
                user.UserName = User.FirstName+User.LastName;
                user.Phone = User.Phone;
                user.Role = User.Role;
                IdentityResult result = await _usermanger.CreateAsync(user, User.Password);
                if (result.Succeeded)
                {
                    var role = await _rolemanger.RoleExistsAsync(User.Role);
                    if (role == true)
                    {
                        var addrole = await _usermanger.AddToRoleAsync(user, User.Role);
                    }
                    return Ok();
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteUser/{id:alpha}")]
        public async Task<IActionResult> delete(string id)
        {
            UserApplication doc = await _usermanger.FindByIdAsync(id);
            if (doc == null)
            {
                return BadRequest("doctor not found");
            }
            var result = await _usermanger.DeleteAsync(doc);
            if (result.Succeeded)
            {
                return Ok($"{doc.UserName} is deleted");
            }
            return BadRequest(result.Errors);
        }
        [HttpGet("getAllDoctor")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _usermanger.GetUsersInRoleAsync("doctor");

            // Return only the required fields
            var result = doctors.Select(a => new
            {
                FirstName = a.FirstName,
                LastName = a.LastName,
                Phone = a.Phone
            });

            return Ok(result);
        }
        [HttpGet("getAllPatient")]
        public async Task<IActionResult> getAllPatient()
        {
            var doctors = await _usermanger.GetUsersInRoleAsync("Patient");

            // Return only the required fields
            var result = doctors.Select(a => new
            {
                FirstName = a.FirstName,
                LastName = a.LastName,
                Phone = a.Phone
            });

            return Ok(result);
        }

        [HttpGet("getProfile/{id:alpha}")]
        public async Task<IActionResult> GetByName(string id)
        {
            var user = await _usermanger.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _usermanger.GetRolesAsync(user);
            return Ok(new
            {
                user = new {name=user.UserName ,Phone=user.Phone ,user.Role},
                roles = roles
            });


        }
        [HttpPut("Upadateprofile/{id:alpha}")]
        public async Task<IActionResult> UpdateDoc(string id, [FromBody] DTOupdate user)
        {
            UserApplication User = await _usermanger.FindByIdAsync(id);
            if (User == null)
            {
                return NotFound();
            }
          
            User.FirstName = user.FirstName;
            User.LastName = user.LastName;
            User.UserName = user.UserName;
            User.Phone = user.Phone;
            IdentityResult result = await _usermanger.UpdateAsync(User);

            if (result.Succeeded)
            {
                return Ok(new { mass = "updated sucessfully" });
            }
            return BadRequest(result.Errors);
        }
    }
    
}
