using webapi.Models;

namespace webapi.Service
{
    public interface IPhysiotherapistService
    {

        Task<Physiotherapist> Register(Physiotherapist physiotherapist);
        Task<Physiotherapist> Login (string email);

    }
}
