
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
using SGDemoFunctions.Infrastructure;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SGDemoFunctions.Api
{
    public static class HttpGetAllEmployeesFunction_v1
    {
        [FunctionName("HttpGetAllEmployeesFunction_v1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/employees")]HttpRequest req, 
            TraceWriter log)
        {
            log.Info("Get all employees");
            var repository = CreateRepository();
            var employees = await repository.FindAll();
            var employeeDtos = employees.Select(e =>
                new EmployeeDto()
                {
                    Id = e.Id.ToString(),
                    Name = e.Name,
                    JobTitle = e.JobTitle
                });

            return new OkObjectResult(employeeDtos);
        }


        private static IEmployeeRepository CreateRepository()
        {
            string connectionString =
                @"mongodb://sgdemomongo:GWw0QSGv9Lz2lrPJaVfzJZHzfuZDDImpbAR88BH2y3JVFG1YtwSLFjdp23bwldP9YqOlzkljtJn9lgfAA36IvQ==@sgdemomongo.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
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
