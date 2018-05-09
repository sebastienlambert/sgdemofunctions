
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using SGDemoFunctions.Domain;
using MongoDB.Driver;
using System.Security.Authentication;
using System;
using System.Threading.Tasks;
using SGDemoFunctions.Infrastructure;

namespace SGDemoFunctions.Api
{
    public static class HttpGetOneEmployeeFunctions_v1
    {
        [FunctionName("HttpGetOneEmployeeFunctions_v1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/employees/{employeeId}")]HttpRequest req,
            string employeeId,
            TraceWriter log)
        {
            log.Info($"Get employee {employeeId}");

            var mongoCollection = CreateMongoCollection();
            var repository = new EmployeeRepository(mongoCollection);
            var employee = await repository.FindOneById(Guid.Parse(employeeId));
            if (employee == null)
            {
                return new NotFoundObjectResult($"Employee {employeeId} not found");
            }
            else
            {
                var dto = new EmployeeDto
                {
                    Id = employee.Id.ToString(),
                    JobTitle = employee.JobTitle,
                    Name = employee.Name
                };
                return new OkObjectResult(dto);
            }
         }


        private static IMongoCollection<Employee> CreateMongoCollection()
        {
            string connectionString =
   @"mongodb://sgdemocosmodb:xgVR8phzKbf4q5q69OYFqI6OVfPmuNEEbmAojXVxsdGYfkln8T55jyrVTZCCc7saUWgeQNiHhhpb2lI2AnbiqA==@sgdemocosmodb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);


            var mongoDatabase = mongoClient.GetDatabase("SgDemo");
            return mongoDatabase.GetCollection<Employee>("employee");
        }
    }
}
