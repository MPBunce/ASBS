using System.Security.Claims;
using webapi.Models;

namespace webapi.Service
{
    public interface IPhysiotherapistService
    {

        Task<Physiotherapist> Register(Physiotherapist physiotherapist);
        Task<Physiotherapist> Login (string email);
        Task<Physiotherapist> GetPhysiotherapist(string id);

    }
}
