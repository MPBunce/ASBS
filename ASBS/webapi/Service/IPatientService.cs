using webapi.Models;

namespace webapi.Service
{
    public interface IPatientService
    {

        Task<Patient> Register(Patient patient);
        Task<Patient> Login(string email);
        Task<Patient> GetPatient(string id);
        Task<Patient> CreateAppointment(string id, Appointment appointment);
        Task<Patient> UpdateAppointment(string patientId, Appointment appointment);
        Task<Patient> DeleteAppointment(string patientId, string appointmentId);
        Task<Patient> UpdateUser(Patient patient);
        Task<string> DeleteUser(string id);

    }
}