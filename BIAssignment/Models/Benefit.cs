using System;
using System.Collections.Generic;

namespace BIAssignment.Models
{
    public partial class Benefit
    {
        public Benefit()
        {
            EmployeeBenefit = new HashSet<EmployeeBenefit>();
        }

        public int BenefitId { get; set; }
        public string Name { get; set; }

        public ICollection<EmployeeBenefit> EmployeeBenefit { get; set; }
    }
}
