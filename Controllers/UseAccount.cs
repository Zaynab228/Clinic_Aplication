using clinic_api_project.DTO;
using clinic_api_project.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
//The problem arises because you're mixing the namespaces.
//You need to ensure that both UserManager and RoleManager
//are from the Microsoft.AspNetCore.Identity namespace.
namespace clinic_api_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UseAccount : ControllerBase
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor for dependency injection
        public UseAccount(UserManager<UserApplication> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(registerDTO registerDTO)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the user already exists
            var existingUser = await _userManager.FindByNameAsync(registerDTO.FirstName);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "User already exists." });
            }

            // Create a new UserApplication instance from the DTO
            UserApplication user = new UserApplication
            {
                Id = registerDTO.Id,
                UserName = registerDTO.FirstName + registerDTO.LastName,
                LastName = registerDTO.LastName,
                FirstName = registerDTO.FirstName,
                Role = registerDTO.Role,
                Phone = registerDTO.Phone // Assuming `Phone` corresponds to `PhoneNumber`
            };

            // Create the user and hash the password
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                // Check if the role exists
                var roleExists = await _roleManager.RoleExistsAsync(registerDTO.Role);
                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(registerDTO.Role));
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                }

                // Assign role to user
                var roleAssignedResult = await _userManager.AddToRoleAsync(user, registerDTO.Role);
                if (!roleAssignedResult.Succeeded)
                {
                    foreach (var err in roleAssignedResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                    return BadRequest(ModelState);
                }

                return Ok(new { Message = "User registered and role assigned successfully." });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO lgindto)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized(ModelState);
            }
            UserApplication user = await _userManager.FindByNameAsync(lgindto.UserName);
            if (user == null)
            {
                return Unauthorized(new { Message = "No user with this name" });
            }
            bool found = await _userManager.CheckPasswordAsync(user, lgindto.Password);
            if (!found)
            {
                return Unauthorized("Incorrect password");
            }

            // The process of creating token
            // 1. Create claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create roles of user to add to claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 2. Create key
            SecurityKey jwtkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ncwkru3408vnhfdni"));

            // 3. Choose algorithm and encode with key
            SigningCredentials signingCredentials = new SigningCredentials(jwtkey, SecurityAlgorithms.HmacSha256);

            // 4. Generate the token
            JwtSecurityToken Token = new JwtSecurityToken(
                             issuer: "https://localhost:7137/",
                             audience: "https://localhost:4200/",
                             claims: claims,
                             expires: DateTime.Now.AddHours(1),
                             signingCredentials: signingCredentials);

            // Create the token
            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(Token) });
        }
    }
}
