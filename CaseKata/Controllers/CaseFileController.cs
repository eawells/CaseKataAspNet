using System;
using System.Collections.Generic;
using System.Linq;
using CaseKata.Models;
using CaseKata.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CaseKata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseFileController : ControllerBase
    {
        private readonly Storable _repository;

        public CaseFileController(Storable repository)
        {
            _repository = repository;
        }
        
        [HttpGet("{docketId}")]
        public ActionResult<IList<CaseFile>> RetrieveByDocketId(int docketId)
        {
            var result = _repository.FindByDocketId(docketId);

            if (result == null)
            {
                return NotFound();
            }

            return new List<CaseFile> {result};
        }

        [HttpPost]
        public void Add(CaseFile caseFile)
        {
            _repository.Save(caseFile);
            
        }

        [HttpDelete("{docketId}")]
        public void Remove(int docketId)
        {
            _repository.Delete(docketId);
        }
        
        
    }
}