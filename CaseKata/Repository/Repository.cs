using System.Linq;
using CaseKata.Models;

namespace CaseKata.Repository
{
    public class Repository
    {
        private readonly CaseFileContext _context;
           
        public Repository(CaseFileContext context)
        {
            _context = context;
        }

        public void Save(CaseFile casefile)
        {
            _context.Add(casefile);
            _context.SaveChanges();
        }

        public CaseFile FindByDocketId(int docketId)
        {
            return _context.CaseFiles.FirstOrDefault(c => c.DocketId == docketId);
        }
    }
}