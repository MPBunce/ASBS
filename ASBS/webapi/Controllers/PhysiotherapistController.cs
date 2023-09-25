using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using webapi.Models;
using webapi.Service;

namespace webapi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PhysiotherapistController : ControllerBase
    {

        public readonly IPhysiotherapistService _physioService;
        private readonly IConfiguration _configuration;


        public PhysiotherapistController(IPhysiotherapistService physioService, IConfiguration configuration)
        {
            _physioService = physioService;
            _configuration = configuration;

        }


        public static Physiotherapist physio = new Physiotherapist();
        public static Appointment appointment = new Appointment();


        [HttpPost("RegisterPhysiotherapist")]
        public async Task<ActionResult<Physiotherapist>> RegisterAsync(Physiotherapist request)
        {


            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            physio.PhysiotherapistId = Guid.NewGuid().ToString();
            physio.FirstName = request.FirstName;
            physio.LastName = request.LastName;
            physio.ContactNumber = request.ContactNumber;
            physio.Email = request.Email.ToLower();
            physio.Password = passwordHash;
            physio.Specialization = request.Specialization;


            var result = await _physioService.Register(physio);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<String>> LoginAsync(Auth request)
        {
            string email = request.Email.ToLower();
            string password = request.Password;

            var result = await _physioService.Login(email);
            if (result == null)
            {
                return BadRequest("Error incorrect email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, result.Password))
            {
                return BadRequest("Error incorrect password");
            }


            string token = CreateToken(result, _configuration);
            return Ok(new { Token = token });
        }

        [HttpGet("GetAllPhysiotherapists")]
        public async Task<ActionResult<List<Physiotherapist>>> GetAllPhysiotherapists()
        {
            var result = await _physioService.GetAllPhysiotherapists();
            return result;
        }

        [HttpGet("GetOnePhysiotherapists"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Physiotherapist>> GetOnePhysiotherapist()
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

            var result = await _physioService.GetPhysiotherapist(claim.Value);
            return Ok(result);
        }


        //Update Physio Therapist
        [HttpPut("UpdatePhysiotherapist"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Physiotherapist>> UpdatePhysiotherapist(Physiotherapist newPhysiotherapist)
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
            if(claim.Value != newPhysiotherapist.PhysiotherapistId)
            {
                return BadRequest("Issue with ID");
            }

            var currentUser = await _physioService.GetPhysiotherapist(claim.Value);

            physio.PhysiotherapistId = currentUser.PhysiotherapistId;
            physio.FirstName = newPhysiotherapist.FirstName;
            physio.LastName = newPhysiotherapist.LastName;
            physio.ContactNumber = newPhysiotherapist.ContactNumber;
            physio.Email = newPhysiotherapist.Email.ToLower();
            physio.Specialization = newPhysiotherapist.Specialization;


            if (!BCrypt.Net.BCrypt.Verify(newPhysiotherapist.Password, currentUser.Password))
            {

                physio.Password = BCrypt.Net.BCrypt.HashPassword(newPhysiotherapist.Password);

            } else {
                physio.Password = currentUser.Password;
            }

            var response = await _physioService.UpdatePhysiotherapist(physio);
            return Ok(response);   

        }



        //Delete Physio Therapist
        [HttpDelete("DeletePhysiotherapist"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> DeletePhysiotherapist()
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


            var response = await _physioService.DeletePhysiotherapist(claim.Value);
            return Ok(response);

        }

        //Read History
        [HttpGet("ReadPatientHistory"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Patient>> ReadPatientHistory(string id)
        {
            var response = await _physioService.ReadPatientHistory(id);
            return Ok(response);
        }
        
        
        //Create app
        [HttpPut("CreatePatientsAppointment"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Patient>> CreatePatientsAppointment(string id, Appointment appointment)
        {
            appointment.AppointmentId = Guid.NewGuid().ToString();
            var response = await _physioService.CreatePatientsAppointment(id, appointment);
            return Ok(response);
        }

        //update app
        [HttpPut("UpdatePatientsAppointment"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Patient>> UpdatePatientsAppointment(string id, Appointment appointment)
        {
            var response = await _physioService.UpdatePatientsAppointment(id, appointment);
            return Ok(response);
        }

        //delete app
        [HttpDelete("DeletePatientsAppointment"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Patient>> DeletePatientsAppointment(string id, string appointmentId)
        {
            var response = await _physioService.DeletePatientsAppointment(id, appointmentId);
            return Ok(response);
        }

        private string CreateToken(Physiotherapist physiotherapist, IConfiguration _configuration)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, physiotherapist.PhysiotherapistId),
                new Claim(ClaimTypes.Role, "Admin")
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
