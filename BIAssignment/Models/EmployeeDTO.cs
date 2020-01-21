using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIAssignment.Models
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public int? Salary { get; set; }
        public List<Benefit> Benefits { get; set; }
    }
}
