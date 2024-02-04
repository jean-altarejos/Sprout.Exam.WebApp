using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Models;
using Sprout.Exam.WebApp.Repository;
using Sprout.Exam.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //public EmployeesController(IEmployeeRepository employeeRepository)
        //{
        //    _employeeRepository = employeeRepository;
        //}
        private readonly ApplicationDbContext _context;
        public EmployeesController(ApplicationDbContext context)
        {
             _context = context;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.Employees.ToListAsync();

            var employeesDto = from res in result
                               select new EmployeeDto()
                               {
                                   FullName = res.FullName,
                                   Tin = res.Tin,
                                   TypeId = res.TypeId,
                                   Birthdate = res.Birthdate,
                                   Id = res.Id
                               };
            return Ok(employeesDto);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Task.FromResult(_context.Employees.Find(id));

            var employeesDto =  new EmployeeDto
                               {
                                   FullName = result.FullName,
                                   Tin = result.Tin,
                                   TypeId = result.TypeId,
                                   Birthdate = result.Birthdate,
                                   Id = result.Id
                               };

            return Ok(employeesDto);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto employee)
        {
            var item = await Task.FromResult(_context.Employees.Find(employee.Id));
            var editEmp = new Employee()
            {
                FullName = item.FullName,
                Birthdate = item.Birthdate,
                Tin = item.Tin,
                TypeId = item.TypeId,
                Id = item.Id

            };
            _context.Employees.Update(editEmp);
            if (employee == null) return NotFound();
            

            return Ok(editEmp);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            //var item = await Task.FromResult(_context.Employees.Max(m => m.Id) + 1);
            var emp = new Employee
            {
                FullName = input.FullName,
                Tin = input.Tin,
                Birthdate = input.Birthdate.ToString("yyyy-mm-dd"),
                TypeId = input.TypeId,
                
            };

            _context.Add(emp);
            await _context.SaveChangesAsync();

            return Created($"/api/employees/{emp.Id}", emp.Id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Employee id)
        {
            var item = await Task.FromResult(_context.Employees.Find(id));
            _context.Employees.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(id);
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id, decimal absentDays, decimal workedDays)
        {
            var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));

            if (result == null) return NotFound();
            var type = (EmployeeType)result.TypeId;
            return type switch
            {
                EmployeeType.Regular =>
                    //create computation for regular.
                    Ok(25000),
                EmployeeType.Contractual =>
                    //create computation for contractual.
                    Ok(20000),
                _ => NotFound("Employee Type not found")
            };

        }

    }
}
