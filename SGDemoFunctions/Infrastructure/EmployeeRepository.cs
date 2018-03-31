using MongoDB.Driver;
using SGDemoFunctions.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SGDemoFunctions.Infrastructure
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IMongoCollection<Employee> mongoCollection;

        public EmployeeRepository(IMongoCollection<Employee> mongoCollection)
        {
            this.mongoCollection = mongoCollection;
        }

        public async Task<List<Employee>> FindAll()
        {
            var employeesCursor = await mongoCollection.FindAsync(x => true);
            var employees = await employeesCursor.ToListAsync();
            return employees;
        }

        public async Task<Employee> FindOneById(Guid id)
        {
            var employeesCursor = await mongoCollection.FindAsync(x => x.Id == id);
            var employee = await employeesCursor.SingleOrDefaultAsync();
            return employee;
        }

        public async Task Save(Employee employee)
        {
            await mongoCollection.InsertOneAsync(employee);
        }
    }
}
