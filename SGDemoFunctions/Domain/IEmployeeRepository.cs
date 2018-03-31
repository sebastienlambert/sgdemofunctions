using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SGDemoFunctions.Domain
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> FindAll();
        Task<Employee> FindOneById(Guid id);
        Task Save(Employee employee);
    }
}
