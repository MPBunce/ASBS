using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using webapi.Models;

namespace webapi.Service
{
    public class PhysiotherapistService : IPhysiotherapistService
    {

        private readonly Microsoft.Azure.Cosmos.Container _container;

        public PhysiotherapistService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }



        public Task<Physiotherapist> Login(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Physiotherapist> Register(Physiotherapist physiotherapist)
        {
            var item = await _container.CreateItemAsync<Physiotherapist>(physiotherapist, new PartitionKey(physiotherapist.PhysiotherapistId));
            return item;
        }

    }
}
