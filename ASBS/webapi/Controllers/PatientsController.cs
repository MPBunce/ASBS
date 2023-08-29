using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Service;

namespace webapi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {

        public readonly IPatientsService _patientsService;

        public PatientsController(IPatientsService patientsService)
        {
            _patientsService = patientsService;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sqlQuery = "SELECT * FROM ";
            var result = await _patientsService.GetAll();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Add(Patients patient)
        {

            patient.PatientId = Guid.NewGuid().ToString();
            var result = await _patientsService.Add(patient);
            return Ok(result);

        }


    }
}
