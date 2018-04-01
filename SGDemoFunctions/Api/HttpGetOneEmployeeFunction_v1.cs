
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using SGDemoFunctions.Domain;
using MongoDB.Driver;
using System.Security.Authentication;
using SGDemoFunctions.Infrastructure;
using System.Threading.Tasks;

namespace SGDemoFunctions.Api
{
    public static class HttpGetOneEmployeeFunction_v1
    {
        [FunctionName("HttpGetOneEmployeeFunction_v1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/employees/{employeeId}")]HttpRequest req, 
            string employeeId,
            TraceWriter log)
        {
            log.Info($"Get employee {employeeId}");

            var repository = CreateRepository();
            var employee = await repository.FindOneById(Guid.Parse(employeeId));

            if (employee == null)
            {
                return new NotFoundObjectResult($"Employee not found : {employeeId}");
            }
            else
            {
                var dto = new EmployeeDto
                {
                    Id = employee.Id.ToString(),
                    Name = employee.Name,
                    JobTitle = employee.JobTitle
                };

                return new OkObjectResult(dto);
            }
        }


        private static IEmployeeRepository CreateRepository()
        {
            string connectionString =
              @"mongodb://sgdemomongodb:E13Dw0NIeYGHEJohNTSsXSwQ7KQVnV73Nkp1k2Z3IRJ05Ol2f1DMDYUhqzrV8N9SqpYvMiNS8zG8pmCsac0J1w==@sgdemomongodb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);

            var mongoDatabase = mongoClient.GetDatabase("SgDemo");
            var employeeCollection = mongoDatabase.GetCollection<Employee>("employee");
            return new EmployeeRepository(employeeCollection);
        }
    }
}
