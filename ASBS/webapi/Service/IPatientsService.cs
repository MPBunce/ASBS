using webapi.Models;

namespace webapi.Service
{
    public interface IPatientsService
    {

        Task<Patients> Add(Patients patient);
        Task<List<Patients>> GetAll();
        Task<Patients> GetById(int id);
        Task<Patients> Update(Patients patient);
        Task DeleteById(int id);

    }
}
