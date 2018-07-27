using Microsoft.EntityFrameworkCore;

namespace CaseKata.Models
{
    public class CaseFileContext : DbContext
    {
        public CaseFileContext(DbContextOptions<CaseFileContext> options)
            : base(options)
        {
        }
        
        public DbSet<CaseFile> CaseFiles { get; set; }
    }
}