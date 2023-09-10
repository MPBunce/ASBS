﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using webapi.Models;

namespace webapi.Service
{
    public class PatientService : IPatientService
    {

        private readonly Microsoft.Azure.Cosmos.Container _container;

        public PatientService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
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

    }
}