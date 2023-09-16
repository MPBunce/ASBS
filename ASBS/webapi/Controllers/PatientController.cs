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
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;

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

        [HttpGet("HelloWorld")] 
        public string Get()
        {
            return "Hellow World";
        }

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
        public async Task<ActionResult<JSON>> LoginAsync(Auth request)
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
            return Ok(new { Token = token });
        }


        [HttpGet("GetUserAndAppointment"), Authorize(Roles = "User")]
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

        [HttpPost("CreateAppointment"), Authorize(Roles = "User")]
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
            if (physioId == null)
            {
                return BadRequest("Issue finding physio");
            }

            appointment.AppointmentId = Guid.NewGuid().ToString();
            appointment.Physiotherapist = await _physiotherapistService.GetPhysiotherapist(physioId);
            appointment.AppointmentDateTime = request.AppointmentDateTime;
            appointment.Duration = request.Duration;
            appointment.Notes = request.Notes;

            var result = await _patientService.CreateAppointment(claim.Value, appointment);

            return Ok(result);

        }

        [HttpPut("UpdateAppointment"), Authorize(Roles = "User")]
        public async Task<ActionResult<Patient>> UpdateAppointment(Appointment request, string physioId)
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
            if (physioId == null)
            {
                return BadRequest("Issue finding physio");
            }

            appointment.AppointmentId = request.AppointmentId;
            appointment.Physiotherapist = await _physiotherapistService.GetPhysiotherapist(physioId);
            appointment.AppointmentDateTime = request.AppointmentDateTime;
            appointment.Duration = request.Duration;
            appointment.Notes = request.Notes;


            var result = await _patientService.UpdateAppointment(claim.Value, appointment);

            return Ok(result);

        }

        [HttpDelete("DeleteAppointment"), Authorize(Roles = "User")]
        public async Task<ActionResult<Patient>> DeleteAppointment(string appointmentId)
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
            if (appointmentId == null)
            {
                return BadRequest("Issue finding appointment");
            }


            var result = await _patientService.DeleteAppointment(claim.Value, appointmentId);

            return Ok(result);
        }


        [HttpPut("UpdateUser"), Authorize(Roles = "User")]
        public async Task<ActionResult<Patient>> UpdateUser(Patient request)
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
        
            if(claim.Value != request.PatientId)
            {
                return BadRequest("Error with id");
            }

            //Get Existing using id
            var currentUser = await _patientService.GetPatient(claim.Value);

            //Update new info
            patient.FirstName = request.FirstName;
            patient.LastName = request.LastName;
            patient.PhoneNumber = request.PhoneNumber;
            patient.Email = request.Email;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, currentUser.Password))
            {

                patient.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            } else {
                patient.Password = currentUser.Password;
            }            

            patient.Appointments = new List<Appointment>();
            patient.Appointments = currentUser.Appointments;


            //Send Update
            var updateResult = await _patientService.UpdateUser(patient);

            return Ok(updateResult);

        }


        [HttpDelete("DeleteUser"), Authorize(Roles = "User")]
        public async Task<ActionResult<string>> DeleteUser()
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

            var result = await _patientService.DeleteUser(claim.Value);
            return result;

        }

        private string CreateToken(Patient patient, IConfiguration _configuration)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, patient.PatientId),
                new Claim(ClaimTypes.Role, "User")
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
