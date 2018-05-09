
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Security.Authentication;
using SGDemoFunctions.Infrastructure;
using SGDemoFunctions.Domain;
using System.Threading.Tasks;
using System;

namespace SGDemoFunctions.Api
{
    public static class HttpPostNewEmployeeFunction_v1
    {
        [FunctionName("HttpPostNewEmployeeFunction_v1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/employees")] EmployeeDto employeeDto,
            TraceWriter log)
        {
            log.Info($"Post employee : {employeeDto.Id}");

            var mongoCollection = CreateMongoCollection();
            var repository = new EmployeeRepository(mongoCollection);
            var employee = new Employee()
            {
                Id = Guid.Parse(employeeDto.Id),
                Name = employeeDto.Name,
                JobTitle = employeeDto.JobTitle
            };
            await repository.Save(employee);

            return new OkResult();
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
