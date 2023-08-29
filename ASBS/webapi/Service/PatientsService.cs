using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using webapi.Models;

namespace webapi.Service
{
    public class PatientsService : IPatientsService
    {

        private readonly Container _container;

        public PatientsService(CosmosClient cosmosClient, string
        databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName,
            containerName);
        }

        public async Task<Patients> Add(Patients patient)
        {
            var item = await _container.CreateItemAsync<Patients>(patient, new PartitionKey(patient.PatientId));
            return item;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Patients>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Patients> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Patients> Update(Patients patient)
        {
            throw new NotImplementedException();
        }
    }
}
