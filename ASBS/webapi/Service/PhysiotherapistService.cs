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


        public async Task<Physiotherapist> Register(Physiotherapist physiotherapist)
        {
            var item = await _container.CreateItemAsync<Physiotherapist>(physiotherapist, new PartitionKey(physiotherapist.PhysiotherapistId));
            return item;
        }

        public async Task<Physiotherapist> Login(string email)
        {
            List<Physiotherapist> resultList = new List<Physiotherapist>();
            string query = $"SELECT DISTINCT * FROM c WHERE c.email = '{email}'";

            var queryResultSetIterator = _container.GetItemQueryIterator<Physiotherapist>(query);

            try
            {

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Physiotherapist> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var item in currentResultSet)
                    {
                        // Process the retrieved items
                        Console.WriteLine($"Item Id: {item}");

                        // Add the item to the list
                        resultList.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {

                return null;
            }

            if (resultList.Count <= 0)
            {
                return null;
            }

            return resultList[0];
        }

        public async Task<Physiotherapist> GetPhysiotherapist(string id)
        {
            var item = await _container.ReadItemAsync<Physiotherapist>(id, new PartitionKey(id));
            return item;
        }

        public async Task<Physiotherapist> UpdatePhysiotherapist(Physiotherapist physiotherapist)
        {
            var result = await _container.ReplaceItemAsync(physiotherapist, physiotherapist.PhysiotherapistId, new PartitionKey(physiotherapist.PhysiotherapistId));
            return result;
        }

        public async Task<string> DeletePhysiotherapist(string id)
        {
            try
            {
                ItemResponse<Physiotherapist> existingDocument = await _container.DeleteItemAsync<Physiotherapist>(id, new PartitionKey(id));
                return "User deleted";
            }
            catch
            {
                return "Error";
            }
        }
        
        public async Task<Patient> ReadPatientHistory(string patientId)
        {
            var item = await _container.ReadItemAsync<Patient>(patientId, new PartitionKey(patientId));
            return item;
        }

        public async Task<Patient> CreatePatientsAppointment(string patientId, Appointment newAppointment)
        {
            ItemResponse<Patient> existingDocument = await _container.ReadItemAsync<Patient>(patientId, new PartitionKey(patientId));
            Patient patient = existingDocument.Resource;

            patient.Appointments.Add( newAppointment);                     

            var response = await _container.ReplaceItemAsync(patient, patient.PatientId, new PartitionKey(patient.PatientId));
            return response;
        }

        public async Task<Patient> UpdatePatientsAppointment(string patientId, Appointment newAppointment)
        {
            ItemResponse<Patient> existingDocument = await _container.ReadItemAsync<Patient>(patientId, new PartitionKey(patientId));
            Patient patient = existingDocument.Resource;

            int count = 0;

            foreach (var appointment in patient.Appointments)
            {
                if (appointment.AppointmentId == newAppointment.AppointmentId)
                {

                    break;
                }
                else
                {
                    count++;
                }

            }
            patient.Appointments[count] = newAppointment;

            var response = await _container.ReplaceItemAsync(patient, patient.PatientId, new PartitionKey(patient.PatientId));
            return response;

        }

        public async Task<Patient> DeletePatientsAppointment(string patientId, string appointmentId)
        {
            ItemResponse<Patient> existingDocument = await _container.ReadItemAsync<Patient>(patientId, new PartitionKey(patientId));
            Patient patient = existingDocument.Resource;

            int count = 0;

            foreach (var appointment in patient.Appointments)
            {
                if (appointment.AppointmentId == appointmentId)
                {

                    break;
                }
                else
                {
                    count++;
                }

            }
            patient.Appointments.RemoveAt(count);


            var response = await _container.ReplaceItemAsync(patient, patient.PatientId, new PartitionKey(patient.PatientId));
            return response;
        }
    
        public async Task<List<Physiotherapist>> GetAllPhysiotherapists()
        {
            List<Physiotherapist> resultList = new List<Physiotherapist>();
            string query = $"SELECT DISTINCT * FROM c WHERE IS_DEFINED(c.specialization)";

            var queryResultSetIterator = _container.GetItemQueryIterator<Physiotherapist>(query);

            try
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Physiotherapist> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var item in currentResultSet)
                    {
                        // Process the retrieved items
                        Console.WriteLine($"Item Id: {item}");

                        // Add the item to the list
                        resultList.Add(item);
                    }
                }


            }
            catch (Exception ex)
            {

                return null;
            }

            if (resultList.Count <= 0)
            {
                return null;
            }

            return resultList;

        }

    }
}
