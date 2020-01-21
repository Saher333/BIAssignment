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
    public class BenefitsController : ControllerBase
    {
        private readonly BIAssignmentContext _context;

        public BenefitsController(BIAssignmentContext context)
        {
            _context = context;
        }

        // GET: api/Benefits
        [HttpGet]
        public IEnumerable<Benefit> GetBenefit()
        {
            return _context.Benefit;
        }
    }
}