using System;
using System.Linq;
using CaseKata.Models;
using CaseKata.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CaseKataTest
{
    [TestFixture]
    public class RepositoryTest
    {
        private static Repository _repository;
        private CaseFileContext _context;

        
        [SetUp]
        public void MakeContext()
        {
            var dbContext = new DbContextOptionsBuilder<CaseFileContext>()
                .UseInMemoryDatabase("databaseName")
                .Options;
            _context = new CaseFileContext(dbContext);
            _repository = new Repository(_context);
        }
        
        [Test]
        public void GivenCaseItIsSaved()
        {
            var beforeDate = DateTime.Now;
            var casefile = new CaseFile {DocketId = 1};

            _repository.Save(casefile);

            var result = _context.CaseFiles.Where(f => f.DocketId == 1).ToList();

            var afterDate = DateTime.Now;
            
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].DocketId);
            Assert.IsTrue(result[0].OpenDate > beforeDate && result[0].OpenDate < afterDate);
        }


        [Test]
        public void GivenCaseExistsWhenLookForIt_ItIsReturned()
        {
            var casefile = new CaseFile {DocketId = 2};

            _context.CaseFiles.Add(casefile);
            _context.SaveChanges();

            var result = _repository.FindByDocketId(2);
            
            Assert.AreEqual(2, result.DocketId);
        }


        [Test]
        public void GivenCaseDoesNotExistWhenLookForIt_ItIsNotReturned()
        {
            var casefile = new CaseFile {DocketId = 3};

            _context.CaseFiles.Add(casefile);
            _context.SaveChanges();

            var result = _repository.FindByDocketId(100);
            
            Assert.IsNull(result);
        }

    }
}