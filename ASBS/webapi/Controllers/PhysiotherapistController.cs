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



        [HttpGet("GetAllPhysiotherapists"), Authorize(Roles = "Admin")]

        [HttpGet("GetOnePhysiotherapists")]


        //Create app

        //update app

        //delete app



        //Update Physio Therapist

        //Delete Physio Therapist


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
