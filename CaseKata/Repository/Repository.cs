using System;
using System.Linq;
using CaseKata.Models;

namespace CaseKata.Repository
{
    public class Repository : Storable
    {
        private readonly CaseFileContext _context;
           
        public Repository(CaseFileContext context)
        {
            _context = context;
        }

        public void Save(CaseFile casefile)
        {
            casefile.OpenDate = DateTime.Now;
            _context.Add(casefile);
            _context.SaveChanges();
        }

        public CaseFile FindByDocketId(int docketId)
        {
            return _context.CaseFiles.FirstOrDefault(c => c.DocketId == docketId);
        }

        public void Delete(int docketId)
        {
            var caseFiles = _context.CaseFiles.Where(c => c.DocketId == docketId).ToList();
            foreach (var caseFile in caseFiles)
            {
                _context.CaseFiles.Remove(caseFile);
            }
            _context.SaveChanges();
        }
    }
}