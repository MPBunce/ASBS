
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Azure;
using Azure.Communication.Email;


namespace webapi.Communications
{
    public class Communication
    {

        private readonly string connectionString ="";

        public async Task<bool> sendConfirmation(string userEmail, string time, string physio)
        {

            EmailClient emailClient = new EmailClient(connectionString);
            System.Threading.CancellationToken Token = default;

            try
            {
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    WaitUntil.Completed,
                    senderAddress: "DoNotReply@30daec63-345a-4d9e-ac84-a5310ec0b54e.azurecomm.net",
                    recipientAddress: userEmail,
                    subject: "Fast Physio Booking Confirmation",
                    htmlContent: null,
                    plainTextContent: $"Conirming your appointment at {time} with {physio}",
                    cancellationToken: Token
                );


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> sendDelete(string userEmail, string time, string physio)
        {
            EmailClient emailClient = new EmailClient(connectionString);
            System.Threading.CancellationToken Token = default;

            try
            {
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    WaitUntil.Completed,
                    senderAddress: "DoNotReply@30daec63-345a-4d9e-ac84-a5310ec0b54e.azurecomm.net",
                    recipientAddress: userEmail,
                    subject: "Fast Physio Booking Cancelation",
                    htmlContent: null,
                    plainTextContent: $"Your appointment at {time} with {physio} has been cancelled",
                    cancellationToken: Token
                );


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> sendUpdate(string userEmail, string time, string physio)
        {
            EmailClient emailClient = new EmailClient(connectionString);
            System.Threading.CancellationToken Token = default;

            try
            {
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    WaitUntil.Completed,
                    senderAddress: "DoNotReply@30daec63-345a-4d9e-ac84-a5310ec0b54e.azurecomm.net",
                    recipientAddress: userEmail,
                    subject: "Fast Physio Booking Notes",
                    htmlContent: null,
                    plainTextContent: $"Conirming physiotherapist {physio} has update the notes from your appointment",
                    cancellationToken: Token
                );


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
