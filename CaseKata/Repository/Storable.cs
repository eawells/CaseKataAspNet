using System.Collections.Generic;
using CaseKata.Models;

namespace CaseKata.Repository
{
    public interface Storable
    {
        void Save(CaseFile item);
        CaseFile FindByDocketId(int docketId);
        void Delete(int docketId);
    }
}