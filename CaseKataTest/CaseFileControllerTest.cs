using System;
using System.Data;
using CaseKata.Controllers;
using CaseKata.Models;
using CaseKata.Repository;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Moq;
using NUnit.Framework;

namespace CaseKataTest
{
    public class CaseFileControllerTest
    {
        [Test]
        public void GivenANewCaseFileItIsStored()
        {
            var mockedStorable = new Mock<Storable>();
            var controller = new CaseFileController(mockedStorable.Object);
            var casefile = new CaseFile();
            
            controller.Add(casefile);
            
            mockedStorable.Verify(s => s.Save(casefile)); 
        }

        [Test]
        public void GivenAnExistingCaseFileWhenItIsLookedForThenItIsFound()
        {
            var mockedStorable = new Mock<Storable>();
            var controller = new CaseFileController(mockedStorable.Object);
            var casefile = new CaseFile {DocketId = 1};

            mockedStorable.Setup(s => s.FindByDocketId(1)).Returns(casefile);

            var result = controller.RetrieveByDocketId(1);

            Assert.AreEqual(1, result.Value.Count);
            
            Assert.AreSame(casefile, result.Value[0]);
        }

        [Test]
        public void GivenTheDatabaseIsInBadStateWhenTheDatabaseThrowsAnErrorThenTheErrorIsBubbledToTheTopOfTheApplication()
        {
            var mockedStorable = new Mock<Storable>();
            var controller = new CaseFileController(mockedStorable.Object);

            mockedStorable.Setup(s => s.FindByDocketId(1)).Throws<ConstraintException>();

            try
            {
                controller.RetrieveByDocketId(1);
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.IsInstanceOf<ConstraintException>(e);
            }
        }
    }
}