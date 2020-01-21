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
    public class SystemConfigsController : ControllerBase
    {
        private readonly BIAssignmentContext _context;

        public SystemConfigsController(BIAssignmentContext context)
        {
            _context = context;
        }

        // GET: api/SystemConfigs
        [HttpGet]
        public SystemConfig GetSystemConfig()
        {
            return _context.SystemConfig.FirstOrDefault();
        }
    }
}