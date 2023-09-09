using webapi.Models;

namespace webapi.Service
{
    public interface IPatientService
    {

        Task<Patient> Register(Patient patient);
        Task<Patient> Login(string email);
        Task<Patient> GetPatient(string id);
        Task<Patient> CreateAppointment(string id, Appointment appointment);


    }
}