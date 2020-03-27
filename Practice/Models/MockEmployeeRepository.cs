using System.Collections.Generic;
using System.Linq;

namespace Practice.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employeesList;

        public MockEmployeeRepository()
        {
            _employeesList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Mary", Department = Dept.HR, Email = "mary@pragimtech.com" },
                new Employee() { Id = 2, Name = "John", Department = Dept.IT, Email = "john@pragimtech.com" },
                new Employee() { Id = 3, Name = "Sam", Department = Dept.IT, Email = "sam@pragimtech.com" }
            };
        }
        public Employee GetEmployee(int id)
        {
            return _employeesList.FirstOrDefault(e => e.Id == id);
        }
        public Employee Add(Employee employee)
        {
            employee.Id = _employeesList.Max(e => e.Id) + 1;
            _employeesList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = _employeesList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;


            }

            return employee;
        }

        public Employee Delete(int id)
        {
            var employee = _employeesList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeesList.Remove(employee);
            }

            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeesList;
        }


    }
}