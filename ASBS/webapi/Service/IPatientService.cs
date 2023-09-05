using webapi.Models;

namespace webapi.Service
{
    public interface IPatientService
    {

        Task<Patient> Register(Patient patient);
        Task<Patient> Login(String email);

    }
}