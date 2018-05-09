
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
            var mongoCollection = CreateMongoCollection();
            var repository = new EmployeeRepository(mongoCollection);
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


        private static IMongoCollection<Employee> CreateMongoCollection()
        {
            var mongoClient = MongoClientFactory.Create();
            var mongoDatabase = mongoClient.GetDatabase("SgDemo");
            return mongoDatabase.GetCollection<Employee>("employee");
        }
    }
}
