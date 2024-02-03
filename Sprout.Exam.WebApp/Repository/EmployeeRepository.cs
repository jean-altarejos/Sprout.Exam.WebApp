using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Models;
using System.Collections.Generic;

namespace Sprout.Exam.WebApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Delete(Employee employee)
        {
            var result = _context.Employees.Find(employee.Id);
            if(result != null)
            {
                _context.Employees.Remove(result);
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            var result = _context.Employees;
            return result;
        }

        public Employee GetById(int id)
        {
            var result = _context.Employees.Find(id);
            return result;
        }

        public void Insert(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            var result = _context.Employees.Find(employee.Id);
            if (result != null)
            {
                result.FullName = employee.FullName;
                result.Birthdate = employee.Birthdate;
                result.Tin = employee.Tin;
                result.TypeId = employee.TypeId;

            }
        }
    }
}
