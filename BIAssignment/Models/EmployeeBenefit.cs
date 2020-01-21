using System;
using System.Collections.Generic;

namespace BIAssignment.Models
{
    public partial class EmployeeBenefit
    {
        public int EmployeeBenefitId { get; set; }
        public int EmployeeId { get; set; }
        public int BenefitId { get; set; }

        public Benefit Benefit { get; set; }
        public Employee Employee { get; set; }
    }
}
