using System;
using System.Collections.Generic;
using System.Linq;
using CaseKata.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaseKata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseFileController : ControllerBase
    {
        private readonly CaseFileContext _context;

        public CaseFileController(CaseFileContext context)
        {
            _context = context;
        }
        
        [HttpGet("{docketId}")]
        public ActionResult<IList<CaseFile>> Get(int docketId)
        {
            var caseFiles = _context.CaseFiles
                .Where(c => c.DocketId == docketId)
                .ToList();
            if (caseFiles.Count == 0)
            {
                return NotFound();
            }
            return caseFiles;
        }

        [HttpPost]
        public void Post(CaseFile caseFile)
        {
            _context.CaseFiles.Add(caseFile);
            _context.SaveChanges();
        }
        
        
    }
}