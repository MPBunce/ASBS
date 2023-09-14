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

        [HttpPost("RegisterPhysiotherapist")]
        public async Task<ActionResult<Physiotherapist>> RegisterAsync(Physiotherapist request)
        {


            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            physio.PhysiotherapistId = Guid.NewGuid().ToString();
            physio.FirstName = request.FirstName;
            physio.LastName = request.LastName;
            physio.ContactNumber = request.ContactNumber;
            physio.Email = request.Email;
            physio.Password = passwordHash;
            physio.Specialization = request.Specialization;
            physio.WorkingDays = request.WorkingDays;
            physio.WorkingHoursStart = request.WorkingHoursStart;
            physio.WorkingHoursEnd = request.WorkingHoursEnd;

            var result = await _physioService.Register(physio);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<String>> LoginAsync(Auth request)
        {
            string email = request.Email;
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
            return Ok(token);
        }

        [HttpGet("GetAllPhysiotherapists")]


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
        [HttpPut("UpdateUser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Physiotherapist>> UpdatePhysiotherapist(Physiotherapist physiotherapist)
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
            if(claim.Value != physiotherapist.PhysiotherapistId)
            {
                return BadRequest("Issue with ID");
            }

            var response = await _physioService.UpdatePhysiotherapist(physiotherapist);
            return Ok(response);   

        }
        //Delete Physio Therapist
        [HttpDelete("UpdateUser"), Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> UpdatePhysiotherapist(string id)
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
            if (claim.Value != id)
            {
                return BadRequest("Issue with ID");
            }

            var response = await _physioService.DeletePhysiotherapist(id);
            return Ok(response);

        }

        //Read Histort
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
        [HttpPut("DeletePatientsAppointment"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Patient>> DeletePatientsAppointment(string id, Appointment appointment)
        {
            var response = await _physioService.DeletePatientsAppointment(id, appointment);
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
