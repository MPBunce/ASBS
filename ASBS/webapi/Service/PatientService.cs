using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using webapi.Models;
using webapi.Communications;

namespace webapi.Service
{
    public class PatientService : IPatientService
    {

        private readonly Microsoft.Azure.Cosmos.Container _container;

        public PatientService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<List<Patient>> GetAll()
        {
            List<Patient> resultList = new List<Patient>();
            string query = $"SELECT DISTINCT * FROM c WHERE IS_DEFINED(c.appointments)";

            var queryResultSetIterator = _container.GetItemQueryIterator<Patient>(query);

            try
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Patient> currentResultSet = await queryResultSetIterator.ReadNextAsync();
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



        public async Task<Patient> Register(Patient patient)
        {
            var item = await _container.CreateItemAsync<Patient>(patient, new PartitionKey(patient.PatientId));
            return item;
        }

        public async Task<Patient> Login(String email)
        {

            List<Patient> resultList = new List<Patient> ();
            string query = $"SELECT DISTINCT * FROM c WHERE c.email = '{email}'";

            var queryResultSetIterator = _container.GetItemQueryIterator<Patient>(query);

            try
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Patient> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var item in currentResultSet)
                    {
                        // Process the retrieved items
                        Console.WriteLine($"Item Id: {item}");

                        // Add the item to the list
                        resultList.Add(item);
                    }
                }


            }
            catch (Exception ex) {
            
                return null;
            }

            if( resultList.Count <= 0)
            {
                return null;
            }

            return resultList[0];

        }

        public async Task<Patient> GetPatient(string id)
        {

            List<Patient> resultList = new List<Patient>();
            string query = $"SELECT DISTINCT * FROM c WHERE c.id = '{id}'";
            var queryResultSetIterator = _container.GetItemQueryIterator<Patient>(query);

            try
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Patient> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var item in currentResultSet)
                    {

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

        public async Task<Patient> CreateAppointment(String id, Appointment appointment)
        {

            ItemResponse<Patient> response = await _container.ReadItemAsync<Patient>(id, new PartitionKey(id));
            Patient patient = response.Resource;

            if(appointment == null)
            {
                return null;
            }

            if(patient.Appointments == null)
            {
                patient.Appointments = new List<Appointment> { appointment };
            }
            else
            {
                patient.Appointments.Add(appointment);
            }
            
            var secondResponse = await _container.ReplaceItemAsync(patient, id, new PartitionKey(id));
            return secondResponse;
        }

        public async Task<Patient> UpdateAppointment(string patientId, Appointment newAppointment)
        {
            ItemResponse<Patient> existingDocument = await _container.ReadItemAsync<Patient>(patientId, new PartitionKey(patientId));
            Patient patient = existingDocument.Resource;

            int count = 0;

            foreach(var appointment in patient.Appointments)
            {
                if(appointment.AppointmentId == newAppointment.AppointmentId)
                {
                    
                    break;
                }
                else
                {
                    count ++;
                }
                
            }
            patient.Appointments[count] = newAppointment;


            var response = await _container.ReplaceItemAsync(patient, patient.PatientId ,new PartitionKey(patient.PatientId));
            return response;
        }

        public async Task<Patient> DeleteAppointment(string patientId, string appointmentId)
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
            var returnApp = patient.Appointments.ElementAt(count);
            string physio = $"{returnApp.Physiotherapist.FirstName} {returnApp.Physiotherapist.LastName}";

            patient.Appointments.RemoveAt(count);

            var response = await _container.ReplaceItemAsync(patient, patient.PatientId, new PartitionKey(patient.PatientId));

            var communicationInstance = new Communication();
            bool result = await communicationInstance.sendDelete(patient.Email, returnApp.AppointmentDateTime, physio);

            return response;

        }

        public async Task<Patient> UpdateUser(Patient patient)
        {
            var response = await _container.ReplaceItemAsync(patient, patient.PatientId, new PartitionKey(patient.PatientId));
            return response;
        }

        public async Task<string> DeleteUser(string id)
        {
            try
            {
                ItemResponse<Patient> existingDocument = await _container.DeleteItemAsync<Patient>(id, new PartitionKey(id));
                return "User deleted";
            }
            catch
            {
                return "Error";
            }
            
        }
    }
}
