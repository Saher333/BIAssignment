using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BIAssignment.Models;

namespace BIAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly BIAssignmentContext _context;

        public EmployeesController(BIAssignmentContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<Employee> GetEmployee()
        {
            return _context.Employee;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.Include(eb => eb.EmployeeBenefit).ThenInclude(b => b.Benefit).Select(emp =>
            new EmployeeDTO()
            {
                EmployeeId = emp.EmployeeId,
                Name = emp.Name,
                Dob = emp.Dob,
                Salary = emp.Salary,
                Benefits = emp.EmployeeBenefit.Select(x => x.Benefit).ToList()
            }).SingleOrDefaultAsync(b => b.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            Employee Emp = new Employee() { EmployeeId = employee.EmployeeId, Name = employee.Name, Dob = employee.Dob, Salary = employee.Salary };
            _context.Entry(Emp).State = EntityState.Modified;
                                    
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            List<int> benIds = employee.Benefits.Select(x => x.BenefitId).ToList();

            EmployeeBenefit empBen;
            foreach (Benefit benefit in employee.Benefits) // to add new benefits
            {
                if (_context.EmployeeBenefit.Where(x => x.EmployeeId == employee.EmployeeId && x.BenefitId == benefit.BenefitId).FirstOrDefault() == null)
                {
                    empBen = new EmployeeBenefit()
                    {
                        BenefitId = benefit.BenefitId,
                        EmployeeId = employee.EmployeeId
                    };
                    _context.EmployeeBenefit.Add(empBen);
                    await _context.SaveChangesAsync();
                }
            }

            foreach (int benefitId in _context.Benefit.Where(x => !benIds.Contains(x.BenefitId)).Select(x => x.BenefitId).ToList()) // to delete unchecked benefits
            {
                var EmployeeBenefits = _context.EmployeeBenefit.Where(x => x.EmployeeId == employee.EmployeeId && x.BenefitId == benefitId).ToList();
                foreach (var EmpBen in EmployeeBenefits)
                {
                    _context.EmployeeBenefit.Remove(EmpBen);
                    await _context.SaveChangesAsync();
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Employee Emp = new Employee() { EmployeeId = employee.EmployeeId, Name = employee.Name, Dob = employee.Dob, Salary = employee.Salary };
            _context.Employee.Add(Emp);
            await _context.SaveChangesAsync();

            EmployeeBenefit empBen;
            foreach (Benefit benefit in employee.Benefits) // to add new benefits
            {
                empBen = new EmployeeBenefit()
                {
                    BenefitId = benefit.BenefitId,
                    EmployeeId = Emp.EmployeeId
                };
                _context.EmployeeBenefit.Add(empBen);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetEmployee", new { id = Emp.EmployeeId }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var empBen = _context.EmployeeBenefit.Where(x => x.EmployeeId == id).ToList(); // delete all employee's benefits
            foreach (var item in empBen)
            {
                _context.EmployeeBenefit.Remove(item);
                await _context.SaveChangesAsync();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}