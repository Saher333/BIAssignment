using System;
using System.Collections.Generic;

namespace BIAssignment.Models
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeBenefit = new HashSet<EmployeeBenefit>();
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public int? Salary { get; set; }

        public ICollection<EmployeeBenefit> EmployeeBenefit { get; set; }
    }
}
