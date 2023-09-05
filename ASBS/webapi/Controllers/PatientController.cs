using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Models;
using webapi.Service;

namespace webapi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {

        public readonly IPatientService _patientService;
        private readonly IConfiguration _configuration;


        public PatientController(IPatientService patientsService, IConfiguration configuration)
        {
            _patientService = patientsService;
            _configuration = configuration;

        }  

        

        public static Patient patient = new Patient();


        [HttpPost("register")]
        public async Task<ActionResult<Patient>> RegisterAsync(Patient request)
        {
            Console.WriteLine(request);

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            patient.PatientId = Guid.NewGuid().ToString();
            patient.FirstName = request.FirstName;
            patient.LastName = request.LastName;    
            patient.PhoneNumber = request.PhoneNumber;
            patient.Email = request.Email;
            patient.Password = passwordHash;
            patient.Appointments = null;

            var result = await _patientService.Register(patient);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Patient>> LoginAsync(Auth request)
        {
            string email = request.Email;
            string password = request.Password;
            
            var result = await _patientService.Login(email);
            if(result == null)
            {
                return BadRequest("Error incorrect email or password");
            }
            
            if(!BCrypt.Net.BCrypt.Verify(password, result.Password))
            {
                return BadRequest("Error incorrect password");
            }


            string token = CreateToken(result, _configuration);



            return Ok(token);
        }



        private string CreateToken(Patient patient, IConfiguration _configuration)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, patient.PatientId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

    }
}
