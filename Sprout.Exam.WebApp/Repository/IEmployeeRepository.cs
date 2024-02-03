using Sprout.Exam.WebApp.Models;
using System.Collections;
using System.Collections.Generic;

namespace Sprout.Exam.WebApp.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);

        void Insert(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        void Save();

    }
}
