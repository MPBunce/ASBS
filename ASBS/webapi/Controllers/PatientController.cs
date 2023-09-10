using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Models;
using webapi.Service;
using System.Net.Http.Headers;
using System.Net;

namespace webapi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {

        public readonly IPatientService _patientService;
        private readonly IConfiguration _configuration;
        private readonly IPhysiotherapistService _physiotherapistService;

        public PatientController(IPatientService patientsService, IConfiguration configuration, IPhysiotherapistService physiotherapistService)
        {
            _patientService = patientsService;
            _configuration = configuration;
            _physiotherapistService = physiotherapistService;
        }

        public static Patient patient = new Patient();
        public static Appointment appointment = new Appointment();

        [HttpPost("RegisterPatient")]
        public async Task<ActionResult<Patient>> RegisterAsync(Patient request)
        {
            

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

        [HttpPost("Login")]
        public async Task<ActionResult<String>> LoginAsync(Auth request)
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



        [HttpGet("GetUserAndAppointment"), Authorize]
        public async Task<ActionResult<Patient>> GetUserAndAppointment()
        {

            var parameter = "";
            string authorization = Request.Headers["Authorization"];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                // we have a valid AuthenticationHeaderValue that has the following details:
                var scheme = headerValue.Scheme;
                parameter = headerValue.Parameter;

            }
            else
            {
                return BadRequest("User Not Authorized");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(parameter);
            var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (claim == null)
            {
                return BadRequest("User Not Authorized");
            }

            var result = await _patientService.GetPatient(claim.Value);
            return Ok(result);
        }

        [HttpPost("CreateAppointment"), Authorize]
        public async Task<ActionResult<Patient>> CreateAppointment(Appointment request, string physioId)
        {
            var parameter = "";
            string authorization = Request.Headers["Authorization"];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                // we have a valid AuthenticationHeaderValue that has the following details:
                var scheme = headerValue.Scheme;
                parameter = headerValue.Parameter;

            }
            else
            {
                return BadRequest("User Not Authorized");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(parameter);
            var claim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (claim == null)
            {
                return BadRequest("User Not Authorized");
            }

            //Need to select Physiotherapist some home

            appointment.AppointmentId = Guid.NewGuid().ToString();
            appointment.Physiotherapist = await _physiotherapistService.GetPhysiotherapist(physioId);
            appointment.AppointmentDateTime = request.AppointmentDateTime;
            appointment.Duration = request.Duration;
            appointment.Notes = request.Notes;

            var result = await _patientService.CreateAppointment(claim.Value, appointment);

            return Ok(result);

        }

        [HttpPut("UpdateAppointment"), Authorize]

        [HttpDelete("DeleteAppointment"), Authorize]



        [HttpPut("UpdateUser"), Authorize]

        [HttpDelete("DeleteUser"), Authorize]


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
