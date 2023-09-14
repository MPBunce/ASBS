using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Security.Claims;
using webapi.Models;

namespace webapi.Service
{
    public interface IPhysiotherapistService
    {

        Task<Physiotherapist> Register(Physiotherapist physiotherapist);
        Task<Physiotherapist> Login (string email);
        Task<Physiotherapist> GetPhysiotherapist(string id);
        Task<Physiotherapist> UpdatePhysiotherapist(Physiotherapist physiotherapist);
        Task<string> DeletePhysiotherapist(string id);
        Task<Patient> CreatePatientsAppointment(string patientId, Appointment appointment);
        Task<Patient> ReadPatientHistory(string patientId);
        Task<Patient> UpdatePatientsAppointment(string patientId, Appointment appointment);
        Task<Patient> DeletePatientsAppointment(string patientId, Appointment appointment);


    }
}
